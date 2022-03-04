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
    Animator animator = null;

    GameObject playerObj = null;

    void Start()
    {
        // grab the player
        playerObj = GameObject.FindWithTag("Player");
        if (!animator) animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 playerPos = playerObj.transform.position;

        float distance = (transform.position - playerPos).magnitude;
        
        if (distance < attackRange)
        {
            // TODO: use ability
            Debug.Log("enemy attack");
        }
        else
        {
            Vector3 movement = (playerPos - transform.position).normalized;
            animator.SetFloat("xInput", movement.x);
            animator.SetFloat("yInput", movement.y);
        }
    }
}