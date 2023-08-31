using System;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public abstract class VersionOptionBase : IDisposable {
        public abstract void OptionDraw();
        public abstract void SetEvent(VersionModule module);
        public abstract void Dispose();
    }
}
