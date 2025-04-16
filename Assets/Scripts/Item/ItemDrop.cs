using System;
using System.Collections;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SphereCollider))]
    public abstract class ItemDrop : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] protected float travelTime = 1f;
        [field: SerializeField] public bool GotPickedUp { get; private set; }
        [field: SerializeField] public bool HasArrived { get; private set; }

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

        // Static event (global to all ItemDrop instances) for item pickup feedback.
        public static event System.Action<ItemDrop> onItemPickedUp;

        protected virtual void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError($"Missing MeshRenderer on {gameObject.name}");
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
                StartCoroutine(GoToEntity());
            }
            GotPickedUp = true;
        }
        
        // Use collision detection to determine arrival.
        public void OnTriggerEnter(Collider other)
        {
            if (other == null || targetPlayer == null)
            {
                return;
            }

            // Ensure the collider belongs to a Player.
            if (other.GetComponent<Player>() == null)
            {
                throw new InvalidCastException("Collider does not belong to a Player.");
            }

            // When a non-trigger collider collides, mark the item as arrived.
            if (!other.isTrigger)
            {
                HasArrived = true;
            }
        }

        protected IEnumerator GoToEntity()
        {
            float elapsedTime = 0f;
            while (elapsedTime < travelTime && targetPlayer != null)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / travelTime;
                transform.position = Vector3.Lerp(startPos, targetPlayer.transform.position, t);

                if (HasArrived || targetPlayer == null)
                {
                    break;
                }
                yield return null;
            }

            if (targetPlayer != null && HasArrived)
            {
                onItemPickedUp?.Invoke(this);
                ApplyEffect();
                Destroy(gameObject);
            }
        }

        public abstract void ApplyEffect();
    }
}
