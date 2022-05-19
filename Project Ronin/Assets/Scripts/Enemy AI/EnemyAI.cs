using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Right now it's still a prototype.
/// The idea is simple: try to emulate a player, and put input to the animator regardless of its states.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    float attackRange = 0f;
    [SerializeField]
    float attackCooldown = 3f;

    [SerializeField]
    Animator animator = null;

    GameObject playerObj = null;

    void Start()
    {
        // grab the player
        playerObj = GameObject.FindWithTag("Player");
        if (!animator) animator = gameObject.GetComponent<Animator>();
    }

    float nextAttackTime = 0;
    void Update()
    {
        Vector3 playerPos = playerObj.transform.position;

        float distance = (transform.position - playerPos).magnitude;
        
        if (distance < attackRange)
        {
            if(nextAttackTime <= Time.time)
            {
                animator.SetTrigger("attack");
                nextAttackTime += attackCooldown;
            }
            
            animator.SetFloat("xInput", 0);
            animator.SetFloat("yInput", 0);

            
        }
        else
        {
            Vector3 movement = - playerPos + transform.position;
            animator.SetFloat("xInput", movement.x);
            animator.SetFloat("yInput", movement.z);
        }
    }
}
