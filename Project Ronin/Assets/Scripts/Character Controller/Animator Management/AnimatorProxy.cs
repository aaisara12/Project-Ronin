using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorProxy : MonoBehaviour
{
    [SerializeField]
    protected Animator animator = null;

    protected virtual void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public virtual void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        OnParameterChange();
    }

    public virtual void SetBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
        OnParameterChange();
    }

    public virtual void SetFloat(string floatName, float value)
    {
        animator.SetFloat(floatName, value);
        OnParameterChange();
    }

    protected virtual void OnParameterChange() {}
}
