using System;
using System.ComponentModel;
using UnityEngine;

namespace UpgradeMachine
{
    public class UpgradeMachineSelector : MonoBehaviour
    {
        [SerializeField] private UpgradeMachineButton[] upgradeMachineButtons;
        
        private int currentLevel;
        private int desiredLevel;
        
        public event Action<int> OnDesiredLevelChanged;

        private void Start()
        {
            foreach (var button in upgradeMachineButtons)
            {
                button.ChangeButtonState(UpgradeMachineButtonState.Unselected);
                button.OnClickEvent += ManageButtonClick;
            }
        }
        
        private void ManageButtonClick(UpgradeMachineButton button)
        {
            switch (button.State)
            {
                case UpgradeMachineButtonState.Selected:
                    ChangeButtonState(currentLevel, button.Level, UpgradeMachineButtonState.Selected);
                    desiredLevel = button.Level;
                    break;
                case UpgradeMachineButtonState.Unselected:
                    ChangeButtonState(button.Level, desiredLevel, UpgradeMachineButtonState.Unselected);
                    desiredLevel = button.Level - 1;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Invalid button state");
            }
            
            OnDesiredLevelChanged?.Invoke(desiredLevel);
        }
        
        private void ChangeButtonState(int origin, int end, UpgradeMachineButtonState state)
        {
            for (int i = origin; i < end; i++)
            {
                if (upgradeMachineButtons[i].State != state)
                    upgradeMachineButtons[i].ChangeButtonState(state);
            }
        }
        
        public void UpgradeLevel()
        {
            for (int i = currentLevel; i < desiredLevel; i++)
            {
                upgradeMachineButtons[i].ChangeButtonState(UpgradeMachineButtonState.Buy);
            }
            
            currentLevel = desiredLevel;
        }

        private void OnDestroy()
        {
            foreach (var button in upgradeMachineButtons)
            {
                button.OnClickEvent -= ManageButtonClick;
            }
        }
        
        public int CurrentLevel => currentLevel;
        public int DesiredLevel => desiredLevel;
        
        public bool WantToBeUpgraded => desiredLevel > currentLevel;
    }
}