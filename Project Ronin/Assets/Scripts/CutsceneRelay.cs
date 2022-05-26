using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneRelay : MonoBehaviour
{
    public event System.Action OnCutsceneStart;
    public event System.Action OnCutsceneEnded;

    public void BroadcastCutsceneStart()
    {
        OnCutsceneStart?.Invoke();
    }

    // Cut scene animator can tell listeners when it's done
    public void BroadcastCutsceneEnd()
    {
        OnCutsceneEnded?.Invoke();
    }
}
