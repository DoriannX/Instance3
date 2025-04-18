using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DebugTool
{
    public class ConsoleLogger : MonoBehaviour
    {
        [SerializeField] private int maxLogLines = 50;
        [field: SerializeField] public bool showInfo { get; private set; } = true;
        [field: SerializeField] public bool showWarning { get; private set; } = true;
        [field: SerializeField] public bool showError { get; private set; } = true;
        [SerializeField] private Button clearButton;
        [SerializeField] private TextMeshProUGUI console;
        private readonly List<LogEntry> logEntries = new();

        private void Awake()
        {
            Assert.IsNotNull(clearButton);
            Assert.IsNotNull(console);
            Application.logMessageReceived += HandleLog;
            AddLogEntry("Console initialized...", LogType.Log);
        }

        private void Start()
        {
            clearButton.onClick.AddListener(ClearConsole);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("test log");
                Debug.LogWarning("test warning log");
                Debug.LogError("test error log");
            }
        }
        
        public void ShowLog(LogType type, bool show)
        {
            switch (type)
            {
                case LogType.Log:
                    showInfo = show;
                    break;
                case LogType.Warning:
                    showWarning = show;
                    break;
                case LogType.Error:
                case LogType.Exception:
                    showError = show;
                    break;
            }
            UpdateConsoleText();
        }
        public void ClearConsole()
        {
            logEntries.Clear();
            AddLogEntry("Console cleared...", LogType.Log);
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            AddLogEntry(logString, type);
        }

        private void AddLogEntry(string message, LogType type)
        {
            logEntries.Add(new LogEntry(message, type));

            // Trim log if it gets too long
            while (logEntries.Count > maxLogLines)
            {
                logEntries.RemoveAt(0);
            }

            UpdateConsoleText();
        }

        private bool ShouldShowLogType(LogType type)
        {
            return type switch
            {
                LogType.Error or LogType.Exception => showError,
                LogType.Warning => showWarning,
                LogType.Log => showInfo,
                _ => true
            };
        }

        private void UpdateConsoleText()
        {
            if (console == null)
                return;

            string displayText = "";

            foreach (var entry in logEntries)
            {
                if (ShouldShowLogType(entry.Type))
                {
                    string colorCode = entry.Type switch
                    {
                        LogType.Error => "<color=red>",
                        LogType.Warning => "<color=yellow>",
                        LogType.Exception => "<color=red>",
                        _ => "<color=white>"
                    };

                    displayText += $"{colorCode}[{entry.Type}] {entry.Message}</color>\n";
                }
            }

            console.text = displayText;
        }

        private class LogEntry
        {
            public string Message { get; }
            public LogType Type { get; }

            public LogEntry(string message, LogType type)
            {
                Message = message;
                Type = type;
            }
        }
    }
}