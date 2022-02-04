using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausingMover : MonoBehaviour
{
    bool isDisabled = false;
    float timePos = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnNewPauseState += HandleNewPauseState;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDisabled)
        {
            timePos += Time.deltaTime;
            transform.position = 3 * new Vector3(Mathf.Sin(timePos), 0, Mathf.Cos(timePos));
        }
    }

    void HandleNewPauseState(bool newPauseState)
    {
        isDisabled = newPauseState;
        Debug.Log("GameObject has been " + (newPauseState? "paused" : "unpaused") + "!");
    }
}
