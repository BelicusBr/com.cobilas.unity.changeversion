using System;
using UnityEngine;
using UnityEditor;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public class VersionModule : IDisposable {
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
                Name = EditorGUILayout.TextField("Name", Name);
                Change_Format = EditorGUILayout.TextField("Format", Change_Format);
                if (!valid_format)
                    EditorGUILayout.HelpBox("Invalid format", MessageType.Error);
                Index = EditorGUILayout.LongField("Index", Index);
                EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
                ++EditorGUI.indentLevel;
                for (int I = 0; I < ArrayManipulation.ArrayLength(Options); I++)
                    Options[I].OptionDraw();
                --EditorGUI.indentLevel;
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
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
