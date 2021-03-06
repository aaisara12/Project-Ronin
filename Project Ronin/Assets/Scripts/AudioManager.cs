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

    Dictionary<string, List<AudioSource>> name2source = new Dictionary<string, List<AudioSource>>();


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

            if(!name2source.ContainsKey(settings.soundName))
                name2source[settings.soundName] = new List<AudioSource>();
                
            name2source[settings.soundName].Add(newSource);
        }

        
    }

    // Wrapper around dictionary access to get randomized sound if multiple
    AudioSource GetSoundSource(string soundName)
    {
        int randInd = Random.Range(0, name2source[soundName].Count);
        return name2source[soundName][randInd];
    }

    public void PlaySound(string soundName)
    {
        GetSoundSource(soundName).Play();
    }

    public void PlaySoundAtLocation(string soundName, Vector3 location)
    {
        // If we have time, we should make this into a pooling system so that we don't have to keep creating and
        // destroying audio source objects
        AudioSource.PlayClipAtPoint(GetSoundSource(soundName).clip, location);
    }

    
    [Header("Soundtrack")]
    [SerializeField] AudioSource primaryTrackSlot;
    [SerializeField] AudioSource secondaryTrackSlot;      // Swap in any sound or track to loop
    [SerializeField] float fadeTime = 0.75f;
    [SerializeField] float maxVolume = 0.2f;

    bool isPrimaryTrack = true;     // Keep track of which track is the one being heard
    public void SwapTrack(string soundName)
    {
        if(name2source.ContainsKey(soundName))
        {
            StopAllCoroutines();    // Prevent multiple fades occurring from player spam
            StartCoroutine(FadeTrack(name2source[soundName][0].clip));
            isPrimaryTrack = !isPrimaryTrack;
        }
        
    }

    IEnumerator FadeTrack(AudioClip clip)
    {
        float timePassed = 0;
        if(isPrimaryTrack)
        {
            secondaryTrackSlot.clip = clip;
            secondaryTrackSlot.Play();      // Restart the clip
            while(timePassed < fadeTime)
            {
                primaryTrackSlot.volume = Mathf.Lerp(maxVolume, 0, timePassed/fadeTime);
                secondaryTrackSlot.volume = Mathf.Lerp(0, maxVolume, timePassed/fadeTime);
                timePassed += Time.unscaledDeltaTime;       // In case we choose to freeze gameplay with Time.timeScale = 0 but still fade
                yield return null;
            }
        }
        else
        {
            primaryTrackSlot.clip = clip;   // Restart the clip
            primaryTrackSlot.Play();
            while(timePassed < fadeTime)
            {
                primaryTrackSlot.volume = Mathf.Lerp(0, maxVolume, timePassed/fadeTime);
                secondaryTrackSlot.volume = Mathf.Lerp(maxVolume, 0, timePassed/fadeTime);
                timePassed += Time.unscaledDeltaTime;       // In case we choose to freeze gameplay with Time.timeScale = 0 but still fade
                yield return null;
            }
        }
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
