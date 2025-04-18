using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeMachine
{
    public class UpgradeMachine : MonoBehaviour
    {
        [Header("Player References")] 
        [SerializeField] private Player player;
        [SerializeField] private PlayerUpgrade playerUpgrade;

        [Header("Upgrade Machine References")] 
        [SerializeField] private UpgradeMachineSelector speedSelector;
        [SerializeField] private UpgradeMachineSelector cooldownSelector;
        [SerializeField] private UpgradeMachineSelector healthSelector;
        [SerializeField] private UpgradeMachineSelector ammoSelector;

        [Header("UI References")] 
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Button buyButton;
        [SerializeField] private GameObject priceWarningPanel;

        private int cost;
        private int speedCost;
        private int cooldownCost;
        private int healthCost;
        private int ammoCost;

        private void Awake()
        {
            UpdateCostText();

            speedSelector.OnDesiredLevelChanged += OnSpeedSelected;
            cooldownSelector.OnDesiredLevelChanged += OnCooldownSelected;   
            healthSelector.OnDesiredLevelChanged += OnHealthSelected;
            ammoSelector.OnDesiredLevelChanged += OnAmmoSelected;

            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnSpeedSelected(int desiredLevel)
        {
            speedCost = playerUpgrade.SpeedUpgrade.GetCost(desiredLevel);

            UpdateCostText();
        }

        private void OnCooldownSelected(int desiredLevel)
        {
            cooldownCost = playerUpgrade.CooldownUpgrade.GetCost(desiredLevel);

            UpdateCostText();
        }

        private void OnHealthSelected(int desiredLevel)
        {
            healthCost = playerUpgrade.HealthUpgrade.GetCost(desiredLevel);

            UpdateCostText();
        }

        private void OnAmmoSelected(int desiredLevel)
        {
            ammoCost = playerUpgrade.AmmoUpgrade.GetCost(desiredLevel);

            UpdateCostText();
        }

        private void UpdateCostText()
        {
            cost = speedCost + cooldownCost + healthCost + ammoCost;
            costText.text = $"{cost}c";
        }

        private void OnBuyButtonClicked()
        {
            if (player.Chips < cost)
            {
                priceWarningPanel.SetActive(true);
                return;
            }

            player.SetChips(player.Chips - cost);
            
            UpgradeSelectors();
        }
        
        private void UpgradeSelectors()
        {
            speedSelector.UpgradeLevel();
            cooldownSelector.UpgradeLevel();
            healthSelector.UpgradeLevel();
            ammoSelector.UpgradeLevel();
        }

        public void ClosePriceWarningPanel()
        {
            priceWarningPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            speedSelector.OnDesiredLevelChanged -= OnSpeedSelected;
            cooldownSelector.OnDesiredLevelChanged -= OnCooldownSelected;
            healthSelector.OnDesiredLevelChanged -= OnHealthSelected;
            ammoSelector.OnDesiredLevelChanged -= OnAmmoSelected;

            buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }
}