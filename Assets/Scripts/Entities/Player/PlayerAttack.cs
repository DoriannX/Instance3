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
    }

    private void OnDestroy()
    {
        if (rangeWeapon != null)
        {
            rangeWeapon.OnWeaponUsed -= ConsumeAmmo;
        }
    }

    private void Update()
    {
        SwitchWeapon();

        Cooldown();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }

    }

    private void Attack()
    {
        if (cooldownTimer > 0)
            return;

        if (currentWeapon is RangeWeapon)
        {
            if (ammoAmount > rangeWeapon.AmmoConsume)
            {
                currentWeapon.Attack();
            }
            else
            {
                Debug.Log("Not enough ammo");
                return;
            }
        }
        else if (currentWeapon is MeleeWeapon)
        {
            currentWeapon.Attack();
        }

        cooldownTimer = currentWeapon.Cooldown;
    }

    private void ConsumeAmmo(int ammo)
    {
        ammoAmount -= ammo;
    }

    private void Cooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Switching weapon");

            if (currentWeapon == meleeWeapon)
            {
                meleeWeapon.gameObject.SetActive(false);
                rangeWeapon.gameObject.SetActive(true);
                currentWeapon = rangeWeapon;
                Debug.Log("Switched to range weapon");
            }
            else if (currentWeapon == rangeWeapon)
            {
                rangeWeapon.gameObject.SetActive(false);
                meleeWeapon.gameObject.SetActive(true);
                currentWeapon = meleeWeapon;
                Debug.Log("Switched to melee weapon");
            }
        }        
    }
}
