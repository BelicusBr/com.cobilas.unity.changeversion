using System;
using UnityEngine;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    /// <summary>{Major}.{Minor}.{Build}.{Revision}</summary>
    [Serializable]
    public sealed class MMBR_Template : VersionTemplateTarget {
        [SerializeField] private string name;
        [SerializeField] private VersionModule[] modules;

        public override string Name => name;
        public override VersionModule[] Modules => modules;
        public override int ModuleCount => ArrayManipulation.ArrayLength(modules);

        public MMBR_Template() {
            name = "MMBR";
            modules = new VersionModule[] {
                new VersionModule("Major", "{0}.",
                    new UpdateClosedOption(),
                    new UpdateBuildOption(),
                    new UpdateRevisionOption()
                ),
                new VersionModule("Minor", "{0}.",
                    new UpdateClosedOption(),
                    new UpdateBuildOption(),
                    new UpdateRevisionOption()
                ),
                new VersionModule("Build", "{0}.",
                    new UpdateBuildOption()
                ),
                new VersionModule("Revision",
                    new UpdateRevisionOption(),
                    new PreProductionCharacterOption("-{0}")
                )
            };
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < ArrayManipulation.ArrayLength(modules); I++)
                builder.Append(modules[I]);
            return builder.ToString();
        }
    }
}
