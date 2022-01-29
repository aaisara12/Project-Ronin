using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    void OnEnter();
    void Update();
    void LateUpdate();
    void OnExit();

    void Reset();
}
