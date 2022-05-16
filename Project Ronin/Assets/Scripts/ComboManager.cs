using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float comboWindow = 1f;    // How many seconds until the combo is dropped
    [SerializeField] int maxCombo = 3;

    float comboDropTime = 0;

    public void AddCombo()
    {
        int curCombo = animator.GetInteger("combo");
        animator.SetInteger("combo", (curCombo < maxCombo)? curCombo + 1 : 1);

        comboDropTime = Time.time + comboWindow;
    }

    // Update is called once per frame
    void Update()
    {
        if(comboDropTime < Time.time)
            animator.SetInteger("combo", 1);
    }
}
