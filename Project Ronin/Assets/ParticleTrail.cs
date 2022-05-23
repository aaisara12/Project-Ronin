using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleTrail : MonoBehaviour
{
    // Take a reference to the particle trail
    public VisualEffect visualEffect;

    // Take a reference to an animator
    public Animator animator;

    // This is not a good way to do this, but we reference a string that is the animation name
    public string moveAnimationName;

    // Update is called once per frame
    void Update()
    {

        // We need to update the particle's initial velocity
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        visualEffect.SetVector3("Velocity", velocity);

        // If we're not moving, set the emit rate to 3f
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(moveAnimationName))  
        {
            visualEffect.SetFloat("EmitRate", 3f);
        }
        else
        {
            visualEffect.SetFloat("EmitRate", 0f);
        }      
    }
}
