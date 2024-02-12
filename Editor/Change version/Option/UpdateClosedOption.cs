using System;
using UnityEngine;
using UnityEditor;

namespace Cobilas.Unity.Editor.ChangeVersion.Option {
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
            EditorApplication.wantsToQuit += EditorApplication_quitting;
        }

        private bool EditorApplication_quitting() {
            if (!Update_Closed) return true;
            module.Index++;
			return true;
        }

        public override object Clone()
            => new UpdateClosedOption() {
                Update_Closed = this.Update_Closed
            };

        public override int GetHashCode()
            => Update_Closed.GetHashCode();

        public override void Dispose() {
            module = null;
            Update_Closed = default;
            try {
                EditorApplication.wantsToQuit -= EditorApplication_quitting;
            } catch { }
        }
    }
}
