using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    protected float cooldown;
    protected int damage;
    protected string attackSFX;
    protected float knockbackForce;
    private MeshFilter weaponMesh;
    protected Action<int> onWeaponUsed;
    
    public WeaponData Data => weaponData;

    protected virtual void Awake()
    {
        SetupWeapon();
    }

    private void Start()
    {
        if (weaponData)
            LoadData(weaponData);
        else
            Debug.LogWarning($"WeaponData not assigned in inspector for {gameObject.name}");
    }

    protected virtual void SetupWeapon()
    {
        weaponMesh = GetComponent<MeshFilter>();
    }

    public abstract void Attack(Transform playerTransform, float damageMultiplier = 1.0f);

    public virtual void LoadData(WeaponData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        weaponData = data;
        cooldown = Mathf.Clamp(data.cooldown, 0.1f, 10f);
        damage = Mathf.Clamp(data.damage, 1, 100);
        knockbackForce = Mathf.Clamp(data.knockbackForce, 0.1f, 100f);
        attackSFX = data.attackSFX;
        if (weaponMesh != null)
        {
            weaponMesh.mesh = data.mesh;
        }
    }

    /// <summary>
    /// Plays the assigned attack SFX.
    /// </summary>   

    public float Cooldown => cooldown;
    public int Damage => damage;

    public float KnockbackForce => knockbackForce;

    public Action<int> OnWeaponUsed
    {
        get => onWeaponUsed;
        set => onWeaponUsed = value;
    }
}