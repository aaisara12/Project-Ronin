using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class is assigned with an object with an animator which can be used to enable/disable animations 
public class VFXPlayer : MonoBehaviour
{
    // Don't forget to assign in editor
   public List<GameObject> effects;
   public List<GameObject> transforms;


    // list of tuples containing 1) effect, 2) reference to a transform
   private List<(GameObject, GameObject)> tuple;

   private Dictionary<string, GameObject> names;
   private Animator animator;
   
   void Awake()
   {
       names = new Dictionary<string, GameObject>();
       tuple = new List<(GameObject, GameObject)>();
       animator = GetComponent<Animator>();
       for (int i = 0; i < effects.Count; i++)
       {
           names.Add(effects[i].name, effects[i]);
           Debug.Log(effects[i].name);
           tuple.Add((effects[i], transforms[i]));
       }
   }

   public void PlayVFX(string name)
   {
        if (names.ContainsKey(name))
        {
            for (int i = 0; i < effects.Count; i++)
            {
                // Get index in tuple list
                if (effects[i].name == name)
                {
                    Debug.Log("Instantiated effect!");
                    GameObject effect = Instantiate(tuple[i].Item1, tuple[i].Item2.transform.position, tuple[i].Item2.transform.rotation);
                }
            }
        }
   }
}
