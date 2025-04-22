using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings"), Min(0.1f)] [SerializeField]
    private float dashDistance = 3.0f;

    [field: SerializeField, Min(0.01f)] public float dashDuration { get; private set; } = 0.2f;
    [SerializeField, Min(0)] private float dashCooldown = 0.25f;

    public float lastDashTime { get; private set; }
    public bool isDashing { get; private set; }

    public event Action OnDash;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        AllowImmediateDash();
    }

    private void AllowImmediateDash()
    {
        lastDashTime = Time.time - dashCooldown - dashDuration;
    }

    public void StartDash()
    {
        bool isDashOnCooldown = Time.time < lastDashTime + dashDuration + dashCooldown;
        if (isDashing || isDashOnCooldown)
        {
            return;
        }

        OnDash?.Invoke();

        isDashing = true;
        lastDashTime = Time.time;
    }

    public void HandleDash()
    {
        if (isDashing)
        {
            playerMovement.SetVelocity(playerMovement.lastMoveDirection.normalized *
                                       (dashDistance / dashDuration));
            CheckDashFinish();
        }
    }

    private void CheckDashFinish()
    {
        bool isDashFinished = Time.time > lastDashTime + dashDuration;
        if (isDashFinished)
        {
            isDashing = false;
            playerMovement.SetVelocity(Vector3.zero);
        }
    }
}