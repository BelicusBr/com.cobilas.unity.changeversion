using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using Cobilas.Unity.Utility;
using System.Collections.Generic;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    public static class ChangeVersionResource {
        public static string ChangeVersionFolder => UnityPath.Combine(UnityPath.ProjectFolderPath, "ChangeVersion");
        public static string ChangeVersionFile => UnityPath.Combine(ChangeVersionFolder, "ChangeVersion.json");
        public static string FolderTemp => UnityPath.Combine(ChangeVersionFolder, "Temp");

        public static void UnloadChangeVersionFile(VersionTemplateTarget target) {
            if (!Directory.Exists(ChangeVersionFolder))
                Directory.CreateDirectory(ChangeVersionFolder);
            using (VersionSerialize serialize = new VersionSerialize(target)) {
                using (FileStream file = File.Open(ChangeVersionFile, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                    file.SetLength(0L);
                    file.Write(JsonUtility.ToJson(serialize, true), Encoding.UTF8);
                }
            }
        }

        public static VersionTemplateTarget LoadChangeVersionFile() {
            if (!File.Exists(ChangeVersionFile)) return null;
            using (FileStream file = File.Open(ChangeVersionFile, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                VersionSerialize serialize = JsonUtility.FromJson<VersionSerialize>(file.GetString(Encoding.UTF8));
                    return serialize.GetTemplate();
            }
        }
    }
}
