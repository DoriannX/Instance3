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
        [field: SerializeField] public string uniqueItemId { get; private set; }

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

        // Static event (global to all ItemDrop instances) for item pickup feedback.
        public static event Action<ItemDrop> onItemPickedUp;
        public event Action onItemStartPickup;
        protected bool isMovingToTarget = false;
        [SerializeField] protected float moveSpeed = 5f; // Force multiplier
        protected Rigidbody rb;
        [SerializeField] protected float maxPickupTime = 5f; // Maximum seconds before forced pickup
        [SerializeField] protected float timeToWaitBeforeMovingToTarget;
        private bool canMoveToTarget;
        protected float pickupStartTime;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            
            if (string.IsNullOrEmpty(uniqueItemId))
            {
                uniqueItemId = name;
            }
        }

        protected virtual void Start()
        {
            StartCoroutine(WaitBeforeMovingToTarget());
        }

        public bool OnPickUp(Entity target)
        {
            if (target == null)
            {
                Debug.LogWarning("Pickup target is null");
                return false;
            }

            if (!(target is Player player))
            {
                return false;
            }

            targetPlayer = player;
            startPos = transform.position;
            if (!GotPickedUp)
            {
                if (canMoveToTarget)
                {
                    isMovingToTarget = true;
                }
                else
                {
                    StartCoroutine(RetryToPickup(target));
                    return false;
                }
            }

            onItemStartPickup?.Invoke();

            GotPickedUp = true;
            pickupStartTime = Time.time;
            return true;
        }

        private IEnumerator RetryToPickup(Entity target)
        {
            yield return new WaitForEndOfFrame();
            OnPickUp(target);
        }

        private IEnumerator WaitBeforeMovingToTarget()
        {
            yield return new WaitForSecondsRealtime(timeToWaitBeforeMovingToTarget);
            canMoveToTarget = true;
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

            // Apply direct force toward player
            rb.AddForce(direction * (moveSpeed * 3f), ForceMode.Acceleration);

            // If very close, snap to player
            if (distanceToPlayer < 0.5f)
            {
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