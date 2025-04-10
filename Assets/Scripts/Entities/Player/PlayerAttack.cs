using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private Transform playerTransform;
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;

    [Header("Weapons Settings")]
    [SerializeField] private int ammoAmount;
    private float meleeCooldownTimer = 0f;
    private float rangeCooldownTimer = 0f;


    //private Weapon currentWeapon;

    private void Start()
    {
        meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon = GetComponentInChildren<RangeWeapon>(true);
        playerTransform = GetComponent<Transform>();
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
        if (meleeWeapon.gameObject.activeSelf && meleeCooldownTimer <= 0f)
        {
            meleeWeapon.MeleeAttack();
            meleeCooldownTimer = meleeWeapon.Cooldown;
        }

        if (rangeWeapon.gameObject.activeSelf && rangeCooldownTimer <= 0f)
        {            
            if (ammoAmount > 0)
            {
                rangeWeapon.RangeAttack();
                ammoAmount -= rangeWeapon.AmmoConsumme;
                rangeCooldownTimer = rangeWeapon.Cooldown;
            }
            else
            {
                Debug.Log("No ammo left");
                return;
            }
        }        
    }

    private void Cooldown()
    {
        if (meleeCooldownTimer > 0f)
        {
            meleeCooldownTimer -= Time.deltaTime;
        }

        if (rangeCooldownTimer > 0f)
        {
            rangeCooldownTimer -= Time.deltaTime;
        }
    }
}
