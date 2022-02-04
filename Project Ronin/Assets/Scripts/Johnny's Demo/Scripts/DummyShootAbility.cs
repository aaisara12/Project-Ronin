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

    public override void InitiateAbility(AttributeSet inUser)
    {
        base.InitiateAbility(inUser);

        rb.MovePosition(user.transform.position);
        //rb.MoveRotation(user.transform.rotation);
        rb.velocity = user.transform.forward * speed;
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
