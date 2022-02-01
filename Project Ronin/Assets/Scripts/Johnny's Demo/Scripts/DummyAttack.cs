using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttack : Ability
{
    public override void ResetAbility()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        ForEachAttr((AttributeSet attr) =>
        {
            var diffPos = attr.transform.position - user.transform.position;
            float distance = diffPos.magnitude;

            return attr != user && diffPos.magnitude <= 5;
        },
        (AttributeSet attr) =>
        {
            attr.ModifyFloat("hp", -50);
        });

        RecycleAbility();
    }
}
