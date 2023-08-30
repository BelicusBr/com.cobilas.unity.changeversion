using System;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    [Serializable]
    public abstract class ChangeVersionOptionBase {
        public abstract ChangeVersionModuleBase module { get; set; }

        public abstract void Draw();
    }
}
