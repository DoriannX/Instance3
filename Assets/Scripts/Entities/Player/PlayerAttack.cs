using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;

    [Header("Weapons Settings")]
    [SerializeField] private int ammoAmount;
    private float cooldownTimer = 0f;
    private Weapon currentWeapon;

    private void Start()
    {
        meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon = GetComponentInChildren<RangeWeapon>(true);

        rangeWeapon.OnWeaponUsed += ConsummeAmmo;

        if (meleeWeapon != null && !rangeWeapon.gameObject.activeSelf)
        {
            currentWeapon = meleeWeapon;
        }
        else if (rangeWeapon != null && !meleeWeapon.gameObject.activeSelf)
        {
            currentWeapon = rangeWeapon;
        }
    }

    private void OnDestroy()
    {
        if (rangeWeapon != null)
        {
            rangeWeapon.OnWeaponUsed -= ConsummeAmmo;
        }
    }

    private void Update()
    {
        Cooldown();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if(cooldownTimer > 0) 
            return;

        if (currentWeapon is RangeWeapon)
        {
            if(ammoAmount > rangeWeapon.AmmoConsumme)
            {
                currentWeapon.Attack();
            }
            else
            {              
                return;
            }
        }
        else if (currentWeapon is MeleeWeapon)
        {
            currentWeapon.Attack();
        }

        cooldownTimer = currentWeapon.Cooldown;      
    }

    private void ConsummeAmmo(int ammo)
    {
        ammoAmount -= ammo;
    }

    private void Cooldown()
    {
        if(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
