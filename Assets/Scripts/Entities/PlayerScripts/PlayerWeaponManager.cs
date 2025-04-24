using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    private PlayerAttack playerAttack;
    [SerializeField] private Transform weaponHolder;

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

        var weaponInstance = Instantiate(weaponData.weaponPrefab, weaponHolder, false);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        Debug.Log($"[WeaponManager] Instantiated '{weaponInstance.name}' under '{weaponHolder.name}'");

        playerAttack.TakeWeapon(weaponInstance);
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