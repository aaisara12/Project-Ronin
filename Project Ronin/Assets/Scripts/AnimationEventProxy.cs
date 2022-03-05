using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows for animation events to call functions 
public class AnimationEventProxy : MonoBehaviour
{   
    // Probably need to generalize this in the future (tech debt +1)
    // If we can avoid the need for a proxy for all animation events that would be great!
    public void PlaySound(string name)
    {
        AudioManager.instance?.PlaySound(name);
    }
}
