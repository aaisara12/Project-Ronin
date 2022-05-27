using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthStat
{
    int health { get; set; }
    int maxHealth { get; set; }

    event System.Action<HealthInfo> OnHealthChanged;
    event System.Action<HealthStat> OnDied;

    void TakeDamage(int damage);

}

public class HealthStat : MonoBehaviour, IHealthStat
{
    // Using property paired with field so we can assign in inspector
    [SerializeField]
    private int _health = 100;

    // Using property so that proper processing is done when changing health internally
    public int health
    {
        get { return _health; }

        // Setter is public in cases where we want to hard set health to some value (think Hearthstone)
        set
        {
            bool hasBeenDamaged = value < _health;
            _health = GetClampedHealth(value);
            SendHealthUpdate(hasBeenDamaged);
            
        }
    }

    [SerializeField]
    private int _maxHealth = 100;
    public int maxHealth
    {
        get { return _maxHealth; }
        set
        {
            if (value < 1) { return; }

            // Make sure new health fits within new bounds
            _health = GetClampedHealth(_health);

            _maxHealth = value;
            SendHealthUpdate(false);
        }
    }

    public event System.Action<HealthInfo> OnHealthChanged;
    public event System.Action<HealthStat> OnDied;

    void Start()
    {
        // Update the UI, which registers during awake
        SendHealthUpdate(false);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        MainDamagePopup.Create(transform.position + new Vector3(4,0,1), damage);
        if(health <= 0)
        {
            OnDied?.Invoke(this);
            GetComponent<Animator>().SetTrigger("dead");
        }
    }



    // Keep health event call details in one place only (DRY code)
    void SendHealthUpdate(bool damaged) => OnHealthChanged?.Invoke(new HealthInfo { current = health, max = maxHealth, isDamaged = damaged });

    // Prevent health from going beyond boundaries
    int GetClampedHealth(int newHealth)
    {
        if (newHealth > maxHealth)
            return maxHealth;
        else if (newHealth < 0)
            return 0;
        else
            return newHealth;
    }
}

// Struct used to make it cleaner to pass health data around
public struct HealthInfo
{
    public float current;
    public float max;
    public bool isDamaged;
}
