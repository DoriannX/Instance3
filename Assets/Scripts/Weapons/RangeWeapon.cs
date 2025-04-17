using System;
using Pooling;
using UnityEngine;

public class RangeWeapon : Weapon
{
    protected BulletSpawner bulletSpawner;

    [Header("Range Weapon Stats")] [SerializeField]
    private LayerMask hitLayerMask;

    private int ammoConsume;
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

    private void DrawCapsuleCast(Vector3 origin, Vector3 direction, float radius, float distance, Color color,
        float duration = 0.2f)
    {
        // Draw the main ray
        Debug.DrawRay(origin, direction * distance, color, duration);

        // Draw the capsule body (using multiple circles along the ray)
        int segments = 10; // Number of segments for the capsule body
        float segmentLength = distance / segments;

        for (int i = 0; i <= segments; i++)
        {
            Vector3 circleCenter = origin + direction * (i * segmentLength);
            DrawCircle(circleCenter, radius, direction, color, duration);
        }

        // Connect the circles with lines to create capsule effect
        int circlePoints = 8;
        for (int i = 0; i < circlePoints; i++)
        {
            float angle = i * Mathf.PI * 2 / circlePoints;
            Vector3 localOffset = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            // Calculate world offset based on ray direction
            Vector3 worldOffset = Quaternion.LookRotation(direction) * localOffset;

            // Draw line along the capsule length
            Debug.DrawLine(
                origin + worldOffset,
                origin + direction * distance + worldOffset,
                color,
                duration);
        }
    }

    private void DrawCircle(Vector3 center, float radius, Vector3 forward, Color color, float duration)
    {
        // Create a rotation based on the forward direction
        Quaternion rotation = Quaternion.LookRotation(forward);

        // Draw circle with multiple segments
        int segments = 16;
        Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 newPoint = center + rotation * new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Debug.DrawLine(prevPoint, newPoint, color, duration);
            prevPoint = newPoint;
        }
    }

    public override void Attack(Transform playerTransform)
    {
        float sphereRadius = 0.5f;
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        float rayDistance = 100f;

        // Visualize potential collisions
        DrawCapsuleCast(ray.origin, ray.direction, sphereRadius, rayDistance, Color.yellow, 0.1f);

        RaycastHit[] hits = new RaycastHit[10];
        int hitCount = Physics.SphereCastNonAlloc(ray, sphereRadius, hits, rayDistance, hitLayerMask);
        RaycastHit? targetHit = null;

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.TryGetComponent(out EntityHealth _))
            {
                targetHit = hits[i];
                break;
            }
        }

        if (targetHit.HasValue)
        {
            Debug.Log($"Hit object: {targetHit.Value.collider.gameObject.name}");
            bulletSpawner.transform.rotation =
                Quaternion.LookRotation(targetHit.Value.collider.transform.position - bulletSpawner.transform.position);

            Debug.DrawLine(ray.origin, targetHit.Value.collider.transform.position, Color.green, 0.2f);
            DrawCircle(targetHit.Value.collider.transform.position, sphereRadius, targetHit.Value.normal, Color.red,
                0.2f);
        }
        else
        {
            bulletSpawner.transform.localRotation = Quaternion.identity;
        }

        for (int i = 0; i < ammoShoot; i++)
        {
            float spreadAngle = (ammoShoot > 1) ? (i - (ammoShoot - 1) / 2f) * bulletSpread : 0f;
            Quaternion bulletRotation = bulletSpawner.transform.rotation * Quaternion.Euler(0, spreadAngle, 0);
            Vector3 bulletSpawnPos = bulletSpawner.transform.position;

            Bullet bullet = bulletSpawner.SpawnBullet(damage, hitLayerMask, bulletSpawnPos, bulletRotation, transform);


            bullet.gameObject.SetActive(true);
        }
        onWeaponUsed?.Invoke(ammoConsume);

        // Play the weapon's attack SFX.
        PlayAttackSFX();
    }

    public int AmmoConsume => ammoConsume;
    public float BulletSpread => bulletSpread;
    public int AmmoShoot => ammoShoot;
}