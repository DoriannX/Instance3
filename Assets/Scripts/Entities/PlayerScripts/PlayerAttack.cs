using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;
    private Transform playerTransform;

    [Header("Weapons Settings")]
    [SerializeField] private int ammoAmount;
    private float cooldownTimer = 0f;
    private Weapon currentWeapon;

    private void Start()
    {
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
        //SwitchWeapon();

        Cooldown();      
    }

    public void Attack()
    {
        if (cooldownTimer > 0)
            return;

        if (currentWeapon is RangeWeapon)
        {
            if (ammoAmount >= rangeWeapon.AmmoConsume)
            {
                currentWeapon.Attack(playerTransform);
            }
            else
            {
                Debug.Log("Not enough ammo");
                return;
            }
        }
        else if (currentWeapon is MeleeWeapon)
        {
            currentWeapon.Attack(playerTransform);
        }

        cooldownTimer = currentWeapon.Cooldown;
    }

    private void ConsumeAmmo(int ammo)
    {    
        ammoAmount =Mathf.Max(ammoAmount - ammo, 0);
    }

    private void Cooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void SwitchWeapon()
    {        
        GameObject objectToDisable = (currentWeapon == rangeWeapon) ? rangeWeapon.gameObject : meleeWeapon.gameObject;
        GameObject objectToEnable = (currentWeapon == meleeWeapon) ? rangeWeapon.gameObject : meleeWeapon.gameObject;
        objectToDisable.SetActive(false);
        objectToEnable.SetActive(true);
        currentWeapon = (currentWeapon == meleeWeapon) ? rangeWeapon : meleeWeapon;               
    }
}
