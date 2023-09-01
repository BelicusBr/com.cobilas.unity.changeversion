﻿using System;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public abstract class VersionTemplateTarget {
        public abstract string Name { get; }
        public abstract int ModuleCount { get; }
        public abstract VersionModule[] Modules { get; }
        public abstract VersionModule this[int index] { get; }

        ~VersionTemplateTarget() {
            UnityEngine.Debug.Log("Destroy");
            ChangeVersionResource.UnloadChangeVersionFile(this);
        }

        public new abstract int GetHashCode();
        public abstract void Set(string name, VersionModule[] modules);

        public static IEnumerable<VersionTemplateTarget> GetTemplates() {
            Type[] types = UnityTypeUtility.GetAllTypes();

            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++)
                if (types[I].IsSubclassOf(typeof(VersionTemplateTarget)))
                    yield return (VersionTemplateTarget)Activator.CreateInstance(types[I]);
        }
    }
}