using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    /// <summary>Check to update with each build.</summary>
    [Serializable]
    public sealed class UpdateBuildOption : VersionOptionBase, IPostprocessBuildWithReport {

        [SerializeField] private bool Update_Build;
        [NonSerialized] private VersionModule module;

        int IOrderedCallback.callbackOrder => 0;

        public override void OptionDraw() {
            Update_Build = EditorGUILayout.ToggleLeft(
                EditorGUIUtility.TrTextContent("Check to update with each build"),
                Update_Build);
        }

        public override void SetEvent(VersionModule module)
            => this.module = module;

        public override object Clone()
            => new UpdateBuildOption() {
                Update_Build = this.Update_Build
            };

        public override void Dispose() {
            module = null;
            Update_Build = default;
        }

        public override int GetHashCode()
            => Update_Build.GetHashCode();

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report) {
            if (report.summary.totalErrors != 0 || !Update_Build) return;
            module.Index++;
            ChangeVersionWin.UpdateChangeVersionFile();
        }
    }
}
