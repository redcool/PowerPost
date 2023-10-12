#if UNITY_EDITOR
namespace PowerUtilities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using static UnityEditor.PlayerSettings;

    [CustomEditor(typeof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettingsEditor : PowerEditor<PostControlCodeGenSettings>
    {
        string urpFolderPath = @"Packages/com.unity.render-pipelines.universal/Runtime/Overrides";

        public override void DrawInspectorUI(PostControlCodeGenSettings inst)
        {

            GUILayout.BeginVertical("PowerPost");
            if (GUILayout.Button("Gen PowerPost Control code"))
            {
                PowerPostKeyframeAnimGen.GenCode();
            }

            // line
            var pos = EditorGUILayout.GetControlRect(GUILayout.Height(2));
            EditorGUITools.DrawColorLine(pos);

            urpFolderPath = EditorGUILayout.TextArea(urpFolderPath);

            if (GUILayout.Button("Gen URP Post Control code"))
            {
                if (!AssetDatabase.IsValidFolder(urpFolderPath))
                    return;

                var monos = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });

                PowerPostKeyframeAnimGen.GenCode(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monos);
            }
            GUILayout.EndVertical();
        }
    }

    [ProjectSettingGroup(ProjectSettingGroupAttribute.POWER_UTILS+"/PostControlCodeGen")]
    [SOAssetPath(nameof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettings : ScriptableObject
    {

    }
}
#endif