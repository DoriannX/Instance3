using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon Stats")]
    [SerializeField] private LayerMask enemyLayer;
    private float attackRange;

    private Transform selfTransform;
    
    protected override void Awake()
    {
        selfTransform = GetComponent<Transform>();
    }
       
    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not MeleeWeaponData meleeWeaponData)
            throw new InvalidCastException("WeaponData is not a meleeWeaponData");

        attackRange = Mathf.Clamp(meleeWeaponData.attackRange, 0.1f, 100f);
    }

    public override void Attack(Transform playerTransform)
    {
        Vector3 boxCenter = playerTransform.position + playerTransform.forward * attackRange;
        Vector3 boxSize = Vector3.one * attackRange;
        
        // Draw debug box visualization
        DrawDebugBox(boxCenter, boxSize, playerTransform.rotation, Color.red, 0.2f);
        
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize * 0.5f, playerTransform.rotation, enemyLayer);
    
        if (hitColliders.Length <= 0)
            return;
    
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out EntityHealth entityHealth))
            {
                entityHealth.TakeDamage(damage, transform);
                onWeaponUsed?.Invoke(0); 
                Debug.Log("attack ");
                
                // Highlight hit object
                if (hitCollider.TryGetComponent<Renderer>(out var renderer))
                {
                    DrawDebugBox(hitCollider.bounds.center, hitCollider.bounds.size, hitCollider.transform.rotation, Color.green, 0.2f);
                }
            }
        }
    }
    
    private void DrawDebugBox(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration = 0.2f)
    {
        // Calculate the 8 corners of the box
        Vector3 halfSize = size * 0.5f;
        Vector3[] corners = new Vector3[8];
        
        corners[0] = center + rotation * new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        corners[1] = center + rotation * new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        corners[2] = center + rotation * new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        corners[3] = center + rotation * new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        corners[4] = center + rotation * new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        corners[5] = center + rotation * new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        corners[6] = center + rotation * new Vector3(halfSize.x, halfSize.y, halfSize.z);
        corners[7] = center + rotation * new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        
        // Draw the 12 edges of the box
        // Bottom face
        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[0], color, duration);
        
        // Top face
        Debug.DrawLine(corners[4], corners[5], color, duration);
        Debug.DrawLine(corners[5], corners[6], color, duration);
        Debug.DrawLine(corners[6], corners[7], color, duration);
        Debug.DrawLine(corners[7], corners[4], color, duration);
        
        // Connecting edges
        Debug.DrawLine(corners[0], corners[4], color, duration);
        Debug.DrawLine(corners[1], corners[5], color, duration);
        Debug.DrawLine(corners[2], corners[6], color, duration);
        Debug.DrawLine(corners[3], corners[7], color, duration);
    }
}
