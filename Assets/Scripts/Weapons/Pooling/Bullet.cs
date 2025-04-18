using System;
using System.Collections;
using Pooling;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject<Bullet>
{
    [Header("References")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float parryWindow = 1f;
    private Transform bulletTransform;
    private Action<Bullet> releaseFunc;
    private Transform shooterTransform;
    private bool isParried;
    private TrailRenderer bulletTrail;

    [Header("Bullet Settings")]
    private LayerMask hitLayerMask;
    
    private int damage;
    private float lifeTime = 5f;
    [SerializeField] private float maxLifeTime = 5f;

    private void Awake()
    {
        bulletTransform = GetComponent<Transform>();
        bulletTrail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        lifeTime = maxLifeTime;
    }

    private void OnDisable()
    {
        bulletTrail.Clear();
    }

    private void Update()
    {
        bulletTransform.position += bulletTransform.forward * (speed * Time.deltaTime);
        LifeTime();
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
            StartCoroutine(WaitForParry(entityHealth));
            //entityHealth.TakeDamage(damage);
        }
        
        /*if (releaseFunc == null)
        {
            Debug.LogError($"[Bullet] Release function not set on bullet {gameObject.name}");
            return;
        }
        Debug.Log(other.name);
        Debug.Log("hit bullet");
        releaseFunc(this);*/
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

    private void LifeTime()
    {       
        if (lifeTime <= 0)
        {
            releaseFunc(this);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
    }


    public void SetShooterOrigin(Transform shooterOrigin)
    {
        shooterTransform = shooterOrigin;
    }

    public void ParryBullet()
    {
        if (shooterTransform.position == null)
        {
            transform.eulerAngles += 180f * Vector3.up;
            isParried = true;
            return;
        }
        transform.LookAt(shooterTransform.position);
        isParried = true;
    }

    public void SetIsParried(bool value)
    {
        isParried = value;
    }

    IEnumerator WaitForParry(EntityHealth entityHealth)
    {
        yield return new WaitForSeconds(parryWindow);
        if (isParried)
        {
            releaseFunc(this);
            yield break;
        }
        entityHealth.TakeDamage(damage);
        if (releaseFunc == null)
        {
            Debug.LogError($"[Bullet] Release function not set on bullet {gameObject.name}");
            yield break;
            //return;
        }
        //Debug.Log(other.name);
        Debug.Log("hit bullet");
        releaseFunc(this);
    }
}
