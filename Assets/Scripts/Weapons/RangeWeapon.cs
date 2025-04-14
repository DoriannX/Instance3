using System;
using Pooling;
using UnityEngine;

public class RangeWeapon : Weapon
{
    protected BulletSpawner bulletSpawner;

    [Header("Range Weapon Stats")] private int ammoConsume;
    private float bulletSpread;
    private int ammoShoot;

    protected override void SetupWeapon()
    {
        base.SetupWeapon();
        bulletSpawner = GetComponentInChildren<BulletSpawner>(true);
    }

    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not RangeWeaponData rangeWeaponData)
            throw new InvalidCastException("WeaponData is not a rangeWeaponData");

        bulletSpread = Mathf.Clamp(rangeWeaponData.bulletSpread, 1, 360);
        ammoConsume = Mathf.Clamp(rangeWeaponData.ammoConsume, 1, 30);
        ammoShoot = Mathf.Clamp(rangeWeaponData.ammoShoot, 1, 10);
    }

    public override void Attack(Transform playerTransform)
    {
        Ray ray = new Ray(playerTransform.position - new Vector3(0, playerTransform.localScale.y * 0.5f, 0), playerTransform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            bulletSpawner.transform.LookAt(hit.collider.transform);
            Debug.DrawLine(ray.origin, hit.collider.transform.position, Color.green, 0.1f);
        }
        else
        {
            bulletSpawner.transform.rotation = playerTransform.rotation;
        }

        for (int i = 0; i < ammoShoot; i++)
        {
            Bullet bullet = bulletSpawner.SpawnBullet();
            bullet.transform.position = bulletSpawner.transform.position;

            float spreadAngle = (ammoShoot > 1) ? (i - (ammoShoot - 1) / 2f) * bulletSpread : 0f;

            bullet.transform.rotation = bulletSpawner.transform.rotation * Quaternion.Euler(0, spreadAngle, 0);
            bullet.gameObject.SetActive(true);
        }

        onWeaponUsed?.Invoke(ammoConsume);
    }

    public int AmmoConsume => ammoConsume;
    public float BulletSpread => bulletSpread;
    public int AmmoShoot => ammoShoot;
}