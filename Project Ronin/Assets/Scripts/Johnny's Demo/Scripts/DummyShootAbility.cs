using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyShootAbility : Ability
{
    [SerializeField]
    float maxDuration;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    Rigidbody rb;
    float timeElapsed = 0;

    bool happened = false;

    public override void ResetAbility()
    {
        timeElapsed = 0;
        happened = false;
    }

    public override void InitiateAbility()
    {
        rb.velocity = new Vector3(0, 0, speed);
    }

    void Update()
    {
        if (happened || timeElapsed > maxDuration)
        {
            RecycleAbility();
            return;
        }

        timeElapsed += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AttributeSet target = AttributeLookup(collision.gameObject);
        if (target != user)
        {
            target.ModifyFloat("hp", -30);
            happened = true;
        } 
    }

    
}
