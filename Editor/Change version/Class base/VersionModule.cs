using System;
using UnityEngine;
using UnityEditor;
using System.Text;
using Cobilas.Collections;
using Cobilas.Unity.Editor.ChangeVersion.Option;

namespace Cobilas.Unity.Editor.ChangeVersion {
    [Serializable]
    public sealed class VersionModule : IDisposable, ICloneable {
        public string Name;
        public long Index;
        public string Change_Format;
        [SerializeReference]
        public VersionOptionBase[] Options;

        public event Action ChangeName;
        public event Action ChangeIndex;
        public event Action ChangeModuleFormat;
        public event Action<StringBuilder> Pre_Print;
        public event Action<StringBuilder> Pos_Print;

        private bool foldout;

        public VersionModule(string name, long index, string format, params VersionOptionBase[] options) {
            Name = name;
            Index = index;
            Change_Format = format;
            Options = options;
            InitEvents();
        }

        public VersionModule(string name, string format, params VersionOptionBase[] options)
            : this(name, 0L, format, options) { }

        public VersionModule(string name, params VersionOptionBase[] options)
            : this(name, "{0}", options) { }

        public VersionModule(params VersionOptionBase[] options) : this("None", 0L, "{0}", options) { }

        public VersionModule() : this((VersionOptionBase[])null) { }

        public void DrawModule() {
            string txt_format;
            bool valid_format = true;
            try {
                txt_format = string.Format(Change_Format, Index);
            } catch {
                txt_format = Index.ToString();
                valid_format = false;
            }
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            foldout = EditorGUILayout.Foldout(foldout, string.Format("Name:{0} [{1}]", Name, txt_format));
            ++EditorGUI.indentLevel;
            if (foldout) {
                EditorGUI.BeginChangeCheck();
                Name = EditorGUILayout.TextField("Name", Name);
                if (EditorGUI.EndChangeCheck())
                    ChangeName?.Invoke();

                EditorGUI.BeginChangeCheck();
                Change_Format = EditorGUILayout.TextField("Format", Change_Format);
                if (EditorGUI.EndChangeCheck())
                    ChangeModuleFormat?.Invoke();
                if (!valid_format)
                    EditorGUILayout.HelpBox("Invalid format", MessageType.Error);

                EditorGUI.BeginChangeCheck();
                Index = EditorGUILayout.LongField("Index", Index);
                if (EditorGUI.EndChangeCheck())
                    ChangeIndex?.Invoke();
                EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
                ++EditorGUI.indentLevel;
                for (int I = 0; I < ArrayManipulation.ArrayLength(Options); I++)
                    Options[I].OptionDraw();
                --EditorGUI.indentLevel;
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }

        public override int GetHashCode() {
            int res = (string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode()) >>
            (string.IsNullOrEmpty(Change_Format) ? 0 : Change_Format.GetHashCode()) ^
            Index.GetHashCode();
            for (int I = 0, C = 0; I < ArrayManipulation.ArrayLength(Options); I++) {
                switch (C) {
                    case 0:
                        res <<= Options[I].GetHashCode();
                        break;
                    case 2:
                        res ^= Options[I].GetHashCode();
                        break;
                    case 3:
                        res >>= Options[I].GetHashCode();
                        break;
                }
                ++C;
                C = C == 3 ? 0 : C;
            }
            return res;
        }

        public void InitEvents() {
            for (int I = 0; I < ArrayManipulation.ArrayLength(Options); I++)
                Options[I].SetEvent(this);
        }

        public void Dispose() {
            Name = Change_Format = string.Empty;
            ChangeIndex =
                ChangeModuleFormat =
                ChangeName = (Action)null;
            Pre_Print =
                Pos_Print = (Action<StringBuilder>)null;
            Index = 0L;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Options); I++)
                Options[I].Dispose();

            ArrayManipulation.ClearArraySafe(ref Options);
        }

        public object Clone() {
            string c_Name = string.IsNullOrEmpty(Name) ? string.Empty : (string)Name.Clone();
            long c_Index = Index;
            string c_Change_Format = string.IsNullOrEmpty(Change_Format) ? string.Empty : (string)Change_Format.Clone();
            VersionOptionBase[] c_Options = new VersionOptionBase[ArrayManipulation.ArrayLength(Options)];
            for (int I = 0; I < Options.Length; I++)
                c_Options[I] = (VersionOptionBase)Options[I].Clone();
            return new VersionModule(c_Name, c_Index, c_Change_Format, c_Options);
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            Pre_Print?.Invoke(builder);
            try {
                builder.AppendFormat(Change_Format, Index);
            } catch {
                Debug.LogError($"module format in '{Name}' is invalid");
                builder.Append(Index.ToString());
            }
            Pos_Print?.Invoke(builder);
            return builder.ToString();
        }
    }
}
