using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cameraToLookAt;
    
    void Start()
    {
        cameraToLookAt = Camera.main.transform;
    }

    void Update(){
        if(cameraToLookAt == null){                                 //If the billboard doesn't have a camera assignment, then assign it to the client's camera
            cameraToLookAt = Camera.main.transform;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {

        transform.LookAt(transform.position + cameraToLookAt.forward);
    }
}