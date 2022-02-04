using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSoftFollow : MonoBehaviour
{
    GameObject player;
    Vector3 initialDiff = Vector3.zero;

    Vector3 velocity;

    [SerializeField]
    float smoothTime;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        initialDiff = transform.position - player.transform.position;
        initialDiff.y = 0;
    }

    void Update()
    {
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = transform.position.y;
        targetPosition += initialDiff;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
