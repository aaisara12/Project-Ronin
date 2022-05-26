using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private Transform DmgPopup;

    private void Start(){
        // MainDamagePopup.Create(DmgPopup, Vector3.zero, 300);
    }
    private void Update(){
        // if (Input.GetMouseButtonDown(0)){
        //     MainDamagePopup.Create(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), 300);
        // }
    }
}
