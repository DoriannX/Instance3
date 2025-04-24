using System;
using UnityEngine;
using Armory;    // if your Weapon types live in the Armory namespace
using Entities; // for Entity

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;
    private Transform playerTransform;

    [Header("Weapons Settings")]
    [SerializeField] private int ammoAmount;
    private float cooldownTimer = 0f;
    public Weapon currentWeapon { get; private set; }
    public event Action<int> onAmmoChanged;
    public event Action onAttackStarted;

    private Entity entity;    // to notify weapon‐switch

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        playerTransform = transform;
        meleeWeapon     = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon     = GetComponentInChildren<RangeWeapon>(true);
        onAmmoChanged?.Invoke(ammoAmount);

        if (rangeWeapon != null)
            rangeWeapon.OnWeaponUsed += ConsumeAmmo;

        // pick whichever weapon GameObject is active
        if (meleeWeapon != null && !rangeWeapon.gameObject.activeSelf)
            currentWeapon = meleeWeapon;
        else if (rangeWeapon != null && !meleeWeapon.gameObject.activeSelf)
            currentWeapon = rangeWeapon;

        // sync Entity’s currentWeapon once
        if (currentWeapon != null)
            entity.SetCurrentWeapon(currentWeapon);
    }

    private void OnDestroy()
    {
        if (rangeWeapon != null)
            rangeWeapon.OnWeaponUsed -= ConsumeAmmo;
    }

    public void GatherAmmo(int count)
    {
        ammoAmount += count;
        onAmmoChanged?.Invoke(ammoAmount);
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        if (cooldownTimer > 0 || currentWeapon == null)
            return;

        // fire animation trigger
        onAttackStarted?.Invoke();

        if (currentWeapon is RangeWeapon rw)
        {
            if (ammoAmount >= rw.AmmoConsume)
            {
                currentWeapon.Attack(playerTransform);
                ammoAmount -= rw.AmmoConsume;
                onAmmoChanged?.Invoke(ammoAmount);
            }
            else
                Debug.Log("Not enough ammo");
        }
        else
        {
            currentWeapon.Attack(playerTransform);
        }

        cooldownTimer = currentWeapon.Cooldown;
    }

    private void ConsumeAmmo(int ammo)
    {
        ammoAmount = Mathf.Max(ammoAmount - ammo, 0);
        onAmmoChanged?.Invoke(ammoAmount);
    }

    /// <summary>
    /// Toggle between melee & range and notify Entity.
    /// </summary>
    private void Cooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void TakeWeapon(Weapon takenWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
    
        if (takenWeapon is MeleeWeapon newMelee)
        {
            meleeWeapon = newMelee;
            currentWeapon = meleeWeapon;
        }
        else if (takenWeapon is RangeWeapon newRange)
        {
            rangeWeapon = newRange;
            currentWeapon = rangeWeapon;
        }
    
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true);
        }
    }

    public void SwitchWeapon()
    {
        if (meleeWeapon == null || rangeWeapon == null)
            return;

        bool wasRange = (currentWeapon == rangeWeapon);
        meleeWeapon.gameObject.SetActive(wasRange);
        rangeWeapon.gameObject.SetActive(!wasRange);

        currentWeapon = wasRange ? meleeWeapon : rangeWeapon;
        entity.SetCurrentWeapon(currentWeapon);
    }

    /// <summary>
    /// Called when picking up a new Weapon instance.
    ///// </summary>
    //public void TakeWeapon(Weapon pickedUp)
    //{
    //    if (pickedUp is MeleeWeapon m)
    //    {
    //        meleeWeapon   = m;
    //        currentWeapon = m;
    //    }
    //    else if (pickedUp is RangeWeapon r)
    //    {
    //        rangeWeapon   = r;
    //        currentWeapon = r;
    //    }

    //    entity.SetCurrentWeapon(currentWeapon);
    //}
}
