using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace UpgradeMachine
{
    public class UpgradeMachine : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private PlayerUpgrade playerUpgrade;
        
        [Header("Upgrade Machine References")]
        [SerializeField] private UpgradeMachineSelector speedSelector;
        [SerializeField] private UpgradeMachineSelector cooldownSelector;
        [SerializeField] private UpgradeMachineSelector healthSelector;
        [SerializeField] private UpgradeMachineSelector ammoSelector;
        
        [Header("UI References")]
        [SerializeField] private TMP_Text costText;

        //private int cost;
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
            int cost = speedCost + cooldownCost + healthCost + ammoCost;
            costText.text = $"{cost}c";
        }
        
        private void OnDestroy()
        {
            speedSelector.OnDesiredLevelChanged -= OnSpeedSelected;
            cooldownSelector.OnDesiredLevelChanged -= OnCooldownSelected;
            healthSelector.OnDesiredLevelChanged -= OnHealthSelected;
            ammoSelector.OnDesiredLevelChanged -= OnAmmoSelected;
        }
    }
}