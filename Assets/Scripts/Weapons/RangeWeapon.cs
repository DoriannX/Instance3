using System;
using Pooling;
using UnityEngine;

public class RangeWeapon : Weapon
{
    protected BulletSpawner bulletSpawner;

    [Header("Range Weapon Stats")]
    private int ammoConsumme;
    private float bulletSpread;
    private int ammoShoot;

    protected override void SetupWeapon()
    {
        base.SetupWeapon();    
        bulletSpawner = GetComponentInChildren<BulletSpawner>(true);
    }

    public void RangeAttack()
    {
        for (int i = 0; i < ammoShoot; i++)
        {
            Bullet bullet = bulletSpawner.SpawnBullet();
            bullet.transform.position = bulletSpawner.transform.position;

            float spreadAngle = (ammoShoot > 1) ? (i - (ammoShoot - 1) / 2f) * bulletSpread : 0f;

            bullet.transform.rotation = weaponTransform.rotation * Quaternion.Euler(0, spreadAngle, 0);
            bullet.gameObject.SetActive(true);
        }
    }

    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not RangeWeaponData rangeWeaponData)
            throw new InvalidCastException("WeaponData is not a rangeWeaponData");

        bulletSpread = rangeWeaponData.bulletSpread;
        ammoConsumme = rangeWeaponData.ammoConsumme;
        ammoShoot = rangeWeaponData.ammoShoot;
    }

    public int AmmoConsumme => ammoConsumme;
    public float BulletSpread => bulletSpread;
    public int AmmoShoot => ammoShoot;
}
