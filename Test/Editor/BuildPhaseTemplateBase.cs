using System;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    [Serializable]
    public abstract class BuildPhaseTemplateBase {
        public abstract string Name { get; }
        public abstract string[] Stages { get; }
    }
}
