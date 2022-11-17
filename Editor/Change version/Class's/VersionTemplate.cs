using System.Collections.Generic;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public sealed class VersionTemplate : BaseVersionTemplate {
        public override KeyValuePair<string, VersionChangeItem>[] GetTemplates()
            => new KeyValuePair<string, VersionChangeItem>[] {
                new KeyValuePair<string, VersionChangeItem>("Default-mmbr",
                    new VersionChangeItem(
                    new VersionModule("Major"), new VersionModule("Minor"),
                    new VersionModule("Build", true, false, false),
                    new VersionModule("Revision", false, true, false)
                    )
                ),
                new KeyValuePair<string, VersionChangeItem>("Default-mbr",
                    new VersionChangeItem(
                    new VersionModule("Major"), 
                    new VersionModule("Build", true, false, false),
                    new VersionModule("Revision", false, true, false)
                    )
                ),
                new KeyValuePair<string, VersionChangeItem>("Default-mr",
                    new VersionChangeItem(
                    new VersionModule("Major"),
                    new VersionModule("Revision", false, true, false)
                    )
                ),
            };
    }
}