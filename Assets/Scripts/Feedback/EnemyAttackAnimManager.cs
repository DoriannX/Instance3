using UnityEngine;

namespace Feedback
{
    public class EnemyAttackAnimManager : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int HitCount = Animator.StringToHash("HitCount");
        private Animator animator;
        private RangeWeapon rangeWeapon;
        private MeleeWeapon meleeWeapon;
        [SerializeField] private Transform parent;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rangeWeapon = parent.GetComponentInChildren<RangeWeapon>();
            meleeWeapon = parent.GetComponentInChildren<MeleeWeapon>();
            if (rangeWeapon != null)
                rangeWeapon.OnWeaponUsed += OnShootStarted;
            if (meleeWeapon != null)
                meleeWeapon.OnWeaponUsed += OnHitStarted;
        }

        private void OnShootStarted(int damage)
        {
            animator.SetTrigger(Shoot);
        }
        
        private void OnHitStarted(int damage)
        {
            animator.SetTrigger(Hit);
            animator.SetInteger(HitCount, Random.Range(0, 4));
        }
    }
}