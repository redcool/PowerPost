﻿#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace PowerUtilities
{
    public class PowerPostKeyframeAnimGen
    {
        public const string
            POWER_POST_SAVE_PATH = "Assets/PowerPostAnimCodeGen/",
            URP_POST_SAVE_PATH = "Assets/URPPostAnimCodeGen/";


        [MenuItem(nameof(PowerUtilities) + "/PowerPost/Gen KeyFrame Mono")]
        public static void GenCode()
        {
            var settingTextAssets = GetPowerPostSettings();
            GenCode(POWER_POST_SAVE_PATH, settingTextAssets);
        }

        /// <summary>
        /// generate post control save to outputFolderPath
        /// </summary>
        /// <param name="outputFolderPath"></param>
        public static void GenCode(string outputFolderPath, TextAsset[] settingTextAssets)
        {
            var fieldsSB = new StringBuilder();
            var settersSB = new StringBuilder();
            var gettersSB = new StringBuilder();

            PathTools.CreateAbsFolderPath(outputFolderPath);

            foreach (var textAsset in settingTextAssets)
            {
                AnalystCodeString(textAsset.text, fieldsSB, settersSB, gettersSB);

                var className = textAsset.name;
                var path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/{className}Control.cs";
                File.WriteAllText(path, string.Format(CODE_TEMPLATE, className, fieldsSB, settersSB, gettersSB));

                //break;
                fieldsSB.Clear();
                settersSB.Clear();
                gettersSB.Clear();
            }
            AssetDatabaseTools.SaveRefresh();
        }

        private static void OutputVolumeParameters()
        {
            var paramList = TypeCache.GetTypesDerivedFrom<VolumeParameter>();
            var sb = new StringBuilder();
            paramList.ForEach(p => sb.AppendLine(p.Name));

            Debug.Log(sb);
        }

        public static string ConvertVolumeTypeName(string typeName)
        {
            // precision
            if (typeName == "NoInterpTexture") return "Texture";
            if (typeName == "TextureCurve") return "TextureCurve";

            // match 
            if (typeName.Contains("Float")) return "float";
            if (typeName.Contains("Int")) return "int";
            if (typeName.Contains("Bool")) return "bool";
            if (typeName.Contains("LayerMask")) return "LayerMask";
            if (typeName.Contains("Color")) return "Color";
            if (typeName.Contains("Object")) return "object";

            if (typeName.Contains("Texture2D")) return "Texture2D";
            if (typeName.Contains("Texture3D")) return "Texture3D";
            if (typeName.Contains("Cubemap")) return "Cubemap";
            if (typeName.Contains("RenderTexture")) return "RenderTexture";
            if (typeName.Contains("Texture")) return "Texture";

            if (typeName.Contains("Vector2")) return "Vector2";
            if (typeName.Contains("Vector3")) return "Vector3";
            if (typeName.Contains("Vector4")) return "Vector4";

            if (typeName.Contains("AnimationCurve")) return "AnimationCurve";

            if (typeName.Contains("ToneMappingMode")) return "ToneMappingSettings.Mode";
            return typeName;
        }


        public static void AnalystCodeString(string codeText, StringBuilder fieldsSB, StringBuilder setterSB, StringBuilder getterSB)
        {
            //public ClampedFloatParameter glitchHorizontalIntensity = new ClampedFloatParameter(0,0,1);
            const string linePattern = @"(//\s*\w+\s*)*(\w+)Parameter (\w+) *= ";
            var items = Regex.Matches(codeText, linePattern);
            foreach (Match match in items)
            {
                // comments line
                if (match.Groups[0].Value.Contains("//"))
                    continue;


                var varType = match.Groups[2].Value;
                var varName = match.Groups[3].Value;

                fieldsSB.AppendLine($"public {ConvertVolumeTypeName(varType)} {varName};");
                // setters
                //settings.baseLineMapIntensity.value = baseLineMapIntensity;
                setterSB.AppendLine($"settings.{varName}.value = {varName};");
                // getters
                //rotateRate = settings.rotateRate.value;
                getterSB.AppendLine($"{varName} = settings.{varName}.value;");
            }
        }

        private static TextAsset[] GetPowerPostSettings()
        {
            var items = AssetDatabaseTools.FindAssetsInProject<TextAsset>("PowerPost");
            items = items.Where(item => item.name == "PowerPost").ToArray();
            if (items.Length == 0)
                throw new Exception("PowerPost.asmdef canot found!");

            var path = AssetDatabase.GetAssetPath(items[0]);
            var asmdefDir = PathTools.GetAssetDir(path);

            items = AssetDatabaseTools.FindAssetsInProject<TextAsset>("*Settings", $"{asmdefDir}/PowerPost/Effects");
            return items;
        }
        /**
        0 : className
        1 : fields
        2 : setters
        3 : getters
         */
        const string CODE_TEMPLATE = @"namespace PowerUtilities
{{
    using PowerPost;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof({0}Control))]
    public class {0}ControlEditor : Editor
    {{
        public override void OnInspectorGUI()
        {{
            base.OnInspectorGUI();
            if (GUILayout.Button(""Record Vars""))
            {{
                var inst = target as {0}Control;
                inst.RecordVars();
            }}
        }}
    }}
#endif
    
    [ExecuteInEditMode]
    public class {0}Control : BaseSettingsControl<{0}>
    {{
        // variables
        {1}

        public override void UpdateVars()
        {{
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            {2}
        }}

        public override void RecordVars()
        {{
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            {3}
        }}
    }}
}}
";
    }
}
#endif