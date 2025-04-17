using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponCardUI : MonoBehaviour
{
    [SerializeField] private Image      icon;
    [SerializeField] private TMP_Text   nameLabel;
    [SerializeField] private TMP_Text   descLabel;
    [SerializeField] private TMP_Text   damageLabel;
    [SerializeField] private TMP_Text   speedLabel;
    [SerializeField] private GameObject selectedOverlay;
    [SerializeField] private Button     selectButton;

    /// <summary>
    /// Call once to (re)initialize this card.
    /// </summary>
    public void Initialize(WeaponData data, Action onClicked)
    {
        // clear old
        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            if (onClicked != null)
            {
                selectButton.onClick.AddListener(() => onClicked());
                selectButton.interactable = true;
            }
            else
            {
                selectButton.interactable = false;
            }
        }
        if (selectedOverlay != null)
            selectedOverlay.SetActive(false);

        // blank if no data
        if (data == null)
        {
            if (icon        != null) icon.sprite      = null;
            if (nameLabel   != null) nameLabel.text   = "";
            if (descLabel   != null) descLabel.text   = "";
            if (damageLabel != null) damageLabel.text = "";
            if (speedLabel  != null) speedLabel.text  = "";
            return;
        }

        // fill fields
        if (icon        != null) icon.sprite      = data.icon;
        if (nameLabel   != null) nameLabel.text   = data.weaponName;
        if (descLabel   != null) descLabel.text   = data.description;
        if (damageLabel != null) damageLabel.text = $"{data.damage}";
        if (speedLabel  != null) speedLabel.text  = $"{1f/data.cooldown:0.0}/s";
    }

    public void SetSelected(bool selected)
    {
        if (selectedOverlay != null)
            selectedOverlay.SetActive(selected);
    }
}
