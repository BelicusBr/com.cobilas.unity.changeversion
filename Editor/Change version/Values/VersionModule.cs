using System;

namespace Cobilas.Unity.Editor.ChangeVersion {
    [Serializable]
    public struct VersionModule {
        /// <summary>Module name.</summary>
        public string name;
        public string format;
        public long index;
        /// <summary>Update after build.</summary>
        public bool upBuild;
        /// <summary>Update after closing.</summary>
        public bool upClose;
        /// <summary>Update after compiling.</summary>
        public bool upCompiler;
        [NonSerialized] public bool foldout;

        public VersionModule(string name, string format, long index, bool upBuild, bool upClose, bool upCompiler) {
            this.name = name;
            this.format = format;
            this.index = index;
            this.upBuild = upBuild;
            this.upClose = upClose;
            this.upCompiler = upCompiler;
            foldout = false;
        }

        public VersionModule(string name, bool upBuild, bool upClose, bool upCompiler) :
            this(name, string.Empty, 0L, upBuild, upClose, upCompiler) {}

        public VersionModule(string name, string format, long index) :
            this(name, format, index, false, false, false) {}

        public VersionModule(string name, long index) :
            this(name, string.Empty, index) {}

        public VersionModule(string name) : this(name, 0L) {}

        public string IndexFormat()
            => string.IsNullOrEmpty(format) ? index.ToString() : string.Format(format, index);
    }
}