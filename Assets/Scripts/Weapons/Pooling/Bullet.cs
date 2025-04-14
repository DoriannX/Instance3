using System;
using Pooling;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour, IPooledObject<Bullet>
{
    [SerializeField] private float speed = 10f;
    private Action<Bullet> releaseFunc;
    private Transform bulletTransform;

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
        if (releaseFunc == null)
        {
            Debug.LogError($"[Bullet] Release function not set on bullet {gameObject.name}");
            return;
        }
        Debug.Log("hit bullet");
        releaseFunc(this);
    }

    public void SetReleaseFunc(Action<Bullet> currentReleaseFunc)
    {
        releaseFunc = currentReleaseFunc;
    }
}
