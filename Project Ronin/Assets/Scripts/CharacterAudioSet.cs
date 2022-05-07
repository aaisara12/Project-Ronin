using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Set", menuName = "ScriptableObjects/AudioSet")]
public class CharacterAudioSet : ScriptableObject
{
    public List<EventAudioPair> EventToAudioMap;

    [System.Serializable]
    public struct EventAudioPair
    {
        public string eventName;
        public AudioClip audioClip;
    }
}
