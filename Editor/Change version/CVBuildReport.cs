using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Cobilas.Unity.Editor.ChangeVersion {
    internal class CVBuildReport : IPostprocessBuildWithReport, IPreprocessBuildWithReport {
        public static event Action<BuildReport>OnPostprocessBuild;
        public static event Action<BuildReport>OnPreprocessBuild;
        
        int IOrderedCallback.callbackOrder => 0;

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
        {
            OnPostprocessBuild?.Invoke(report);
        }

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            OnPreprocessBuild?.Invoke(report);
        }
    }
}