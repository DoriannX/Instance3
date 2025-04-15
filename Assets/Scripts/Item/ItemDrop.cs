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

        // Replaced direct audio clip with a pickup SFX name that SFXManager uses.
        [SerializeField] protected string pickupSFXName = "Pickup"; // Designers assign this name in the Inspector.
        [SerializeField] protected GameObject pickupVFXPrefab;

        protected Player targetPlayer;
        protected Vector3 startPos;
        protected MeshRenderer meshRenderer;

        // (Optionally, you can add an event here if you want other systems to subscribe)
        // public UnityEvent OnItemPickedUp;

        protected virtual void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError($"Missing MeshRenderer on {gameObject.name}");
            }
            // Optionally initialize events:
            // if (OnItemPickedUp == null) OnItemPickedUp = new UnityEvent();
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

        // Decoupled feedback using SFXManager for sound and direct instantiation for VFX.
        protected virtual void TriggerPickupFeedback()
        {
            // Instead of using AudioSource.PlayClipAtPoint, use SFXManager.
            if (!string.IsNullOrEmpty(pickupSFXName) && SFXManager.instance != null)
            {
                SFXManager.instance.PlaySFX(pickupSFXName);
            }
            
            // Instantiate the VFX prefab.
            if (pickupVFXPrefab != null)
            {
                GameObject vfx = Instantiate(pickupVFXPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
            }
            
            // Optionally, broadcast the event so other systems can react:
            // OnItemPickedUp?.Invoke();
        }

        public abstract void ApplyEffect();
    }
}
