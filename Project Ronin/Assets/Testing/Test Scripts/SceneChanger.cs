using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] float sceneSwitchTime = 5;
    float timePassed = 0;
    bool hasChangedScene = false;
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= sceneSwitchTime && !hasChangedScene)
        {
            hasChangedScene = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
