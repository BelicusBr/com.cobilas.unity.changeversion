using System;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Cobilas.Unity.Test.Editor.ChangeVersion {
    [Serializable]
    public sealed class PreProductionCharacterOption : VersionOptionBase {

        private static readonly string[] CharPreProduc = { 
            "None",
            "alpha",
            "beta",
            "rc",
            "preview"
        };

        private static readonly string[] SCharPreProduc = {
            "None",
            "a",
            "b",
            "rc",
            "p"
        };

        [SerializeField] private int Index;
        [SerializeField] private string format;
        [SerializeField] private bool isSingleChar;

        public PreProductionCharacterOption(string format, bool isSingleChar) {
            Index = 0;
            this.format = format;
            this.isSingleChar = isSingleChar;
        }

        public PreProductionCharacterOption(string format) : this(format, false) { }

        public PreProductionCharacterOption() : this("{0}") { }

        public override void OptionDraw() {
            isSingleChar = EditorGUILayout.ToggleLeft("Is single char", isSingleChar);
            Index = EditorGUILayout.Popup(EditorGUIUtility.TrTextContent("letter"),Index,
                isSingleChar ? SCharPreProduc : CharPreProduc);
            format = EditorGUILayout.TextField(EditorGUIUtility.TrTextContent("Format"), format);

            string txt_format;

            try {
                txt_format = string.Format(format, (isSingleChar ? SCharPreProduc : CharPreProduc)[Index]);
            } catch {
                EditorGUILayout.HelpBox("Invalid format", MessageType.Error);
                txt_format = (isSingleChar ? SCharPreProduc : CharPreProduc)[Index];
            }
            txt_format = Index == 0 ? string.Empty : txt_format;
            EditorGUILayout.LabelField($"Formatted value: {txt_format}", EditorStyles.boldLabel);
        }

        public override void SetEvent(VersionModule module) {
            module.Pos_Print += Module_Pos_Print;
        }

        private void Module_Pos_Print(StringBuilder obj) {
            string text = (isSingleChar ? SCharPreProduc : CharPreProduc)[Index];
            text = Index == 0 ? string.Empty : text;
            try {
                obj.AppendFormat(format, text);
            } catch {
                obj.Append(text);
            }
        }

        public override void Dispose() {
            Index = 0;
            format = string.Empty;
            isSingleChar = default;
        }
    }
}
