using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace TooltipsSystem
{
    public abstract class Tooltip : MonoBehaviour
    {
        [SerializeField] private SO_TooltipText tooltipText;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private CanvasGroup tooltipPanel;
        
        public event Action onTooltipOpen;
        public event Action onTooltipClose;

        protected virtual void Awake()
        {
            Assert.IsNotNull(tooltipText);
            Assert.IsNotNull(headerText);
            Assert.IsNotNull(contentText);
            tooltipPanel.alpha = 0;
        }

        protected virtual void Start()
        {
            headerText.text = tooltipText.Header;
            contentText.text = tooltipText.Content;
        }

        public virtual void OpenTooltip()
        {
            tooltipPanel.alpha = 1;
            onTooltipOpen?.Invoke();
        }
        
        public virtual void CloseTooltip()
        {
            tooltipPanel.alpha = 0;
            onTooltipClose?.Invoke();
        }
    }
}