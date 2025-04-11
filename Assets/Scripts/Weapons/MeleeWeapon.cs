using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon Stats")]
    [SerializeField] private LayerMask enemyLayer;
    private float attackRange;    

    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not MeleeWeaponData meleeWeaponData)
            throw new InvalidCastException("WeaponData is not a meleeWeaponData");

        attackRange = meleeWeaponData.attackRange;
    }

    public override void Attack()
    {
        Collider[] hitColliders = Physics.OverlapBox(weaponTransform.position + weaponTransform.forward * attackRange, (Vector3.one * attackRange), weaponTransform.rotation, enemyLayer);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Hit enemies");
        }
    }
}
