using System.IO;
using Cobilas.Collections;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public static class ParseOldChangeVersion {
        public static VersionChangeItem Parse(string VersionConfigPath) {
            if (!File.Exists(VersionConfigPath)) return (VersionChangeItem)null;

            VersionModule[] modules = (VersionModule[])null;
            VersionModule temp = default;

            using (StreamReader reader = new StreamReader(VersionConfigPath))
                while (!reader.EndOfStream) {
                    string[] parts = reader.ReadLine().Split(':');
                    switch (parts[0]) { 
                        case "-n":
                        temp = new VersionModule();
                        temp.name = parts[1];
                         break;
                        case "-v": temp.index = long.Parse(parts[1]); break;
                        case "-f": temp.format= parts[1]; break;
                        case "-b": temp.upBuild = bool.Parse(parts[1]); break;
                        case "-m": temp.upCompiler= bool.Parse(parts[1]); break;
                        case "-u":
                        temp.upClose = bool.Parse(parts[1]);
                        ArrayManipulation.Add(temp, ref modules);
                        break;
                    }
                }
            return new VersionChangeItem(modules);
        }
    }
}