using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Item
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SphereCollider))]
    public abstract class ItemDrop : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] protected float travelTime = 1f;
        [field: SerializeField] public bool GotPickedUp { get; private set; }
        [field: SerializeField] public bool HasArrived { get; private set; }

        // Pickup settings provided by designers.
        [SerializeField] protected string pickupSfxName = "Pickup"; // Use camelCase for variables.
        [SerializeField] protected GameObject pickupVfxPrefab;

        // Public properties for feedback data using PascalCase.
        public string PickupSfxName => pickupSfxName;
        public GameObject PickupVfxPrefab => pickupVfxPrefab;

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

        // Static event (global to all ItemDrop instances) for item pickup feedback.
        // Note: per lead dev preference, events are named in camelCase.
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

        protected IEnumerator GoToEntity()
        {
            float elapsedTime = 0f;
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
                // Instead of directly handling feedback here, raise a global event.
                onItemPickedUp?.Invoke(this);
                ApplyEffect();
                Destroy(gameObject);
            }
        }

        public abstract void ApplyEffect();
    }
}
