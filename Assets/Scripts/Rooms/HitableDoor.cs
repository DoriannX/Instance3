using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class HitableDoor : DoorSystem
{
    private EntityHealth doorHealth;
    protected override void Awake()
    {
        base.Awake();
        doorHealth = GetComponent<EntityHealth>();
    }
    
    private void Start()
    {
        doorHealth.onDeath += OpenDoor;
    }
}