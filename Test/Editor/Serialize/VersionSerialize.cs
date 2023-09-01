using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Utility;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public sealed class VersionSerialize : IDisposable {
        [SerializeField] private string typename;
        [SerializeField] private string name;
        [SerializeField] private VersionModule[] modules;

        public VersionSerialize(VersionTemplateTarget target) {
            typename = target.GetType().FullName;
            name = target.Name;

            modules = new VersionModule[target.ModuleCount];
            for (int I = 0; I < modules.Length; I++)
                modules[I] = (VersionModule)target[I].Clone();
        }

        public VersionTemplateTarget GetTemplate() {
            Type type = UnityTypeUtility.GetType(typename);
            VersionTemplateTarget temp = (VersionTemplateTarget)Activator.CreateInstance(type);
            VersionModule[] c_modules = new VersionModule[ArrayManipulation.ArrayLength(modules)];
            for (int I = 0; I < c_modules.Length; I++)
                c_modules[I] = (VersionModule)modules[I].Clone();

            temp.Set(name, c_modules);
            return temp;
        }

        public void Dispose() {
            typename = name = string.Empty;
            for (int I = 0; I < ArrayManipulation.ArrayLength(modules); I++)
                modules[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref modules);
        }
    }
}
