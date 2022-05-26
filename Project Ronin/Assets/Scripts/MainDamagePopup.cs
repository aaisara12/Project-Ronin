using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainDamagePopup : MonoBehaviour
{
    public static MainDamagePopup Create(Vector3 position, int damageAmount){
        Transform damagePopupTransform = Instantiate(DamagePopupInstance.i.DmgPopup, position, Quaternion.LookRotation(new Vector3(1,-1,1)));
        MainDamagePopup damagePopup = damagePopupTransform.GetComponent<MainDamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }
    private const float DISAPPEAR_TIMER_MAX = .3f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    float increaseScaleAmount = .5f;
    float decreaseScaleAmount = .1f;
    private void Awake(){
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int damageAmount){
        textMesh.text = damageAmount.ToString();
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    private void Update(){
        float moveYSpeed = 5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f){
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else{
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0){
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0){
                Destroy(gameObject);
            }
        }
    }
}
