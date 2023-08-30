using System;
using UnityEngine;
using System.Collections.Generic;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    [Serializable]
    public sealed class SerializationModule : IDisposable {

        [SerializeField] private string object_type;
        [SerializeField] private string name;
        [SerializeField] private string change_format;
        [SerializeField] private ulong index;

        public void Dispose() {

        }

        public static void Serialize(string path, ChangeVersionModuleBase serialized) { }

        public static ChangeVersionModuleBase Deserialize(string path) {
            return null;
        }

        public readonly struct Module {
            public readonly string name;
            public readonly string type;
        }
    }
}
