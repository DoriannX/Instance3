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

        // New fields for pickup feedback
        [SerializeField] protected AudioClip pickupSFX;
        [SerializeField] protected GameObject pickupVFXPrefab;

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

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

            if (target is not Player player)
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

        protected IEnumerator GoToEntity()
        {
            float elapsedTime = 0;
            while (elapsedTime < travelTime && targetPlayer != null)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / travelTime;
                transform.position = Vector3.Lerp(startPos, targetPlayer.transform.position, t);

                if (HasArrived || targetPlayer == null)
                    break;

                yield return null;
            }

            if (targetPlayer != null && HasArrived)
            {
                TriggerPickupFeedback();
                ApplyEffect();
                Destroy(gameObject);
            }
        }

        // Triggers visual and audio feedback on pickup.
        protected virtual void TriggerPickupFeedback()
        {
            // Play the pickup sound at the item's position.
            if (pickupSFX != null)
            {
                AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
            }
            
            // Instantiate the VFX prefab.
            if (pickupVFXPrefab != null)
            {
                GameObject vfx = Instantiate(pickupVFXPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
            }
        }

        public abstract void ApplyEffect();
    }
}
