using System;
using Pooling;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject<Bullet>
{
    [SerializeField] private float _speed = 10f;
    private Action<Bullet> _releaseFunc;
    private Transform bulletTransform;

    private void Awake()
    {
        bulletTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        bulletTransform.position += bulletTransform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit bullet");
        _releaseFunc(this);
    }

    public void SetReleaseFunc(Action<Bullet> releaseFunc)
    {
        _releaseFunc = releaseFunc;
    }
}
