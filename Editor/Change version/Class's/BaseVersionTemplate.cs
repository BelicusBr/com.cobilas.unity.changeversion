using System;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public abstract class BaseVersionTemplate {
        public abstract KeyValuePair<string, VersionChangeItem>[] GetTemplates();

        public static BaseVersionTemplate[] GetAllTemplates() {
            BaseVersionTemplate[] res = new BaseVersionTemplate[0];
            Type[] types = UnityTypeUtility.GetAllTypes();

            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++)
                if (types[I].IsSubclassOf(typeof(BaseVersionTemplate)))
                    ArrayManipulation.Add((BaseVersionTemplate)Activator.CreateInstance(types[I]), ref res);

            return res;
        }
    }
}