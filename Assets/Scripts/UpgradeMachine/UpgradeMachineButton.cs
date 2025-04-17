using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UpgradeMachine
{
    public class UpgradeMachineButton : MonoBehaviour
    {
        [Header("Button States")]
        [SerializeField, Min(0)] private int buttonLevel;
        
        [Header("Button Colors")]
        [SerializeField] private UpgradeMachineButtonColor[] buttonColors;
        
        
        private UpgradeMachineButtonState selfState;
        public event Action<UpgradeMachineButton> OnClickEvent;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>() ?? throw new MissingComponentException("Button component not found on UpgradeMachineButton.");
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            SwitchEnableState();
            OnClickEvent?.Invoke(this);
        }
        
        public void SwitchEnableState()
        {
            if (selfState == UpgradeMachineButtonState.Buy)
                return;
            
            selfState = selfState switch
            {
                UpgradeMachineButtonState.Selected => UpgradeMachineButtonState.Unselected,
                UpgradeMachineButtonState.Unselected => UpgradeMachineButtonState.Selected,
                _ => throw new ArgumentException("Invalid state")
            };
            
            UpdateButtonColor();
        }
        
        public void ChangeButtonState(UpgradeMachineButtonState state)
        {
            if (state == selfState)
                return;
            
            selfState = state;
            UpdateButtonColor();
        }

        private void UpdateButtonColor()
        {
            foreach (var buttonColor in buttonColors)
            {
                if (buttonColor.state != selfState) 
                    continue;
                
                button.colors = buttonColor.color;
                break;
            }
        }
        
        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpgradeMachineButtonState[] enumValues = (UpgradeMachineButtonState[])Enum.GetValues(typeof(UpgradeMachineButtonState));

            if (buttonColors == null || buttonColors.Length != enumValues.Length)
            {
                buttonColors = new UpgradeMachineButtonColor[enumValues.Length];
                
                for (int i = 0; i < enumValues.Length; i++)
                    buttonColors[i].color = ColorBlock.defaultColorBlock;
            }

            for (int i = 0; i < enumValues.Length; i++)
            {
                buttonColors[i].state = enumValues[i];
                buttonColors[i].name = enumValues[i].ToString();
            }
        }
#endif
        
        public int Level => buttonLevel;
        public UpgradeMachineButtonState State => selfState;
    }
}