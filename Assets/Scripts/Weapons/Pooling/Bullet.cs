using System;
using Pooling;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject<Bullet>
{
    [SerializeField] private float speed = 10f;
    private Action<Bullet> releaseFunc;
    private Transform bulletTransform;
    
    private int damage;

    private void Awake()
    {
        bulletTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        bulletTransform.position += bulletTransform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.TakeDamage(damage);
        }
        
        if (releaseFunc == null)
        {
            Debug.LogError($"[Bullet] Release function not set on bullet {gameObject.name}");
            return;
        }
        releaseFunc(this);
    }
    
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetReleaseFunc(Action<Bullet> currentReleaseFunc)
    {
        releaseFunc = currentReleaseFunc;
    }
}
