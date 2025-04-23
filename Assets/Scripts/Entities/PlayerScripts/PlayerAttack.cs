using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;
    private Transform playerTransform;

    [Header("Weapons Settings")]
    [SerializeField] private int ammoAmount;
    [SerializeField] private float damageMultiplier = 1.0f;
    [SerializeField] private float cooldownMultiplier = 1.0f;
    [SerializeField] private float ammoMultiplier = 1.0f;
    private float cooldownTimer = 0f;
    public Weapon currentWeapon { get; private set; }

    public event Action<int> onAmmoChanged;

    private void Start()
    {
        onAmmoChanged?.Invoke(ammoAmount);
        playerTransform = GetComponent<Transform>();
        meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon = GetComponentInChildren<RangeWeapon>(true);
        

        if (rangeWeapon != null)
        {
            rangeWeapon.OnWeaponUsed += ConsumeAmmo;
        }

        if (meleeWeapon != null && !rangeWeapon.gameObject.activeSelf)
        {
            currentWeapon = meleeWeapon;
        }
        else if (rangeWeapon != null && !meleeWeapon.gameObject.activeSelf)
        {
            currentWeapon = rangeWeapon;
        }

        ammoAmount = Mathf.RoundToInt(ammoAmount * ammoMultiplier);
    }

    private void OnDestroy()
    {
        if (rangeWeapon != null)
        {
            rangeWeapon.OnWeaponUsed -= ConsumeAmmo;
        }
    }

    public void GatherAmmo(int count)
    {
        ammoAmount += count;
        onAmmoChanged?.Invoke(ammoAmount);
    }

    private void Update()
    {
        //SwitchWeapon();

        Cooldown();      
    }

    public void Attack()
    {
        if (cooldownTimer > 0 || currentWeapon == null)
            return;

        if (currentWeapon is RangeWeapon)
        {
            if (ammoAmount >= rangeWeapon.AmmoConsume)
            {
                currentWeapon.Attack(playerTransform, damageMultiplier);
            }
            else
            {
                Debug.Log("Not enough ammo");
                return;
            }
        }
        else if (currentWeapon is MeleeWeapon)
        {
            currentWeapon.Attack(playerTransform, damageMultiplier);
        }

        cooldownTimer = currentWeapon.Cooldown * cooldownMultiplier;
    }

    private void ConsumeAmmo(int ammo)
    {
        ammoAmount = Mathf.Max(ammoAmount - ammo, 0);
        onAmmoChanged?.Invoke(ammoAmount);
    }

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
      {
          return;
      }
      
      GameObject objectToDisable = (currentWeapon == rangeWeapon) ? rangeWeapon.gameObject : meleeWeapon.gameObject;
      GameObject objectToEnable = (currentWeapon == meleeWeapon) ? rangeWeapon.gameObject : meleeWeapon.gameObject;
      objectToDisable.SetActive(false);
      objectToEnable.SetActive(true);
      currentWeapon = (currentWeapon == meleeWeapon) ? rangeWeapon : meleeWeapon;            
    }

    public float GetCooldownMultiplier()
    {
        return cooldownMultiplier;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public float GetAmmoMultiplier()
    {
        return ammoMultiplier;
    }

    public void SetCooldownMultiplier(float newMultiplier)
    {
        cooldownMultiplier = newMultiplier;
    }

    public void SetDamageMultiplier(float newMultiplier)
    {
        damageMultiplier = newMultiplier;
    }

    public void SetAmmoMultiplier(float newMultiplier)
    {
        ammoMultiplier = newMultiplier;
    }
}
