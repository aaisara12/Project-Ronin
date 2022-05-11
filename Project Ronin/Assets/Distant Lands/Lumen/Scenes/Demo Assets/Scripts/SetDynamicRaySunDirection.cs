using DistantLands.Lumen.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands.Lumen.Demo
{
    [ExecuteAlways]
    public class SetDynamicRaySunDirection : MonoBehaviour
    {

        public enum SetType { pointTo, pullDirection }
        public SetType setType;
        public Transform targetObject;
        public DynamicRayData rayData;

        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

            if (setType == SetType.pointTo)
                rayData.sunDirection = (transform.position - targetObject.position).normalized;
            else
                rayData.sunDirection = targetObject.forward;

            rayData.needsToBeUpdated = true;


        }
    }
}