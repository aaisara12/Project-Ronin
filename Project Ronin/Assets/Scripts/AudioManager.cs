using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance
    {
        get {return _instance;}
        private set
        {
            // Destroy any old AudioManager
            if(_instance != null)
                Destroy(_instance.gameObject);
            
            _instance = value;
        }
    }
    static AudioManager _instance;



    [SerializeField] List<SoundSettings> sounds;

    Dictionary<string, AudioSource> name2source = new Dictionary<string, AudioSource>();


    void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);

        // Convert editor values to real audio sources
        foreach(SoundSettings settings in sounds)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = settings.clip;
            newSource.volume = settings.volume;
            newSource.pitch = settings.pitch;
            newSource.loop = false;

            name2source[settings.soundName] = newSource;
        }

        
    }

    public void PlaySound(string soundName)
    {
        name2source[soundName].Play();
    }

    public void PlaySoundAtLocation(string soundName, Vector3 location)
    {
        // If we have time, we should make this into a pooling system so that we don't have to keep creating and
        // destroying audio source objects
        AudioSource.PlayClipAtPoint(name2source[soundName].clip, location);
    }

    // Play a sound and have it loop
    public void LoopSound(string soundName)
    {
        name2source[soundName].loop = true;
        PlaySound(soundName);
    }

    public void StopSound(string soundName)
    {
        name2source[soundName].Stop();
    }

}

[System.Serializable]
struct SoundSettings
{
    public string soundName;
    [Range(0, 1)]
    public float volume;

    [Range(0.1f, 2)]
    public float pitch;
    public AudioClip clip;
}
