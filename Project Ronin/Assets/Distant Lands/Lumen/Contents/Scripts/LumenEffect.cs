using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands.Lumen
{
    [ExecuteAlways]
    public class LumenEffect : MonoBehaviour
    {
        public enum UpdateFrequency
        {
            always,
            onChanges,
            viaScripting
        }
        public UpdateFrequency updateFrequency = UpdateFrequency.onChanges;

        [ColorUsage(true, true)]
        public Color localColor = Color.white;


        // Start is called before the first frame update
        void Awake()
        {

            GlobalLumenFunctions.DoubleCheckShapeLists();
            RedoEffect();

        }



        // Update is called once per frame
        void LateUpdate()
        {

            if (updateFrequency == UpdateFrequency.always)
                RedoEffect();

        }

        public virtual void RedoEffect()
        {

            

        }

        public void RedoEffectFromDelegate()
        {

            if (updateFrequency != UpdateFrequency.onChanges)
                return;


            RedoEffect();

        }
    }
}