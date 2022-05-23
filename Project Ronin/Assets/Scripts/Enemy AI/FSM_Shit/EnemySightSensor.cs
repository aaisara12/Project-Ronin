using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Enemy
{
    public class EnemySightSensor : MonoBehaviour
    {
        public Transform Player { get; private set; }

        [SerializeField] private LayerMask _ignoreMask;

        private Ray _ray;

        private void Awake()
        {
            Player = GameObject.Find("Ronin").transform;
        }

        public bool Ping()
        {
            if (Player == null)
                return false;

            _ray = new Ray(this.transform.position + new Vector3(0,1,0), Player.position-this.transform.position);

            var dir = new Vector3(_ray.direction.x, 0, _ray.direction.z);

            var angle = Vector3.Angle(dir, this.transform.forward);

            if (angle>60)
                return false;

            if (!Physics.Raycast(_ray, out var hit, 25, ~_ignoreMask))
            {
                return false;
            }
            
            // Debug.Log("Hit: " + hit.collider);

            if(hit.collider.tag == "Player")
            {
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * 25);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 25);
        }
    }
}
