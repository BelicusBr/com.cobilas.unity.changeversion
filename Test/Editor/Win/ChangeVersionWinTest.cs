using UnityEditor;
using UnityEngine;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    public class ChangeVersionWinTest : EditorWindow {

        private static VersionTemplateTarget target = new MMBR_Template();

        [MenuItem("Window/Change version test")]
        private static void Init() {
            ChangeVersionWinTest test = GetWindow<ChangeVersionWinTest>();
            test.Show();
        }

        [InitializeOnLoadMethod]
        private static void FuncSencond() {

        }

        private Vector2 scrollView;

        private void OnGUI() {
            if (target == null) return;
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            for (int I = 0; I < target.ModuleCount; I++)
                target.Modules[I].DrawModule();
            EditorGUILayout.EndScrollView();
        }
    }
}