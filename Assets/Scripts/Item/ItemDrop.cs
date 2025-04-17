using System;
using System.Collections;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ItemDrop : MonoBehaviour
    {
        public bool GotPickedUp { get; private set; }
        public bool HasArrived { get; private set; }

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

        // Static event (global to all ItemDrop instances) for item pickup feedback.
        public static event System.Action<ItemDrop> onItemPickedUp;
        protected bool isMovingToTarget = false;
        [SerializeField] protected float moveSpeed = 5f; // Force multiplier
        protected Rigidbody rb;
        [SerializeField] protected float maxPickupTime = 5f; // Maximum seconds before forced pickup
        protected float pickupStartTime;
        
        protected virtual void Awake()  
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

            if (!(target is Player player))
            {
                return;
            }
            targetPlayer = player;
            startPos = transform.position;
            if (!GotPickedUp)
            {
                isMovingToTarget = true;
            }

            GotPickedUp = true;
            pickupStartTime = Time.time;
        }
        
        protected virtual void Update()
        {
            if (isMovingToTarget && targetPlayer != null)
            {
                MoveTowardsTarget();

                if (HasArrived)
                {
                    onItemPickedUp?.Invoke(this);
                    ApplyEffect();
                    Destroy(gameObject);
                }
            }
        }

        protected void MoveTowardsTarget()
        {
            if (targetPlayer == null) return;
        
            Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
            
            // Apply force for natural movement
            rb.AddForce(direction * moveSpeed, ForceMode.Acceleration);
            
            // If very close or orbiting (detected by distance not decreasing over time)
            if (distanceToPlayer < 0.5f)
            {
                // If close enough, snap to player
                transform.position = targetPlayer.transform.position;
                HasArrived = true;
            }
            else if (isMovingToTarget && Time.time - pickupStartTime > maxPickupTime)
            {
                // Force completion after timeout
                transform.position = targetPlayer.transform.position;
                HasArrived = true;
            }
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
