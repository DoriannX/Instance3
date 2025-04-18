using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] private MeleeWeapon meleeWeaponComponent;
    [SerializeField] private RangeWeapon rangeWeaponComponent;
    private Entity playerEntity;

    private void Awake()
    {
        playerEntity = GetComponent<Entity>();
    }

    /// <summary>
    /// Installs the new weapon data and fires the OnWeaponChanged event.
    /// </summary>
    public void SwapWeapon(WeaponData newData)
    {
        if (newData is RangeWeaponData)
        {
            rangeWeaponComponent.gameObject.SetActive(true);
            meleeWeaponComponent.gameObject.SetActive(false);
            rangeWeaponComponent.LoadData(newData);
            playerEntity.SetCurrentWeapon(rangeWeaponComponent);
        }
        else // Melee
        {
            meleeWeaponComponent.gameObject.SetActive(true);
            rangeWeaponComponent.gameObject.SetActive(false);
            meleeWeaponComponent.LoadData(newData);
            playerEntity.SetCurrentWeapon(meleeWeaponComponent);
        }
    }

    /// <summary>
    /// Exposes what the player currently has equipped (for the UI swap).
    /// </summary>
    public WeaponData CurrentWeaponData
    {
        get
        {
            var w = playerEntity.CurrentWeapon;
            return w != null ? w.Data : null;
        }
    }
}