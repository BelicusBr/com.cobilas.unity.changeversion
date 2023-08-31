using System;
using UnityEngine;
using UnityEditor;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    /// <summary>Update When Close.</summary>
    [Serializable]
    public sealed class UpdateClosedOption : VersionOptionBase {

        [SerializeField] private bool Update_Closed;
        [NonSerialized] private VersionModule module;

        public override void OptionDraw() {
            Update_Closed = EditorGUILayout.ToggleLeft(
                EditorGUIUtility.TrTextContent("Update When Close"),
                Update_Closed);
        }

        public override void SetEvent(VersionModule module) {
            this.module = module;
            EditorApplication.quitting += EditorApplication_quitting;
        }

        private void EditorApplication_quitting() {
            module.Index++;
        }

        public override void Dispose() {
            module = null;
            Update_Closed = default;
            try {
                EditorApplication.quitting -= EditorApplication_quitting;
            } catch { }
        }
    }
}
