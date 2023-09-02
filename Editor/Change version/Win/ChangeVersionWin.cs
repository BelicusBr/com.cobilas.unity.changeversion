using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Editor.ChangeVersion.Template;
using Cobilas.Unity.Editor.ChangeVersion.Serialization;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public class ChangeVersionWin : EditorWindow {

        private static VersionTemplateTarget target;

        [MenuItem("Window/Change version")]
        private static void Init() {
            ChangeVersionWin window = GetWindow<ChangeVersionWin>();
            window.titleContent = new GUIContent("Change Version");
            window.minSize = new Vector2(570f, 420f);
            window.Show();
        }
         
        [InitializeOnLoadMethod]
        private static void FuncSencond() {
            if (target == null)
                target = ChangeVersionResource.LoadChangeVersionFile();
            EditorApplication.quitting += () => UpdateChangeVersionFile();
        }

        public static void UpdateChangeVersionFile() {
            if (target is null) return;
            PlayerSettings.bundleVersion = target.ToString();
            ChangeVersionResource.UnloadChangeVersionFile(target);
        }

        private Vector2 scrollView;
        [SerializeField] private string[] displayName;
        [SerializeField] private int displayNameIndex;
        [SerializeField] private List<VersionTemplateTarget> targets;

        private void OnEnable() {

            if (target != null) {
                UpdateChangeVersionFile();
                target.Dispose();
                target = null;
            }

            targets = new List<VersionTemplateTarget>(VersionTemplateTarget.GetTemplates());
            target = ChangeVersionResource.LoadChangeVersionFile();

            if (target != null) {
                targets.Add(target);
                titleContent.text = $"Change Version[{target.Name}]";
            }

            displayName = new string[targets.Count + 1];
            displayName[0] = "None";
            for (int I = 1; I < displayName.Length; I++)
                displayName[I] = targets[I - 1].Name;
        }

        private void OnDestroy() {
            UpdateChangeVersionFile();
            foreach (var item in targets)
                item.Dispose();
            targets.Clear();
            targets.Capacity = 0;
            target = ChangeVersionResource.LoadChangeVersionFile();
        }

        private void OnGUI() {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Print version", GUILayout.Width(100f)))
                ChangeVersionResource.PrintVersion();
            
            EditorGUI.BeginChangeCheck();
            displayNameIndex = EditorGUILayout.Popup(
                    EditorGUIUtility.TrTextContent("Version tamplate"),
                    displayNameIndex,
                    displayName
                );
            if (EditorGUI.EndChangeCheck()) {
                if (displayNameIndex == 0)
                    SetVersionTarget((VersionTemplateTarget)null);
                else SetVersionTarget(targets[displayNameIndex - 1]);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            if (target != null) {
                for (int I = 0; I < target.ModuleCount; I++)
                    target[I].DrawModule();
            }

            EditorGUILayout.EndScrollView();
        }

        private void SetVersionTarget(VersionTemplateTarget version) {
            if (version is null) {
                titleContent.text = "Change Version";
                target = version;
            } else if (!version.Name.Contains("custom")) {
                target = (VersionTemplateTarget)version.Clone();
                target.Set($"custom-{version.Name}", version.Modules);
                titleContent.text = $"Change Version[{version.Name}]";
            }
        }
    }
}