using UnityEngine;

public class DamageTester : MonoBehaviour
{
    [SerializeField] private EntityHealth playerHealth; // Assign your player's EntityHealth component here.
    [SerializeField] private int damageAmount = 5; // Damage to apply per test.

    // This method can be called by a UI Button.
    public void ApplyDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log($"Applied {damageAmount} damage to {playerHealth.gameObject.name}");
        }
        else
        {
            Debug.LogWarning("PlayerHealth component is not assigned in DamageTester!");
        }
    }
}