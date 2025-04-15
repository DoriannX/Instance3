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
        protected Vector3 startPos;

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
                ApplyEffect();
                Destroy(gameObject);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other == null || targetPlayer == null)
            {
                return;
            }
            if (!other.GetComponent<Player>())
            {
                throw new InvalidCastException();
            }
            if (!other.isTrigger)
            {
                HasArrived = true;
            }
        }

        public abstract void ApplyEffect();
    }
}