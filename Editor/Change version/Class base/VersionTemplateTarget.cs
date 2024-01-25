using System;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.Unity.Editor.ChangeVersion.Template {
    [Serializable]
    public abstract class VersionTemplateTarget : IDisposable, ICloneable {
        public abstract string Name { get; }
        public abstract int ModuleCount { get; }
        public abstract VersionModule[] Modules { get; }
        public abstract VersionModule this[int index] { get; }

        public abstract object Clone();
        public abstract void Dispose();
        public abstract void Set(string name, VersionModule[] modules);

        public static IEnumerable<VersionTemplateTarget> GetTemplates() {
            Type[] types = TypeUtilitarian.GetTypes();

            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++)
                if (types[I].IsSubclassOf(typeof(VersionTemplateTarget)))
                    yield return (VersionTemplateTarget)Activator.CreateInstance(types[I]);
        }
    }
}
