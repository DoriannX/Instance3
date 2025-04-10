using Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Player
{
    [RequireComponent(typeof(SphereCollider), typeof(global::Player))] 

    public class PlayerPickupItem : MonoBehaviour
    {
        private SphereCollider itemPickUpCol;
        [FormerlySerializedAs("ItemPickUpRad")] [SerializeField] private float itemPickUpRad;
        private global::Player player;
        
        private void Awake()
        {
            player = GetComponent<global::Player>();
            if (!itemPickUpCol)
                itemPickUpCol = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            if (!itemPickUpCol)
            {
                Debug.LogError($"Missing SphereCollider on {gameObject.name}", this);
                enabled = false;
                return;
            }

            itemPickUpCol.radius = itemPickUpRad;
            itemPickUpCol.isTrigger = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other || !player) return;

            if (other.TryGetComponent<ItemDrop>(out var itemDrop) && !itemDrop.GotPickedUp)
            {
                Debug.Log($"{other.gameObject.name} entered pickup range", other.gameObject);
                itemDrop.OnPickUp(player);
            }
        }
    }
}
