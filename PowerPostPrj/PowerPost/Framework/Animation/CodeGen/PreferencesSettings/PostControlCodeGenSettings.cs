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
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeStructData_Template)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeBehaviour_DataTemplate)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeCompParamTypeAsset)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PostControlCodeGenSettings.volumeControlMono_Template)));

            DrawPowerPostCodeGen(inst);

            // line
            EditorGUILayout.Space(20);
            var pos = EditorGUILayout.GetControlRect(GUILayout.Height(2));
            EditorGUITools.DrawColorLine(pos);

            DrawURPPostGen(inst);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawPowerPostCodeGen(PostControlCodeGenSettings inst)
        {
            EditorGUITools.DrawTitleLabel(GUIContentEx.TempContent("PowerPost"));
            EditorGUITools.BeginHorizontalBox(() =>
            {
                /** use struct data more convenient*/
                //if (GUILayout.Button(GUIContentEx.TempContent("Gen PowerPost Control code", "generate power post control codes")))
                //{
                //    var settingTextAssets = PowerPostKeyframeAnimGen.GetPowerPostSettings();
                //    PowerPostKeyframeAnimGen.GenCode(PowerPostKeyframeAnimGen.POWER_POST_SAVE_PATH, settingTextAssets, inst.codeTemplateAsset.text, inst.volumeCompParamTypeAsset.text);
                //}
                if (GUILayout.Button(GUIContentEx.TempContent("Gen PowerPost Volume Control(Mono & Timeline)", "generate code for PowerPost volumeControl(MonoBehavriour and Timeline)")))
                {
                    var monoTextAssets = PowerPostKeyframeAnimGen.GetPowerPostSettings();
                    PowerPostKeyframeAnimGen.GenCode_VolumeKeyFrame(PowerPostKeyframeAnimGen.POWER_POST_SAVE_PATH, monoTextAssets,
                    inst.volumeStructData_Template.text,
                    inst.volumeCompParamTypeAsset.text,
                    inst.volumeBehaviour_DataTemplate.text,
                    inst.volumeControlMono_Template.text,
                    "PowerPostVolumeControl", "using PowerPost;",
                    "PowerPostVolumeControlBehaviour_VolumeData", "PowerPostVolumeControlBehaviour",
                    "PowerPostVolumeControlMono"
                    );
                }
            });
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
                    /** use struct data more convenient*/
                    //if (GUILayout.Button(GUIContentEx.TempContent("Gen URP VolumeControl(MonoBehaviour)", "generate urp post control codes")))
                    //{
                    //    if (!AssetDatabase.IsValidFolder(urpFolderPath))
                    //        return;

                    //    var monos = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });
                    //    PowerPostKeyframeAnimGen.GenCode(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monos, inst.codeTemplateAsset.text, inst.volumeCompParamTypeAsset.text);
                    //}

                    if (GUILayout.Button(GUIContentEx.TempContent("Gen URP Volume Control(Mono & Timeline)", "generate code for urp volumeControl(MonoBehavriour and Timeline)")))
                    {
                        if (!AssetDatabase.IsValidFolder(urpFolderPath))
                            return;

                        var monoTextAssets = AssetDatabaseTools.FindAssetsPathAndLoad<TextAsset>(out _, "", ".cs", searchInFolders: new[] { urpFolderPath });
                        PowerPostKeyframeAnimGen.GenCode_VolumeKeyFrame(PowerPostKeyframeAnimGen.URP_POST_SAVE_PATH, monoTextAssets,
                            inst.volumeStructData_Template.text,
                            inst.volumeCompParamTypeAsset.text,
                            inst.volumeBehaviour_DataTemplate.text,
                            inst.volumeControlMono_Template.text
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

        [LoadAsset("VolumeStructDataTemplate.txt")]
        [Tooltip("generate volume data code from template file")]
        public TextAsset volumeStructData_Template;

        [LoadAsset("PowerPostVolumeBehaviour_DataTemplate.txt")]
        [Tooltip("generate VolumeBehaviour update code from template file,timeline use")]
        public TextAsset volumeBehaviour_DataTemplate;

        [LoadAsset("VolumeComponentParamaterTypes.txt")]
        [Tooltip("volument component parameter type mappping file")]
        public TextAsset volumeCompParamTypeAsset;
        
        [LoadAsset("VolumeControlMono_Template")]
        [Tooltip("generate update code from template file,mono use")]
        public TextAsset volumeControlMono_Template;

    }
}
#endif