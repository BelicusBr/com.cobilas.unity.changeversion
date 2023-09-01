using System;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public abstract class VersionOptionBase : IDisposable, ICloneable {
        public abstract void Dispose();
        public abstract object Clone();
        public abstract void OptionDraw();
        public abstract void SetEvent(VersionModule module);
        public new abstract int GetHashCode();
    }
}
