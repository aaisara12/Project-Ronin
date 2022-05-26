using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCinematicController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioManager.instance.SwapTrack("soundtrack-boss");
        }
    }
}
