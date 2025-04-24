using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private Player player;
    [SerializeField] private Transform weaponHolder;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        player = GetComponent<Player>();
    }

    public void TakeWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            Debug.LogWarning("Attempted to take null weapon data");
            return;
        }

        var weaponInstance = Instantiate(weaponData.weaponPrefab, weaponHolder, false);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        Debug.Log($"[WeaponManager] Instantiated '{weaponInstance.name}' under '{weaponHolder.name}'");

        playerAttack.TakeWeapon(weaponInstance);
        
        player.NotifyWeaponSwitched();
    }


    /// <summary>
    /// Exposes what the player currently has equipped (for the UI swap).
    /// </summary>
    public WeaponData CurrentWeaponData =>
        playerAttack.currentWeapon != null
            ? playerAttack.currentWeapon.Data
            : null;
}