using System;
using System.IO;
using System.Text;
using UnityEngine;
using Cobilas.Collections;

namespace Cobilas.Unity.Editor.ChangeVersion {
    [Serializable]
    public sealed class VersionChangeItem {
        /// <summary>Indicates the build phase.(0 isNone | 1 isAlpha | 2 isBeta)</summary>
        public int buildPhase;
        public VersionModule[] modules;

        public VersionChangeItem(int buildPhase, params VersionModule[] modules) {
            this.buildPhase = buildPhase;
            this.modules = modules;
        }

        public VersionChangeItem(params VersionModule[] modules) :
            this(0, modules) {}

        public static VersionChangeItem Load(string path)
            => File.Exists(path) ? 
                JsonUtility.FromJson<VersionChangeItem>(Encoding.UTF8.GetString(File.ReadAllBytes(path))) : 
                (VersionChangeItem)null;

        public static void Unload(string path, VersionChangeItem version) {
            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Debug.LogError(string.Format("Directory '{0}' not found!", Path.GetDirectoryName(path)));
                return;
            }
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
                byte[] cont = Encoding.UTF8.GetBytes(JsonUtility.ToJson(version, true));
                stream.SetLength(cont.Length);
                stream.Write(cont, 0, cont.Length);
            }
        }

        public static string VersionToString(VersionChangeItem version) {
            StringBuilder builder = new StringBuilder();
            string[] p_builds = new string[1];
            p_builds[0] = string.Empty;

            foreach (var item in BaseBuildPhaseTemplate.GetAllTemplates())
                ArrayManipulation.Add(item.GetTemplates(), ref p_builds);
            
            for (int I = 0; I < ArrayManipulation.ArrayLength(version.modules); I++) {
                VersionModule mod = version.modules[I];
                string temp = mod.index.ToString();
                if (!string.IsNullOrEmpty(mod.format))
                    temp = string.Format(mod.format, temp);
                builder.AppendFormat("{0}.", temp);
            }
            
            return string.Format(version.buildPhase == 0 ? "{0}" : "{0}-{1}",
                builder.ToString().TrimEnd('.'),
                p_builds[version.buildPhase]
                );
        }
    }
}