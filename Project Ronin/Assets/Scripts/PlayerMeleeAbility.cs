using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAbility : Ability
{ 
    [SerializeField] float damage = 10;
    [SerializeField] float maxEnemiesHit = 3;   // How many enemies we can hit at most
    [SerializeField] float duration = 0.5f; // How long the hitbox should linger

    List<AttributeSet> enemiesHit = new List<AttributeSet>();

    float timeStart;
    void Update()
    {
        if(Time.time > timeStart + duration)
        {
            RecycleAbility();
        }
    }
    
    // Use OnTriggerEnter because it's easier to shape/modify the hitbox using a collider
    void OnTriggerEnter(Collider other)
    {
        var attSet = AttributeLookup(other.gameObject);

        // Make sure we don't hit enemies twice and more than we said we would
        if(attSet != user && !enemiesHit.Contains(attSet) && enemiesHit.Count < maxEnemiesHit)
        {
            //Debug.LogFormat("Hit {0}", other.name);
            attSet.ModifyFloat("hp", -damage);
            attSet.AddTag("backoff");
            enemiesHit.Add(attSet);
        }
        
    }

    public override void ResetAbility()
    {
        base.ResetAbility();
        enemiesHit.Clear();
    }

    public override void InitiateAbility(AttributeSet inUser)
    {
        base.InitiateAbility(inUser);
        timeStart = Time.time;
    }

}
