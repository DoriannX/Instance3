using UnityEngine;
using UnityEngine.UI;
using Armory;

public class ArmoryUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject     armoryCanvas;   // root
    [SerializeField] private GameObject     confirmWindow;  // confirm pane

    [Header("Choice Cards")]
    [SerializeField] private WeaponCardUI[] choiceCards;    // 3 static slots

    [Header("Confirm Previews")]
    [SerializeField] private PreviewCardUI  equippedPreview;
    [SerializeField] private PreviewCardUI  selectedPreview;

    [Header("Buttons")]
    [SerializeField] private Button         confirmButton;
    [SerializeField] private Button         cancelButton;

    [Header("Player Hook")]
    [SerializeField] private PlayerWeaponManager weaponManager;
    
    [Header("Testing (no player needed)")]
    [SerializeField] private ArmoryTerminal testTerminal;

    private ArmoryTerminal currentTerminal;
    private WeaponData[]   stock;
    private int            selectedIndex = -1;

    private void Awake()
    {
        // armoryCanvas .SetActive(false);
        confirmWindow.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton .onClick.AddListener(OnCancel);
    }
    
    private void Start()
    {
        if (testTerminal != null)
            Show(testTerminal);
    }

    /// <summary>
    /// Called by ArmoryTerminal.TryOpen(...)
    /// </summary>
    public void Show(ArmoryTerminal terminal)
    {
        currentTerminal = terminal;
        stock           = terminal.Storage.WeaponsInStock;
        selectedIndex   = -1;

        // show armory panel
        armoryCanvas.SetActive(true);
        confirmWindow.SetActive(false);
        confirmButton.interactable = false;
        Time.timeScale = 0f;

        // populate each choice card
        for (int i = 0; i < choiceCards.Length; i++)
        {
            var card = choiceCards[i];
            var data = (stock != null && i < stock.Length) ? stock[i] : null;
            int idx = i;
            card.Initialize(data, () => OnCardClicked(idx));
            card.SetSelected(false);
        }
    }

    private void OnCardClicked(int idx)
    {
        if (idx < 0 || idx >= choiceCards.Length) return;

        // highlight
        if (selectedIndex >= 0)
            choiceCards[selectedIndex].SetSelected(false);

        selectedIndex = idx;
        choiceCards[idx].SetSelected(true);

        // fill confirm previews
        equippedPreview.Initialize(weaponManager.CurrentWeaponData);
        var picked = (stock != null && idx < stock.Length) ? stock[idx] : null;
        selectedPreview.Initialize(picked);

        // show confirm pane
        confirmWindow.SetActive(true);
        confirmButton.interactable = true;
    }

    private void OnConfirm()
    {
        if (currentTerminal == null 
         || selectedIndex < 0 
         || stock == null 
         || selectedIndex >= stock.Length)
            return;

        // swap in storage & equip
        var oldData = currentTerminal.Storage.SwitchWeaponData(
            weaponManager.CurrentWeaponData, selectedIndex
        );
        var newData = stock[selectedIndex];
        weaponManager.SwapWeapon(newData);
        currentTerminal.Storage.SwitchWeaponData(oldData, selectedIndex);

        currentTerminal.NotifyChosen();
        HideAll();
    }

    private void OnCancel()
    {
        if (selectedIndex >= 0)
            choiceCards[selectedIndex].SetSelected(false);

        confirmWindow.SetActive(false);
        confirmButton.interactable = false;
        selectedIndex = -1;
    }

    private void HideAll()
    {
        armoryCanvas .SetActive(false);
        confirmWindow.SetActive(false);
        Time.timeScale = 1f;
    }
}
