using UnityEngine;
using Armory;   // if your Weapon types live here
using Entities; // for Entity & EntityHealth

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator & Parameters")]
    [SerializeField] private Animator animator;
    private static readonly int SpeedHash      = Animator.StringToHash("Speed");
    private static readonly int AttackHash     = Animator.StringToHash("Attack");
    private static readonly int IsDeadHash     = Animator.StringToHash("IsDead");
    private static readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");

    private Entity       entity;
    private EntityHealth health;
    private PlayerAttack attack;
    private Rigidbody    rb;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
        health = GetComponent<EntityHealth>();
        attack = GetComponent<PlayerAttack>();
        rb     = GetComponent<Rigidbody>();

        // set initial weapon‑type
        animator.SetInteger(WeaponTypeHash, WeaponToIndex(entity.CurrentWeapon));
    }

    private void OnEnable()
    {
        entity.OnWeaponChanged   += OnWeaponChanged;
        attack.onAttackStarted   += OnAttackTriggered;
        health.onDeath           += OnDeath;
    }

    private void OnDisable()
    {
        entity.OnWeaponChanged   -= OnWeaponChanged;
        attack.onAttackStarted   -= OnAttackTriggered;
        health.onDeath           -= OnDeath;
    }

    private void Update()
    {
        if (rb != null)
        {
            // normalized horizontal speed
            Vector3 horiz = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float norm = horiz.magnitude / Mathf.Max(1f, entity.Speed);
            animator.SetFloat(SpeedHash, norm);
        }
    }

    private void OnWeaponChanged(Weapon newWeapon)
    {
        animator.SetInteger(WeaponTypeHash, WeaponToIndex(newWeapon));
    }

    private void OnAttackTriggered()
    {
        animator.SetTrigger(AttackHash);
    }

    private void OnDeath()
    {
        animator.SetBool(IsDeadHash, true);
    }

    private int WeaponToIndex(Weapon w)
    {
        // convention: 0 = melee, 1 = ranged
        if (w is RangeWeapon) return 1;
        if (w is MeleeWeapon) return 0;
        return 0;
    }
}
