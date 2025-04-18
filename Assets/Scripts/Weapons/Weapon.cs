using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] private AudioClip attackSFX; // SFX assigned per weapon by designers.
    protected float cooldown;
    protected int damage;
    private MeshFilter weaponMesh;
    protected Action<int> onWeaponUsed;
    protected AudioSource audioSource;
    
    public WeaponData Data => weaponData;

    protected virtual void Awake()
    {
        // Ensure an AudioSource is available on this weapon.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        if (weaponMesh != null)
        {
            weaponMesh.mesh = data.mesh;
        }
    }

    /// <summary>
    /// Plays the assigned attack SFX.
    /// </summary>
    protected void PlayAttackSFX()
    {
        if (attackSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSFX);
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