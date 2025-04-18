using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using UnityEngine;
using TMPro;

namespace DebugTool
{
    public class RuntimeCodeEvaluator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField codeInputField;
        
        // Référence au GameObject auquel vous voulez accéder depuis le code
        [SerializeField] private Player player;
        
        // Dictionnaire pour stocker les variables globales
        private Dictionary<string, object> globalVariables = new();
        
        // Liste des assemblies à référencer lors de la compilation
        private List<string> requiredAssemblies = new()
        {
            "mscorlib",
            "System",
            "System.Core",
            "System.Data",
            "System.Runtime",
            "UnityEngine",
            "UnityEngine.CoreModule",
            "UnityEngine.UI",
            "UnityEngine.UIModule",
            "UnityEngine.IMGUIModule",
            "UnityEngine.PhysicsModule",
            "UnityEngine.AnimationModule",
            "UnityEngine.InputModule",
            "Unity.TextMeshPro"
        };
    
        private void Start()
        {
            // S'assurer que l'InputField est défini
            if (codeInputField == null)
            {
                Debug.LogError("Veuillez assigner l'InputField dans l'inspecteur!");
                return;
            }
            
            // Initialiser quelques variables globales
            globalVariables["speed"] = 5.0f;
            
            // Configurer l'InputField pour soumettre sur Enter
            codeInputField.onSubmit.AddListener(ExecuteCode);
        }
        
        public void ExecuteCode(string code)
        {
            string result = EvaluateCode(code);
            Debug.Log($"[Code Evaluator Result] {result}");
            codeInputField.text = ""; // Effacer l'input field après exécution
        }
        
        public string EvaluateCode(string code)
        {
            try
            {
                // Wrapper le code dans une méthode
                string wrappedCode = WrapCodeInMethod(code);
                
                // Compiler le code
                Assembly assembly = CompileCode(wrappedCode);
                if (assembly == null)
                    return "Erreur de compilation! Vérifiez la console Unity pour plus de détails.";
                
                // Exécuter le code
                Type debugType = assembly.GetType("RuntimeDebug");
                if (debugType == null)
                    return "Erreur: Type 'RuntimeDebug' non trouvé dans l'assembly compilé.";
                    
                MethodInfo executeMethod = debugType.GetMethod("Execute");
                if (executeMethod == null)
                    return "Erreur: Méthode 'Execute' non trouvée dans le type 'RuntimeDebug'.";
                
                // Créer une instance et passer le contexte (this) et les variables globales
                object instance = Activator.CreateInstance(debugType);
                
                // Définir les propriétés de l'instance pour accéder aux variables globales et à Unity
                debugType.GetField("Context").SetValue(instance, this);
                debugType.GetField("Player").SetValue(instance, player);
                debugType.GetField("GlobalVars").SetValue(instance, globalVariables);
                
                // Exécuter la méthode
                object result = executeMethod.Invoke(instance, null);
                
                // Récupérer les variables globales mises à jour
                Dictionary<string, object> updatedVars = (Dictionary<string, object>)debugType.GetField("GlobalVars").GetValue(instance);
                globalVariables = updatedVars;
                
                // Retourner le résultat
                return result != null ? result.ToString() : "Code exécuté avec succès";
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erreur d'évaluation: {ex.Message}\n{ex.StackTrace}");
                return $"Erreur lors de l'exécution: {ex.Message}";
            }
        }
        
        private string WrapCodeInMethod(string code)
        {
            // Créer un code source complet avec notre code à l'intérieur d'une méthode
            StringBuilder source = new StringBuilder();
            
            // Ajouter tous les usings nécessaires
            source.AppendLine("using System;");
            source.AppendLine("using System.Collections;");
            source.AppendLine("using System.Collections.Generic;");
            source.AppendLine("using System.Linq;");
            source.AppendLine("using UnityEngine;");
            source.AppendLine("using UnityEngine.UI;");
            source.AppendLine("using UnityEngine.SceneManagement;");
            source.AppendLine("using UnityEngine.AI;");
            source.AppendLine("using UnityEngine.Events;");
            source.AppendLine("using TMPro;");
            source.AppendLine("using DebugTool;");
            
            source.AppendLine("public class RuntimeDebug {");
            source.AppendLine("    public DebugTool.RuntimeCodeEvaluator Context;");
            source.AppendLine("    public Player Player;");
            source.AppendLine("    public Dictionary<string, object> GlobalVars;");
            
            source.AppendLine("    public object Execute() {");
            
            // Ajouter notre propre fonction Debug.Log qui redirige vers UnityEngine.Debug.Log
            source.AppendLine("        void Log(object message) { UnityEngine.Debug.Log(message); }");
            source.AppendLine("        void LogWarning(object message) { UnityEngine.Debug.LogWarning(message); }");
            source.AppendLine("        void LogError(object message) { UnityEngine.Debug.LogError(message); }");
            
            // Extraire les variables globales dans des variables locales pour une utilisation facile
            source.AppendLine("        float speed = GlobalVars.ContainsKey(\"speed\") ? Convert.ToSingle(GlobalVars[\"speed\"]) : 0f;");
            source.AppendLine("        Player player = Player;");
            
            // Ajouter le code utilisateur
            source.AppendLine("        object result = null;");
            source.AppendLine("        try {");
            
            // Gérer le cas où le code se termine par une expression sans return
            if (!code.Trim().EndsWith(";") && !code.Contains("return ") && !IsComplexCode(code))
            {
                source.AppendLine($"            result = {code};");
            }
            else 
            {
                source.AppendLine($"            {code}");
            }
            
            source.AppendLine("        } catch (Exception ex) {");
            source.AppendLine("            return $\"Erreur: {ex.Message}\";");
            source.AppendLine("        }");
            
            // Mettre à jour les variables globales
            source.AppendLine("        GlobalVars[\"speed\"] = speed;");
            
            source.AppendLine("        return result;");
            source.AppendLine("    }");
            source.AppendLine("}");
            
            return source.ToString();
        }
        
        private bool IsComplexCode(string code)
        {
            // Si le code contient certains mots-clés, on suppose qu'il s'agit de code complexe
            // et non d'une simple expression
            string[] complexKeywords = { "if", "for", "while", "switch", "class", "struct", "{" };
            return complexKeywords.Any(keyword => code.Contains(keyword));
        }
        
        private Assembly CompileCode(string source)
        {
            try
            {
                // Collecter toutes les références d'assemblies nécessaires
                var references = new List<MetadataReference>();
                
                // Ajouter mscorlib et System
                references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
                references.Add(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location));
                
                // Ajouter l'assembly contenant cette classe
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                references.Add(MetadataReference.CreateFromFile(executingAssembly.Location));
                
                // Ajouter tous les assemblies référencés par l'assembly en cours d'exécution
                foreach (var referencedAssembly in executingAssembly.GetReferencedAssemblies())
                {
                    try
                    {
                        var loadedAssembly = Assembly.Load(referencedAssembly);
                        references.Add(MetadataReference.CreateFromFile(loadedAssembly.Location));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Impossible de charger l'assembly {referencedAssembly.FullName}: {ex.Message}");
                    }
                }
                
                // Ajouter explicitement les assemblies Unity
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string currentAssemblyName = assembly.GetName().Name;
                    
                    // Vérifier si c'est un assembly Unity ou un assembly requis
                    if (currentAssemblyName.StartsWith("Unity") || requiredAssemblies.Contains(currentAssemblyName))
                    {
                        try
                        {
                            string location = assembly.Location;
                            if (!string.IsNullOrEmpty(location))
                            {
                                references.Add(MetadataReference.CreateFromFile(location));
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning($"Impossible d'ajouter l'assembly {currentAssemblyName}: {ex.Message}");
                        }
                    }
                }
                
                // Options de compilation
                CSharpCompilationOptions options = new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug,
                    allowUnsafe: true
                );
                
                // Créer un nom unique pour l'assembly dynamique
                string assemblyName = "RuntimeAssembly_" + DateTime.Now.Ticks;
                
                // Créer la compilation
                CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName,
                    new[] { CSharpSyntaxTree.ParseText(source) },
                    references,
                    options
                );
                
                // Compiler en mémoire
                using (var ms = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(ms);
                    
                    if (!result.Success)
                    {
                        // Afficher les erreurs de compilation en détail
                        Debug.LogError("Erreurs de compilation:");
                        foreach (var diagnostic in result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error))
                        {
                            Debug.LogError($"{diagnostic.Id}: {diagnostic.GetMessage()} sur {diagnostic.Location}");
                        }
                        
                        Debug.LogError("Code source généré:");
                        Debug.LogError(source);
                        
                        return null;
                    }
                    
                    ms.Seek(0, SeekOrigin.Begin);
                    return Assembly.Load(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erreur lors de la compilation: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
        
        // Méthode utilitaire pour appeler depuis le code évalué
        public void ModifyPlayerSpeed(float amount)
        {
            if (player != null)
            {
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Modifier la vitesse du joueur
                    float currentSpeed = (float)globalVariables["speed"];
                    currentSpeed += amount;
                    globalVariables["speed"] = currentSpeed;
                    
                    // Appliquer la vitesse
                    rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
                    
                    Debug.Log($"Vitesse du joueur modifiée à: {currentSpeed}");
                }
            }
        }
    }
}