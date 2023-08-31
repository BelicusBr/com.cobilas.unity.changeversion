using System;
using UnityEngine;
using UnityEditor;
using Cobilas.Collections;
using UnityEditor.Compilation;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    /// <summary>Mark revision after completion of compilation.</summary>
    [Serializable]
    public sealed class UpdateRevisionOption : VersionOptionBase {

        [SerializeField] private bool Update_Revision;
        [NonSerialized] private VersionModule module;

        public override void OptionDraw() {
            Update_Revision = EditorGUILayout.ToggleLeft(
                EditorGUIUtility.TrTextContent("Mark revision after completion of compilation"),
                Update_Revision);
        }

        public override void SetEvent(VersionModule module) {
            this.module = module;
            CompilationPipeline.assemblyCompilationFinished += CompilationPipeline_assemblyCompilationFinished;
        }

        private void CompilationPipeline_assemblyCompilationFinished(string arg1, CompilerMessage[] arg2) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(arg2); I++)
                if (arg2[I].type == CompilerMessageType.Error)
                    return;
            module.Index++;
        }

        public override void Dispose() {
            module = null;
            Update_Revision = default;
            try {
                CompilationPipeline.assemblyCompilationFinished -= CompilationPipeline_assemblyCompilationFinished;
            } catch { }
        }
    }
}
