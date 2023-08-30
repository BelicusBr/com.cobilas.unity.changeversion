using System;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    [Serializable]
    public abstract class ChangeVersionModuleBase {
        public abstract string Name { get; set; }
        public abstract string ChangeFormat { get; set; }
        public abstract ulong Index { get; set; }
        public abstract ChangeVersionOptionBase[] Options { get; set; }

        public abstract void Draw();
    }
}
