#if UNITY_EDITOR
namespace PowerUtilities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.Graphs;
    using UnityEngine;
    using static UnityEditor.PlayerSettings;

    [CustomEditor(typeof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettingsEditor : PowerEditor<PostControlCodeGenSettings>
    {
        string urpFolderPath = @"Packages/com.unity.render-pipelines.universal/Runtime/Overrides";
        Object folderObj;

        public override void DrawInspectorUI(PostControlCodeGenSettings inst)
        {

            GUILayout.BeginVertical();
            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("PowerPost"));
            if (GUILayout.Button(GUIContentEx.TempContent("Gen PowerPost Control code", "generate power post control codes")))
            {
                PowerPostKeyframeAnimGen.GenCode();
            }

            // line
            EditorGUILayout.Space(20);
            var pos = EditorGUILayout.GetControlRect(GUILayout.Height(2));
            EditorGUITools.DrawColorLine(pos);

            DrawURPPostGen();
            GUILayout.EndVertical();
        }

        private void DrawURPPostGen()
        {
            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("URP post"));

            EditorGUI.indentLevel++;

            //GUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("folder:", EditorStylesEx.BoldLabel);
            //folderObj = EditorGUILayout.ObjectField(folderObj,typeof(Object), false);
            //GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("folder path:", EditorStylesEx.BoldLabel);
            urpFolderPath = EditorGUILayout.TextArea(urpFolderPath,GUILayout.Height(36));
            GUILayout.EndHorizontal();

            if (GUILayout.Button(GUIContentEx.TempContent("Gen URP Post Control code", "generate urp post control codes")))
            {
                if (!AssetDatabase.IsValidFolder(urpFolderPath))
                    return;

                var monos = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });

                PowerPostKeyframeAnimGen.GenCode(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monos);
            }
            EditorGUI.indentLevel--;
        }

    }


    [ProjectSettingGroup(ProjectSettingGroupAttribute.POWER_UTILS+"/PostControlCodeGen")]
    [SOAssetPath(nameof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettings : ScriptableObject
    {

    }
}
#endif