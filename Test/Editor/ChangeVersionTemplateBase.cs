using System;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    [Serializable]
    public abstract class ChangeVersionTemplateBase {
        public abstract string Name { get; }
        public abstract ChangeVersionModuleBase Module { get; }

        public abstract ChangeVersionTemplateBase GetTeplate();
    }
}
