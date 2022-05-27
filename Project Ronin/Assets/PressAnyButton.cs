using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyButton : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip startMenu;

    void Start()
    {
        audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            audioSource.PlayOneShot(startMenu, 1f);
            SceneLoader.instance.LoadScene();
        }
    }
}
