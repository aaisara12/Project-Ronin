using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSceneLoader : MonoBehaviour
{
    public void ReturnToStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
    }
}
