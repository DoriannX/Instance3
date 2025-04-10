using System;
using Pooling;
using UnityEngine;

public class RangeWeapon : Weapon
{
    protected BulletSpawner bulletSpawner;

    [Header("Range Weapon Stats")]
    private int ammoConsumme;
    private float bulletSpread;
    protected override void SetupWeapon()
    {
        base.SetupWeapon();    
        bulletSpawner = GetComponentInChildren<BulletSpawner>(true);
    }

    public void RangeAttack()
    {
        for (int i = 0; i < ammoConsumme; i++)
        {
            Bullet bullet = bulletSpawner.SpawnBullet();
            bullet.transform.position = bulletSpawner.transform.position;

            float spreadAngle = (ammoConsumme > 1) ? (i - (ammoConsumme - 1) / 2f) * bulletSpread : 0f;

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
    }

    public int AmmoConsumme => ammoConsumme;
    public float BulletSpread => bulletSpread;
}
