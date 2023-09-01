using System;
using UnityEngine;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    /// <summary>{Major}.{Minor}.{Revision}</summary>
    [Serializable]
    public sealed class MMR_Template : VersionTemplateTarget {
        [SerializeField] private string name;
        [SerializeField] private VersionModule[] modules;

        public override string Name => name;
        public override VersionModule[] Modules => modules;
        public override int ModuleCount => ArrayManipulation.ArrayLength(modules);

        public override VersionModule this[int index] => modules[index];

        public MMR_Template(string name, VersionModule[] modules) {
            this.name = name;
            this.modules = modules;
        }

        public MMR_Template() : this (
            "MMR",
            new VersionModule[] {
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
                new VersionModule("Revision",
                    new UpdateRevisionOption(),
                    new PreProductionCharacterOption("-{0}")
                )
            }
        ) { }

        public override void Set(string name, VersionModule[] modules) {
            this.name = name;
            this.modules = modules;
        }

        public override int GetHashCode() {
            int res = string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode();
            for (int I = 0, C = 0; I < ModuleCount; I++) {
                switch (C) {
                    case 0:
                        res >>= modules[I].GetHashCode();
                        break;
                    case 2:
                        res ^= modules[I].GetHashCode();
                        break;
                    case 3:
                        res <<= modules[I].GetHashCode();
                        break;
                }
                ++C;
                C = C == 3 ? 0 : C;
            }
            return res;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < ArrayManipulation.ArrayLength(modules); I++)
                builder.Append(modules[I]);
            return builder.ToString();
        }
    }
}
