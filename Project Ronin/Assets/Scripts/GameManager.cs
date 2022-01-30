using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public event System.Action OnPauseGame;

    public static GameManager Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> Freezes all game objects in scene </summary>
    public void PauseGame()
    {
        OnPauseGame?.Invoke();

        // TODO
    }
}
