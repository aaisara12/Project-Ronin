using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageReactor : MonoBehaviour
{
    AttributeSet attributeSet;
    Animator animator;
    UnityAction handleHealthChange;
    float previousHealth;

    [SerializeField] GameObject damagedParticleEffectPrefab;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        handleHealthChange = HandleHealthChange;

        if(AttributeSet.objectToAttributes.ContainsKey(gameObject))
        {
            attributeSet = AttributeSet.objectToAttributes[gameObject];
            attributeSet.RegisterOnAttributeChange(handleHealthChange);

            previousHealth = attributeSet.GetFloat("hp");
        }
    }

    void HandleHealthChange()
    {
        float newHealth = attributeSet.GetFloat("hp");

        if(newHealth < previousHealth)
        {
            if(damagedParticleEffectPrefab != null)
            {
                var prefab = Instantiate(damagedParticleEffectPrefab, transform.position, Quaternion.identity);
                Destroy(prefab, 2); // Later on we may want some particle manager that pools these

                AudioManager.instance?.PlaySound("bloody-impact");
                animator.SetTrigger("backoff");
            }
            previousHealth = newHealth;
        }

        if(newHealth <= 0)
        {
            Destroy(gameObject);    // Ideally we'd just want to add this enemy back to the pooling system
        }
    }
}
