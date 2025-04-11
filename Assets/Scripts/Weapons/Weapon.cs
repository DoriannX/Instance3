using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    protected float cooldown;
    protected int damage;
    private MeshFilter weaponMesh;
    protected Transform weaponTransform;
    protected Action<int> onWeaponUsed;

    private void Start()
    {
        SetupWeapon();
        LoadData(weaponData);
    }

    protected virtual void SetupWeapon()
    {
        weaponMesh = GetComponent<MeshFilter>();
        weaponTransform = transform;
    }

    public abstract void Attack();

    public virtual void LoadData(WeaponData data)
    {
        if (data == null)
            throw new System.ArgumentNullException(nameof(data));


        weaponData = data;

        cooldown = data.cooldown;
        damage = data.damage;

        if (weaponMesh != null)
        {
            weaponMesh.mesh = data.mesh;
        }
    }

    public float Cooldown => cooldown;
    public int Damage => damage;

    public Action<int> OnWeaponUsed
    {
        get => onWeaponUsed;
        set => onWeaponUsed = value;
    }
}
