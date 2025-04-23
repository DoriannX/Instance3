using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewCardUI : MonoBehaviour
{
    [SerializeField] private Image    icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text damageLabel;
    [SerializeField] private TMP_Text speedLabel;

    /// <summary>
    /// Populates this preview card with data (no clicks, no overlays).
    /// </summary>
    public void Initialize(WeaponData data)
    {
        if (icon        != null) icon.sprite      = data?.icon;
        if (nameLabel   != null) nameLabel.text   = data?.weaponName ?? "";
        if (damageLabel != null) damageLabel.text = data != null ? $"{data.damage}" : "";
        if (speedLabel  != null) speedLabel.text  = data != null ? $"{1f/data.cooldown:0.0}/s" : "";
    }
}