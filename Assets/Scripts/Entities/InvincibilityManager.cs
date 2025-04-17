using UnityEngine;

namespace Entities
{
    public abstract class InvincibilityManager : MonoBehaviour
    {
        [SerializeField] protected float invulnerabilityDuration = 0.1f;

        public bool isInvulnerable { get; protected set; }

        public abstract void CheckVulnerability();
    }
}