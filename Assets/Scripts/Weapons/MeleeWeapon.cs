using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon Stats")]
    [SerializeField] private LayerMask enemyLayer;
    private float attackRange;    

    public void MeleeAttack()
    {        
        Collider[] hitColliders = Physics.OverlapBox(weaponTransform.position + weaponTransform.forward * attackRange, (Vector3.one * attackRange), weaponTransform.rotation, enemyLayer);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Hit enemies");
        }
    }

    public override void LoadData(WeaponData data)
    {
        base.LoadData(data);

        if (data is not MeleeWeaponData meleeWeaponData)
            throw new InvalidCastException("WeaponData is not a meleeWeaponData");

        attackRange = meleeWeaponData.attackRange;
    }

    //private void OnDrawGizmos()
    //{
    //    // Dessiner la bo�te du BoxCast      
    //    Vector3 boxHalfExtents = Vector3.one * attackRange; // Taille de la bo�te (moiti� des dimensions)
    //    Quaternion orientation = playerTransform.rotation; // Orientation de la bo�te

    //    // Couleur de la bo�te
    //    Gizmos.color = Color.red;        

    //    // Dessiner la bo�te � la position finale
    //    Gizmos.matrix = Matrix4x4.TRS(playerTransform.position + playerTransform.forward * attackRange, orientation, Vector3.one);
    //    Gizmos.DrawCube(Vector3.zero, boxHalfExtents*2);
    //}
}
