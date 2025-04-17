using UnityEngine;

namespace UpgradeMachine
{
    public class UpgradeMachineSelector : MonoBehaviour
    {
        [SerializeField] private UpgradeMachineButton[] upgradeMachineButtons;
        
        private int currentLevel;
        private int maxLevel = 5;
        private int desiredLevel;

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
            if (button.State == UpgradeMachineButtonState.Selected)
            {
                ChangeButtonState(currentLevel, button.Level, UpgradeMachineButtonState.Selected);
                desiredLevel = button.Level;
            }
            else if (button.State == UpgradeMachineButtonState.Unselected)
            {
                ChangeButtonState(button.Level, desiredLevel, UpgradeMachineButtonState.Unselected);
                desiredLevel = button.Level - 1;
            }
            
        }
        
        private void ChangeButtonState(int origin, int end, UpgradeMachineButtonState state)
        {
            for (int i = origin; i < end; i++)
            {
                if (upgradeMachineButtons[i].State != state)
                    upgradeMachineButtons[i].ChangeButtonState(state);
            }
        }
    }
}