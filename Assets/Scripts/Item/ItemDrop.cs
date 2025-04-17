using System;
using System.Collections;
using UnityEngine;

namespace Item
{
    public abstract class ItemDrop : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] protected float travelTime = 1f;
        [field: SerializeField] public bool GotPickedUp { get; private set; }
        [field: SerializeField] public bool HasArrived { get; private set; }

        protected Player targetPlayer;
        protected bool isMovingToTarget = false;
        [SerializeField] protected float moveSpeed = 5f; // Force multiplier
        protected Rigidbody rb;
        
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
        }

        public void OnPickUp(Entity target)
        {
            if (target == null)
            {
                Debug.LogWarning("Pickup target is null");
                return;
            }

            if (target is not Player player)
            {
                return;
            }

            targetPlayer = player;
            
            if (!GotPickedUp)
            {
                isMovingToTarget = true;
            }

            GotPickedUp = true;
        }
        
        protected virtual void Update()
        {
            if (isMovingToTarget && targetPlayer != null)
            {
                MoveTowardsTarget();

                if (HasArrived)
                {
                    ApplyEffect();
                    Destroy(gameObject);
                }
            }
        }

        protected void MoveTowardsTarget()
        {
            if (targetPlayer == null) return;

            Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed, ForceMode.Acceleration);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other == null || targetPlayer == null)
            {
                return;
            }

            if (!other.GetComponent<Player>())
            {
                return;
            }

            if (!other.isTrigger)
            {
                HasArrived = true;
            }
        }

        public abstract void ApplyEffect();
    }
}