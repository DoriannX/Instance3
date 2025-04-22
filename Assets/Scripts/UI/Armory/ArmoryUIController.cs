using System.Linq;
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

    private ArmoryTerminal currentTerminal;
    private WeaponData[]   stock;
    private int            selectedIndex = -1;

    private void Awake()
    {
        armoryCanvas .SetActive(false);
        confirmWindow.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton .onClick.AddListener(OnCancel);
    }

    /// <summary>
    /// Called by ArmoryTerminal.TryOpen(...)
    /// </summary>
    public void Show(ArmoryTerminal terminal)
    {
        Debug.Log("show");
        currentTerminal = terminal;
        stock           = terminal.Storage.WeaponsInStock;
        selectedIndex   = -1;
        
        // Validate stock
        Debug.Log(stock.Length);
        if (stock == null || stock.Any(item => item == null))
        {
            Debug.LogWarning("Invalid weapon data found in stock.");
        }

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
    
        WeaponData newWeapon = stock[selectedIndex];
        
        WeaponData oldWeapon = weaponManager.CurrentWeaponData;
    
        weaponManager.TakeWeapon(newWeapon);
        
        currentTerminal.Storage.SwitchWeaponData(oldWeapon, selectedIndex);
        
        stock = currentTerminal.Storage.WeaponsInStock;
    
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
