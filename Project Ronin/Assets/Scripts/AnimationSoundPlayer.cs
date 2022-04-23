using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
    [SerializeField] CharacterAudioSet characterAudioSet;

    Dictionary<string, List<AudioSource>> animationEvent2audioClips = new Dictionary<string, List<AudioSource>>();

    void Awake()
    {
        GameObject sourcesParent = new GameObject("Character Audio Sources");
        sourcesParent.transform.parent = transform;

        // Convert list from character audio set to dictionary, which is more efficient for look up
        if(characterAudioSet != null)
        {
            foreach(CharacterAudioSet.EventAudioPair pair in characterAudioSet.EventToAudioMap)
            {
                if(!animationEvent2audioClips.ContainsKey(pair.eventName))
                    animationEvent2audioClips.Add(pair.eventName, new List<AudioSource>());

                GameObject newSourceObject = new GameObject(pair.eventName);
                newSourceObject.transform.parent = sourcesParent.transform;
                AudioSource newSource = newSourceObject.AddComponent<AudioSource>();
                newSource.clip = pair.audioClip;

                animationEvent2audioClips[pair.eventName].Add(newSource);
            }
        }
    }

    public void InvokeSoundEvent(string eventName)
    {
        if(animationEvent2audioClips.ContainsKey(eventName))
            animationEvent2audioClips[eventName][Random.Range(0, animationEvent2audioClips[eventName].Count)].Play();
    }
}
