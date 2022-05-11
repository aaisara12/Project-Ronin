using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace DistantLands.Lumen.Demo
{
    public class LumenDemoManager : MonoBehaviour
    {

        public int nextScene;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyUp(KeyCode.Space))
                SceneManager.LoadScene(nextScene);

        }
    }
}