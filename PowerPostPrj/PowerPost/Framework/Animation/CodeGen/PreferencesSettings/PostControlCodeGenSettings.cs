#if UNITY_EDITOR
namespace PowerUtilities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettingsEditor : PowerEditor<PostControlCodeGenSettings>
    {
        string urpFolderPath = @"Packages/com.unity.render-pipelines.universal/Runtime/Overrides";
        Object folderObj;

        const string helpBox =
            @"Get post key frame codes,
    when urp add new xxxParameter, need change [PowerPostKeyframeAnimGen/ConvertVolumeTypeName] conressponding type string
";

        public override void DrawInspectorUI(PostControlCodeGenSettings inst)
        {
            serializedObject.Update();

            EditorGUITools.DrawScriptScope(serializedObject);
            EditorGUILayout.HelpBox(helpBox, MessageType.Info);

            GUILayout.BeginVertical();
            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("Code Template :"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.codeTemplateAsset)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeCodeTemplateAsset)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeBehaviour_DataTemplate)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeCompParamTypeAsset)));

            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("PowerPost"));
            EditorGUITools.BeginHorizontalBox(() =>
            {
                if (GUILayout.Button(GUIContentEx.TempContent("Gen PowerPost Control code", "generate power post control codes")))
                {
                    PowerPostKeyframeAnimGen.GenCode(inst.codeTemplateAsset.text, inst.volumeCompParamTypeAsset.text);
                }
                if(GUILayout.Button(GUIContentEx.TempContent("Gen Struct Code","struct code for Timeline(VolueControl)")))
                {

                }
            });

            // line
            EditorGUILayout.Space(20);
            var pos = EditorGUILayout.GetControlRect(GUILayout.Height(2));
            EditorGUITools.DrawColorLine(pos);

            DrawURPPostGen(inst);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawURPPostGen(PostControlCodeGenSettings inst)
        {
            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("URP post"));

            EditorGUI.indentLevel++;
            {
                EditorGUITools.BeginHorizontalBox(() =>
                {
                    EditorGUILayout.LabelField("folder path:", EditorStylesEx.BoldLabel);
                    urpFolderPath = EditorGUILayout.TextArea(urpFolderPath, GUILayout.Height(36));

                });

                EditorGUITools.BeginHorizontalBox(() =>
                {
                    if (GUILayout.Button(GUIContentEx.TempContent("Gen URP Post Control code", "generate urp post control codes")))
                    {
                        if (!AssetDatabase.IsValidFolder(urpFolderPath))
                            return;

                        var monos = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });
                        PowerPostKeyframeAnimGen.GenCode(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monos, inst.codeTemplateAsset.text,inst.volumeCompParamTypeAsset.text);
                    }

                    if (GUILayout.Button(GUIContentEx.TempContent("Gen URP Volume Control", "generate code for urp volumeControl(Timeline)")))
                    {
                        if (!AssetDatabase.IsValidFolder(urpFolderPath))
                            return;

                        var monos = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });
                        PowerPostKeyframeAnimGen.GenCode_VolumeKeyFrame(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monos,
                            inst.volumeCodeTemplateAsset.text,
                            inst.volumeCompParamTypeAsset.text,
                            inst.volumeBehaviour_DataTemplate.text
                            );
                    }
                });
            }
            EditorGUI.indentLevel--;
        }

    }


    [ProjectSettingGroup(ProjectSettingGroupAttribute.POWER_UTILS+"/PostControlCodeGen")]
    [SOAssetPath(nameof(PostControlCodeGenSettings))]
    public class PostControlCodeGenSettings : ScriptableObject
    {

        /**
        0 : className
        1 : fields
        2 : setters
        3 : getters
        */
        [LoadAsset("PowerPostCodeTemplate.txt")]
        [Tooltip("generate code from template file")]
        public TextAsset codeTemplateAsset;

        [LoadAsset("PowerPostVolumeCodeTemplate.txt")]
        [Tooltip("generate volume data code from template file")]
        public TextAsset volumeCodeTemplateAsset;

        [LoadAsset("PowerPostVolumeBehavriour_DataTemplate")]
        [Tooltip("generate VolumeBehavriour update code from template file")]
        public TextAsset volumeBehaviour_DataTemplate;

        [LoadAsset("VolumeComponentParamaterTypes.txt")]
        [Tooltip("volument component parameter type mappping file")]
        public TextAsset volumeCompParamTypeAsset;
        
    }
}
#endif