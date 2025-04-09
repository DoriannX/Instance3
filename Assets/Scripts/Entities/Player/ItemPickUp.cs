using UnityEngine;

[RequireComponent(typeof(PlayerScript))] //Name of the player script
[RequireComponent(typeof(SphereCollider))] 

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private SphereCollider ItemPickUpCol;
    [SerializeField] private float ItemPickUpRad;
    [SerializeField] private ItemDrop item;

    public void Start()
    {
        ItemPickUpCol.radius = ItemPickUpRad;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Drop" && collision.isTrigger)
        {
            if (collision.gameObject.TryGetComponent<ItemDrop>(out item) && !item.GetPicked())
            {
                Debug.Log($"{collision.gameObject.name} got in pick up range");
                collision.transform.GetComponent<ItemDrop>().OnPickUp(gameObject);
            }else
            {
                return;
            }
        }
    }
}
