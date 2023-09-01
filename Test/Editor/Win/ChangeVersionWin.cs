using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    public class ChangeVersionWin : EditorWindow, ISerializationCallbackReceiver {

        private static EditorCoroutine coroutine;
        private static VersionTemplateTarget target;

        [MenuItem("Window/Change version test")]
        private static void Init() {
            ChangeVersionWin test = GetWindow<ChangeVersionWin>();
            test.targets = new List<VersionTemplateTarget>(VersionTemplateTarget.GetTemplates());
            test.displayName = new string[test.targets.Count + 1];
            test.displayName[0] = "None";
            for (int I = 1; I < test.displayName.Length; I++)
                test.displayName[I] = test.targets[I - 1].Name;
            FuncSencond();
            test.Show();
        }
         
        [InitializeOnLoadMethod]
        private static void FuncSencond() {
            target = ChangeVersionResource.LoadChangeVersionFile();

            EditorApplication.quitting += () => {
                if (coroutine != null)
                    EditorCoroutineUtility.StopCoroutine(coroutine);
                ChangeVersionResource.UnloadChangeVersionFile(target);
            };
        }

        public static void UpdateChangeVersionFile() {
            if (target is null) return;
            ChangeVersionResource.UnloadChangeVersionFile(target);
        }

        private static IEnumerator VerifikTarget() {
            int hashcode = 0;
            while (true) {
                yield return new WaitForSeconds(1f);
                if (target == null) continue;
                if (hashcode != (hashcode = target.GetHashCode()))
                    ChangeVersionResource.UnloadChangeVersionFile(target);
            }
        }

        private Vector2 scrollView;
        [SerializeField] private string[] displayName;
        [SerializeField] private int displayNameIndex;
        [SerializeField] private List<VersionTemplateTarget> targets;

        private void OnGUI() {
            
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            /*
             * aqui tenho que perceber a alterção do popup
             * para manipular o template salvo
             */
            displayNameIndex = EditorGUILayout.Popup(
                    EditorGUIUtility.TrTextContent("Version tamplate"),
                    displayNameIndex,
                    displayName
                );
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            if (displayNameIndex == 0) {
                target = (VersionTemplateTarget)null;
            } else {
                target = targets[displayNameIndex - 1];
                for (int I = 0; I < target.ModuleCount; I++)
                    target[I].DrawModule();
            }

            EditorGUILayout.EndScrollView();
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}