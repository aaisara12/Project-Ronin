using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHitIndicator : MonoBehaviour
{
    [SerializeField]
    Material hitMat;
    [SerializeField]
    float blinkDuration = 0.1f;
    Material normalMat;
    AttributeSet attributes;
    MeshRenderer myMesh;

    float oldHP = -1;

    private void Start()
    {
        attributes = GetComponent<AttributeSet>();
        oldHP = attributes.GetFloat("hp");
        myMesh = GetComponent<MeshRenderer>();
        normalMat = myMesh.material;
    }

    public void OnAttributeChange()
    {
        float newHp = attributes.GetFloat("hp");
        if (oldHP != attributes.GetFloat("hp"))
        {
            oldHP = newHp;
            StartCoroutine(ResetTimer());
        }
    }

    private IEnumerator ResetTimer()
    {
        myMesh.material = hitMat;

        float time = 0;
        while (time < blinkDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        myMesh.material = normalMat;
    }
}
