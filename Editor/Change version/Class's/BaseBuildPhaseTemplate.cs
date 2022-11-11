using System;
using Cobilas.Collections;
using Cobilas.Unity.Utility;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public abstract class BaseBuildPhaseTemplate {
        public abstract string[] GetTemplates();

        public static BaseBuildPhaseTemplate[] GetAllTemplates() {
            BaseBuildPhaseTemplate[] res = new BaseBuildPhaseTemplate[0];
            Type[] types = UnityTypeUtility.GetAllTypes();

            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++)
                if (types[I].IsSubclassOf(typeof(BaseBuildPhaseTemplate)))
                    ArrayManipulation.Add((BaseBuildPhaseTemplate)Activator.CreateInstance(types[I]), ref res);

            return res;
        }
    }
}