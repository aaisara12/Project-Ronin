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
    AttributeSet self = null;
    [SerializeField]
    CharacterCaptureController controller = null;

    GameObject playerObj = null;

    void Start()
    {
        // grab the player
        playerObj = GameObject.FindWithTag("Player");
        if (!self) self = gameObject.GetComponent<AttributeSet>();
        if (!controller) controller = gameObject.GetComponent<CharacterCaptureController>();
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
            Vector3 movement = playerPos - transform.position;
            movement.y = 0;
            movement = movement.normalized * self.GetFloat("speed");
            controller.MoveInDirection(movement);
        }
    }
}
