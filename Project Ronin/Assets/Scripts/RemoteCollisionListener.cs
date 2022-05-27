using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RemoteCollisionListener : MonoBehaviour
{
    // GameObject whose collisions we are listening in on
    [SerializeField] GameObject remoteCollisionObject;

    CollisionRelay relay;

    void Awake()
    {
        if(remoteCollisionObject == null)
            throw new MissingComponentException("Collision Relay: No collision object specified!");

        relay = remoteCollisionObject.AddComponent<CollisionRelay>();
        relay.OnMyTriggerEnter += OnTriggerEnterRemote;
        relay.OnMyCollisionEnter += OnCollisionEnterRemote;
    }

    // Use OnDestroy since animations will enable/disable components frequently
    void OnDestroy()
    {
        relay.OnMyTriggerEnter -= OnTriggerEnterRemote;
        relay.OnMyCollisionEnter -= OnCollisionEnterRemote;
    }
    
    protected virtual void OnCollisionEnterRemote(Collision other){}
    protected virtual void OnTriggerEnterRemote(Collider other){}

    public class CollisionRelay : MonoBehaviour
    {
        public event System.Action<Collision> OnMyCollisionEnter;
        public event System.Action<Collider> OnMyTriggerEnter;

        void OnTriggerEnter(Collider other)
        {
            OnMyTriggerEnter?.Invoke(other);
        }

        void OnCollisionEnter(Collision collision)
        {
            OnMyCollisionEnter?.Invoke(collision);
        }
    }
}
