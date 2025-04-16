using System;
using Pooling;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject<Bullet>
{
    [SerializeField] private float speed = 10f;
    private Action<Bullet> releaseFunc;
    private Transform bulletTransform;
    private LayerMask hitLayerMask;
    
    private int damage;

    private void Awake()
    {
        bulletTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        bulletTransform.position += bulletTransform.forward * (speed * Time.deltaTime);
    }

    public void SetLayer(LayerMask layerMask)
    {
        hitLayerMask = layerMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        
        if (other.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.TakeDamage(damage, bulletTransform);
        }
        
        if (releaseFunc == null)
        {
            Debug.LogError($"[Bullet] Release function not set on bullet {gameObject.name}");
            return;
        }
        Debug.Log(other.name);
        Debug.Log("hit bullet");
        releaseFunc(this);
    }
    
    public void SetDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogError($"[Bullet] Damage must be greater than 0. Current damage: {damage}");
            return;
        }
        
        this.damage = damage;
    }

    public void SetReleaseFunc(Action<Bullet> currentReleaseFunc)
    {
        releaseFunc = currentReleaseFunc;
    }
}
