using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;

namespace Cobilas.Unity.Editor.ChangeVersion {
    public class ChangeVersionWin : EditorWindow {

        private VersionChangeItem versionChange;
        private string[] buildPhases;
        private string[] templatesName;
        private VersionChangeItem[] templatesValue;
        private Vector2 scrollView;

        private static string ChangeVersionFolder => UnityPath.Combine(UnityPath.ProjectFolderPath, "ChangeVersion");
        private static string oldChangeVersionConfigFile => UnityPath.Combine(ChangeVersionFolder, "VersionConfig.txt");
        private static string ChangeVersionConfigFile => UnityPath.Combine(ChangeVersionFolder, "VersionConfig.json");
        private static string oldChangeConfigFile => UnityPath.Combine(ChangeVersionFolder, "Config.txt");

        [MenuItem("Window/Change version")]
        private static void ShowWindow() {
            ChangeVersionWin window = GetWindow<ChangeVersionWin>();
            window.titleContent = new GUIContent("Change Version");
            window.minSize = new Vector2(570f, 420f);
            window.Show();
        }

        [InitializeOnLoadMethod]
        private static void FuncSencond() {
            VersionChangeItem v_temp;
            VersionModule[] m_temp;
            CompilationPipeline.assemblyCompilationFinished += (s, l) => {
                if (!File.Exists(ChangeVersionConfigFile)) return;
                for (int I = 0; I < ArrayManipulation.ArrayLength(l); I++)
                    if (l[I].type == CompilerMessageType.Error)
                        return;
                v_temp = VersionChangeItem.Load(ChangeVersionConfigFile);
                m_temp = v_temp.modules;
                for (int I = 0; I < ArrayManipulation.ArrayLength(m_temp); I++)
                    if (m_temp[I].upCompiler)
                        ++m_temp[I].index;
                v_temp.modules = m_temp;
                PlayerSettings.bundleVersion = VersionChangeItem.VersionToString(v_temp);
                VersionChangeItem.Unload(ChangeVersionConfigFile, v_temp);
            };

            EditorApplication.quitting += () => {
                if (!File.Exists(ChangeVersionConfigFile)) return;
                v_temp = VersionChangeItem.Load(ChangeVersionConfigFile);
                m_temp = v_temp.modules;
                for (int I = 0; I < ArrayManipulation.ArrayLength(m_temp); I++)
                    if (m_temp[I].upClose)
                        ++m_temp[I].index;
                v_temp.modules = m_temp;
                PlayerSettings.bundleVersion = VersionChangeItem.VersionToString(v_temp);
                VersionChangeItem.Unload(ChangeVersionConfigFile, v_temp);
            };

            PostprocessBuild.EventOnPostprocessBuild += (br) => {
                if (br.summary.totalErrors != 0 || !File.Exists(ChangeVersionConfigFile)) return;
                v_temp = VersionChangeItem.Load(ChangeVersionConfigFile);
                m_temp = v_temp.modules;
                for (int I = 0; I < ArrayManipulation.ArrayLength(m_temp); I++)
                    if (m_temp[I].upBuild)
                        ++m_temp[I].index;
                v_temp.modules = m_temp;
                PlayerSettings.bundleVersion = VersionChangeItem.VersionToString(v_temp);
                VersionChangeItem.Unload(ChangeVersionConfigFile, v_temp);
            };
        }

        [MenuItem("Tools/ChangeVersion/Print version")]
        private static void PrintVersion()
            => Debug.Log(VersionToString());

        [MenuItem("Tools/ChangeVersion/Copy version")]
        private static void CopyVersion()
            => EditorGUIUtility.systemCopyBuffer = VersionToString();

        private void OnEnable() {
            ArrayManipulation.ClearArraySafe(ref buildPhases);
            ArrayManipulation.ClearArraySafe(ref templatesName);
            ArrayManipulation.ClearArraySafe(ref templatesValue);
            ArrayManipulation.Add("None", ref buildPhases);

            foreach (var item in BaseBuildPhaseTemplate.GetAllTemplates())
                ArrayManipulation.Add(item.GetTemplates(), ref buildPhases);

            foreach (var item in BaseVersionTemplate.GetAllTemplates())
                foreach (var item2 in item.GetTemplates()) {
                    ArrayManipulation.Add(item2.Key, ref templatesName);
                    ArrayManipulation.Add(item2.Value, ref templatesValue);
                }

            if (LoadOldChangeVersion()) return;

            if (!File.Exists(ChangeVersionConfigFile)) {
                versionChange = new VersionChangeItem(new VersionModule[0]);
                return;
            }

            versionChange = VersionChangeItem.Load(ChangeVersionConfigFile);
        }

        private void OnDestroy() {
            if (!Directory.Exists(ChangeVersionFolder))
                Directory.CreateDirectory(ChangeVersionFolder);

            if (versionChange == (VersionChangeItem)null) return;

            PlayerSettings.bundleVersion = VersionChangeItem.VersionToString(versionChange);

            VersionChangeItem.Unload(ChangeVersionConfigFile, versionChange);
        }

        private void OnGUI() {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
                if (ToolbarButton(LabelItens.bt_Add))
                    ArrayManipulation.Add(new VersionModule(), ref versionChange.modules);

                if (ToolbarButton(LabelItens.bt_Save))
                    OnDestroy();

                if (ToolbarButton(LabelItens.bt_Clear))
                    ArrayManipulation.ClearArraySafe(ref versionChange.modules);

                versionChange.buildPhase = versionChange.buildPhase > buildPhases.Length ? 0 : versionChange.buildPhase;
                versionChange.buildPhase = ToolbarPopup(versionChange.buildPhase, buildPhases, 130f);
                EditorGUI.BeginChangeCheck();
                int indexVersion = ToolbarPopup(0, templatesName, 130f);
                if (EditorGUI.EndChangeCheck())
                    versionChange.modules = templatesValue[indexVersion].modules;

            EditorGUILayout.EndHorizontal();
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
                VersionModule[] m_temp = versionChange.modules;
                for (int I = 0; I < ArrayManipulation.ArrayLength(m_temp); I++) {
                    bool iv_format = false;
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                            GUIContent guic_Temp;
                            try {
                                guic_Temp = EditorGUIUtility.TrTempContent(string.Format("{0} --Index:{1}", m_temp[I].name, m_temp[I].IndexFormat()));
                            } catch {
                                guic_Temp = EditorGUIUtility.TrTempContent(string.Format("{0} --Index:{1}", m_temp[I].name, m_temp[I].index));
                                iv_format = true;
                            }
                            m_temp[I].foldout = EditorGUILayout.Foldout(m_temp[I].foldout, guic_Temp);
                            if (ToolbarButton(LabelItens.bt_Remove)) {
                                ArrayManipulation.Remove(I, ref m_temp);
                                break;
                            }
                        EditorGUILayout.EndHorizontal();
                        if (m_temp[I].foldout) {
                            ++EditorGUI.indentLevel;
                            m_temp[I].format = EditorGUILayout.TextField(LabelItens.bt_ChangeFormat, m_temp[I].format);
                            if (iv_format)
                                EditorGUILayout.HelpBox("Invalid format", MessageType.Error);
                            m_temp[I].index = EditorGUILayout.LongField(LabelItens.bt_Index, m_temp[I].index);
                            m_temp[I].upBuild = EditorGUILayout.ToggleLeft(LabelItens.ckb_IsBuild, m_temp[I].upBuild);
                            m_temp[I].upClose = EditorGUILayout.ToggleLeft(LabelItens.ckb_updateWhenClose, m_temp[I].upClose);
                            m_temp[I].upCompiler = EditorGUILayout.ToggleLeft(LabelItens.ckb_MRACOC, m_temp[I].upCompiler);
                            --EditorGUI.indentLevel;
                        }
                    EditorGUILayout.EndVertical();
                }
                versionChange.modules = m_temp;
            EditorGUILayout.EndScrollView();
        }

        private int ToolbarPopup(int selectedIndex, string[] displayedOptions, float width)
            => EditorGUILayout.Popup(selectedIndex, displayedOptions, EditorStyles.toolbarPopup, GUILayout.Width(width));

        private bool ToolbarButton(GUIContent content, float width)
            => GUILayout.Button(content, EditorStyles.toolbarButton, GUILayout.Width(width));

        //Método para usar futuramente
        //private bool ToolbarButton(string text, float width)
        //    => ToolbarButton(EditorGUIUtility.TrTempContent(text), width);

        private bool ToolbarButton(GUIContent content)
            => ToolbarButton(content, EditorStyles.toolbarButton.CalcSize(content).x);

        //Método para usar futuramente
        //private bool ToolbarButton(string text)
        //    => ToolbarButton(EditorGUIUtility.TrTempContent(text));

        private bool LoadOldChangeVersion() {
            if (File.Exists(oldChangeConfigFile))
                File.Delete(oldChangeConfigFile);

            if (File.Exists(oldChangeVersionConfigFile)) {
                versionChange = ParseOldChangeVersion.Parse(oldChangeVersionConfigFile);
                File.Delete(oldChangeVersionConfigFile);
                return true;
            }
            return false;
        }

        private static string VersionToString()
            => VersionChangeItem.VersionToString(VersionChangeItem.Load(ChangeVersionConfigFile));

        private class PostprocessBuild : IPostprocessBuildWithReport {
            int IOrderedCallback.callbackOrder => 0;

            public static event Action<BuildReport> EventOnPostprocessBuild;

            void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
                => EventOnPostprocessBuild?.Invoke(report);
        }

        private static class LabelItens {
            public static GUIContent bt_Add = new GUIContent("Add");
            public static GUIContent bt_Save = new GUIContent("Save");
            public static GUIContent bt_Clear = new GUIContent("Clear");
            public static GUIContent bt_Remove = new GUIContent("Remove");
            public static GUIContent bt_Index = new GUIContent("Index");
            public static GUIContent bt_ChangeFormat = new GUIContent("Change format");
            public static GUIContent ckb_updateWhenClose = new GUIContent("Update When Close");
            public static GUIContent ckb_IsBuild = new GUIContent("Check to update with each build");
            public static GUIContent ckb_MRACOC = new GUIContent("Mark revision after completion of compilation");
        }
    }
}