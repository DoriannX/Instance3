using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon Stats")]
    [SerializeField] private LayerMask enemyLayer;
    private float attackRange;

    public Transform playerTransform; //For DrawGizmos
       
    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not MeleeWeaponData meleeWeaponData)
            throw new InvalidCastException("WeaponData is not a meleeWeaponData");

        attackRange = Mathf.Clamp(meleeWeaponData.attackRange, 0.1f, 100f);
    }

    public override void Attack(Transform playerTransform)
    {
        Collider[] hitColliders = Physics.OverlapBox(playerTransform.position + playerTransform.forward * attackRange, (Vector3.one * attackRange), playerTransform.rotation, enemyLayer);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Hit enemies");
        }
    }

    private void OnDrawGizmos()
    {
        // Couleur de la boîte
        Gizmos.color = Color.red;

        // Dessiner la boîte à la position finale
        Gizmos.matrix = Matrix4x4.TRS(playerTransform.position + playerTransform.forward * attackRange, playerTransform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, Vector3.one * attackRange * 2);
    }
}
