using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator & Parameters")]
    [SerializeField] private Animator animator;
    [SerializeField] private float movementThreshold = 0.1f;
    private static readonly int SpeedHash      = Animator.StringToHash("Speed");
    private static readonly int AttackHash     = Animator.StringToHash("Attack");
    private static readonly int IsDeadHash     = Animator.StringToHash("IsDead");
    private static readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");
    private static readonly int IsMoving = Animator.StringToHash("Moving");

    private Player        player;
    private EntityHealth  health;
    private PlayerAttack  attack;
    private Rigidbody     rb;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        health = GetComponent<EntityHealth>();
        attack = GetComponent<PlayerAttack>();
        rb     = GetComponent<Rigidbody>();

        // Set initial weapon type based on currently equipped weapon
        animator.SetFloat(WeaponTypeHash, WeaponToIndex(player.CurrentWeapon));
    }

    private void OnEnable()
    {
        player.onWeaponSwitched += RefreshWeaponAnimation;
        attack.onAttackStarted  += OnAttackTriggered;
        health.onDeath          += OnDeath;
    }

    private void OnDisable()
    {
        player.onWeaponSwitched -= RefreshWeaponAnimation;
        attack.onAttackStarted  -= OnAttackTriggered;
        health.onDeath          -= OnDeath;
    }

    private void Update()
    {
        if (rb != null)
        {
            Vector3 horiz = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float speed = Vector3.Dot(horiz, player.transform.forward);
            animator.SetFloat(SpeedHash, speed);
            animator.SetBool(IsMoving, Mathf.Abs(speed) > movementThreshold);
        }
    }

    private void RefreshWeaponAnimation()
    {
        animator.SetFloat(WeaponTypeHash, WeaponToIndex(player.CurrentWeapon));
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
        // Convention: 0 = melee, 1 = ranged
        if (w is RangeWeapon) return 1;
        return 0;
    }
}
