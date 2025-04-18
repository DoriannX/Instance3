using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DebugTool
{
    [RequireComponent(typeof(ConsoleLogger))]
    public class LogFilterHandler : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button infoButton;
        [SerializeField] private Button warningButton;
        [SerializeField] private Button errorButton;
        [SerializeField] private Button clearFiltersButton;
        
        [Header("Log Filters")] 
        private ConsoleLogger consoleLogger;

        private void Awake()
        {
            consoleLogger = GetComponent<ConsoleLogger>();
            Assert.IsNotNull(infoButton);
            Assert.IsNotNull(warningButton);
            Assert.IsNotNull(errorButton);
            Assert.IsNotNull(clearFiltersButton);
        }
        

        private void Start()
        {
            infoButton.onClick.AddListener(ToggleInfoLogs);
            warningButton.onClick.AddListener(ToggleWarningLogs);
            errorButton.onClick.AddListener(ToggleErrorLogs);
            clearFiltersButton.onClick.AddListener(ClearFilters);
        }
        
        public void ClearFilters()
        {
            consoleLogger.ShowLog(LogType.Error, true);
            consoleLogger.ShowLog(LogType.Warning, true);
            consoleLogger.ShowLog(LogType.Log, true);
        }

        public void ToggleInfoLogs()
        {
            consoleLogger.ShowLog(LogType.Log, !consoleLogger.showInfo);
        }

        public void ToggleWarningLogs()
        {
            consoleLogger.ShowLog(LogType.Warning, !consoleLogger.showWarning);
        }

        public void ToggleErrorLogs()
        {
            consoleLogger.ShowLog(LogType.Error, !consoleLogger.showError);
        }

    }
}