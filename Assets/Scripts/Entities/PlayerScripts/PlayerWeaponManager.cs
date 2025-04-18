using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void TakeWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            Debug.LogWarning("Attempted to take null weapon data");
            return;
        }
        
        playerAttack.TakeWeapon(Instantiate(weaponData.weaponPrefab, playerAttack.transform));
    }

    /// <summary>
    /// Exposes what the player currently has equipped (for the UI swap).
    /// </summary>
    public WeaponData CurrentWeaponData
    {
        get
        {
            var w = playerAttack.currentWeapon;
            return w != null ? w.Data : null;
        }
    }
}