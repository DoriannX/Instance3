using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    protected float cooldown;
    protected int damage;
    private MeshFilter weaponMesh;    
    protected Action<int> onWeaponUsed;
    
    public WeaponData Data => weaponData;

    private void Awake()
    {
        SetupWeapon();
    }

    private void Start()
    {
        if (weaponData)
            LoadData(weaponData);
        else
            Debug.LogWarning("WeaponData not assigned in inspector");
    }

    protected virtual void SetupWeapon()
    {
        weaponMesh = GetComponent<MeshFilter>();
    }

    public abstract void Attack(Transform playerTransform);

    public virtual void LoadData(WeaponData data)
    {
        if (data == null)
            throw new System.ArgumentNullException(nameof(data));

        weaponData = data;

        cooldown = Mathf.Clamp(data.cooldown, 0.1f, 10f);
        damage = Mathf.Clamp(data.damage, 1, 100);

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
