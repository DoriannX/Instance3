#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class EditorOnlyUsingAnalyzer : EditorWindow
{
    private Vector2 _scroll;
    private List<EditorOnlyUsingInfo> _editorOnlyUsings = new List<EditorOnlyUsingInfo>();
    private bool _isAnalyzing = false;
    private string _analysisStatus = "";
    private readonly HashSet<string> _editorNamespaces = new HashSet<string>
    {
        "UnityEditor",
        "UnityEditor.Animations",
        "UnityEditor.AssetImporters",
        "UnityEditor.Collaboration",
        "UnityEditor.Experimental",
        "UnityEditor.PackageManager",
        "UnityEditor.Profiling",
        "UnityEditor.SceneManagement",
        "UnityEditor.Search",
        "UnityEditor.Timeline",
        "UnityEditor.U2D",
        "UnityEditor.UIElements",
        "UnityEditor.VFX"
    };

    [MenuItem("Tools/Editor-Only Using Analyzer")]
    public static void ShowWindow()
    {
        GetWindow<EditorOnlyUsingAnalyzer>("Editor-Only Using Analyzer");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Detect Editor-Only Using Statements", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This tool analyzes all C# scripts in your project to find editor-only using statements that would cause build errors.", MessageType.Info);
        EditorGUILayout.Space(5);

        GUI.enabled = !_isAnalyzing;
        if (GUILayout.Button(_isAnalyzing ? "Analyzing..." : "Analyze Scripts"))
        {
            _editorOnlyUsings.Clear();
            _isAnalyzing = true;
            _analysisStatus = "Analyzing...";
            EditorApplication.delayCall += AnalyzeScripts;
        }
        GUI.enabled = true;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField(_analysisStatus, EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        if (_editorOnlyUsings.Count > 0)
        {
            EditorGUILayout.LabelField($"Found {_editorOnlyUsings.Count} editor-only usings that will cause build errors:", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            foreach (var info in _editorOnlyUsings)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Script:", EditorStyles.boldLabel, GUILayout.Width(50));
                if (GUILayout.Button(info.ScriptPath, EditorStyles.linkLabel))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<Object>(info.ScriptPath);
                    if (asset != null)
                        AssetDatabase.OpenAsset(asset);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Line:", EditorStyles.boldLabel, GUILayout.Width(50));
                EditorGUILayout.LabelField(info.LineNumber.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Using:", EditorStyles.boldLabel, GUILayout.Width(50));
                EditorGUILayout.LabelField(info.UsingStatement, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("Potential Fix:", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Wrap with #if UNITY_EDITOR / #endif or move to an Editor folder.", MessageType.Info);
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }
        else if (!_isAnalyzing && _analysisStatus != "")
        {
            EditorGUILayout.LabelField("No editor-only usings found!", EditorStyles.boldLabel);
        }

        EditorGUILayout.EndScrollView();
    }

    private void AnalyzeScripts()
    {
        try
        {
            string[] scriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            int totalScripts = scriptPaths.Length;
            int processedScripts = 0;

            foreach (string fullPath in scriptPaths)
            {
                string relativePath = "Assets" + fullPath.Replace(Application.dataPath, "").Replace("\\", "/");
                
                // Skip scripts in Editor folders - they're allowed to use UnityEditor
                if (relativePath.Contains("/Editor/") || Path.GetDirectoryName(relativePath).EndsWith("Editor"))
                {
                    processedScripts++;
                    continue;
                }

                string[] lines = File.ReadAllLines(fullPath);
                bool insideEditorConditional = false;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    // Check for editor conditionals
                    if (line.Contains("#if UNITY_EDITOR") || line.Contains("#if UNITY_EDITOR"))
                    {
                        insideEditorConditional = true;
                    }
                    else if (line.StartsWith("#endif") && insideEditorConditional)
                    {
                        insideEditorConditional = false;
                    }
                    // Skip lines inside editor conditionals
                    else if (!insideEditorConditional && line.StartsWith("using ") && line.EndsWith(";"))
                    {
                        // Extract namespace
                        string ns = line.Substring(6, line.Length - 7).Trim();
                        
                        // Check if it's an editor namespace
                        if (_editorNamespaces.Any(editorNs => ns == editorNs || ns.StartsWith(editorNs + ".")))
                        {
                            _editorOnlyUsings.Add(new EditorOnlyUsingInfo
                            {
                                ScriptPath = relativePath,
                                LineNumber = i + 1,
                                UsingStatement = line
                            });
                        }
                    }
                }

                processedScripts++;
            }

            _analysisStatus = $"Analysis complete! Checked {totalScripts} scripts.";
        }
        catch (Exception ex)
        {
            _analysisStatus = "Error during analysis: " + ex.Message;
            Debug.LogError("Editor-Only Using Analyzer error: " + ex);
        }
        finally
        {
            _isAnalyzing = false;
            Repaint();
        }
    }

    private class EditorOnlyUsingInfo
    {
        public string ScriptPath { get; set; }
        public int LineNumber { get; set; }
        public string UsingStatement { get; set; }
    }
}
#endif