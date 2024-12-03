#if UNITY_EDITOR
using System;
using System.Collections.Generic;
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

        // save precision types
        static Dictionary<string, string> precisionTypeDict = new();
        // save string match types
        static Dictionary<string, string> matchTypeTypeDict = new();


        //[MenuItem(nameof(PowerUtilities) + "/PowerPost/Gen KeyFrame Mono")]
        // look ProjectSettings/PostControlCodeGen
        public static void GenCode(string codeTemplate, string componentTypeInfo)
        {
            var settingTextAssets = GetPowerPostSettings();
            GenCode(POWER_POST_SAVE_PATH, settingTextAssets, codeTemplate, componentTypeInfo);
        }


        /// <summary>
        /// generate post control save to outputFolderPath
        /// </summary>
        /// <param name="outputFolderPath"></param>
        public static void GenCode(string outputFolderPath, TextAsset[] settingTextAssets,string codeTemplate,string componentTypeInfo)
        {
            SetupComponentTypeInfoDict(componentTypeInfo);

            var fieldsSB = new StringBuilder();
            var settersSB = new StringBuilder();
            var gettersSB = new StringBuilder();

            PathTools.CreateAbsFolderPath(outputFolderPath);

            foreach (var textAsset in settingTextAssets)
            {
                AnalystCodeString(textAsset.text, fieldsSB, settersSB, gettersSB);

                var className = textAsset.name;
                var path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/{className}_Control.cs";
                File.WriteAllText(path, string.Format(codeTemplate, className, fieldsSB, settersSB, gettersSB));

                //break;
                fieldsSB.Clear();
                settersSB.Clear();
                gettersSB.Clear();
            }
            AssetDatabaseTools.SaveRefresh();
        }

        private static void SetupComponentTypeInfoDict(string componentTypeInfo)
        {
            precisionTypeDict.Clear();
            matchTypeTypeDict.Clear();

            componentTypeInfo.ReadKeyValue(onReadLineKeyValue: (kv) =>
            {
                if (kv.Length <= 1)
                    return;

                var k = kv[0];
                var v = kv[1];
                var isPrecisionType = (v.Contains(",p"));
                if (isPrecisionType)
                {
                    v = v.SplitBy()[0];
                    precisionTypeDict[k] = v;
                }
                else
                {
                    matchTypeTypeDict[k] = v;
                }
            });
        }
        /// <summary>
        /// gen struct data for VolumeControl(Timeline)
        /// </summary>
        /// <param name="outputFolderPath"></param>
        /// <param name="settingTextAssets"></param>
        /// <param name="codeTemplate"></param>
        /// <param name="componentTypeInfo"></param>
        public static void GenCode_VolumeKeyFrame(string outputFolderPath, TextAsset[] settingTextAssets, string codeTemplate, string componentTypeInfo,string volumeBehaviour_DataTemplate)
        {
            SetupComponentTypeInfoDict(componentTypeInfo);

            var fieldsSB = new StringBuilder();
            var settersSB = new StringBuilder();
            var gettersSB = new StringBuilder();

            PathTools.CreateAbsFolderPath(outputFolderPath);

            var classNameList = new List<string>();

            var structDataCodeSB = new StringBuilder();
            foreach (var textAsset in settingTextAssets)
            {
                AnalystCodeString(textAsset.text, fieldsSB, settersSB, gettersSB);

                var className = textAsset.name;
                structDataCodeSB.Append(string.Format(codeTemplate, className, fieldsSB, settersSB,gettersSB));
                //break;
                fieldsSB.Clear();
                settersSB.Clear();
                gettersSB.Clear();

                classNameList.Add(className);
            }
            //--- output URPPostDataStructs
            //
            var path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/VolumeControl.cs";
            File.WriteAllText(path, structDataCodeSB.ToString());

            //--- output VolumeControLBehaviour_Data(partial)
            //
            GenVolumeDataUpdateCode(classNameList, fieldsSB, settersSB,gettersSB);

            var volumeDataUpdateCodeStr = string.Format(volumeBehaviour_DataTemplate, fieldsSB, settersSB);
            path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/VolumeControlBehaviour_VolumeData.cs";
            File.WriteAllText(path, volumeDataUpdateCodeStr);

            AssetDatabaseTools.SaveRefresh();
        }

        public static void GenVolumeDataUpdateCode(List<string> classNameList,StringBuilder fieldSB, StringBuilder settersSB, StringBuilder gettersSB)
        {
            foreach (var className in classNameList)
            {
                var dataClassName = $"{className}_Data";
                if (fieldSB != null)
                    fieldSB.AppendLine($"public {dataClassName} _{dataClassName};");
                if (settersSB != null)
                {
                    settersSB.AppendLine($"if(_{dataClassName}.isEnable) \n" +
                    $"  VolumeDataTools.Update(clipVolume, _{dataClassName});");
                }
            }
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
            // check precision dict
            if (precisionTypeDict.TryGetValue(typeName, out var typeStr))
                return typeStr;
            //check match mapping dict
            if(matchTypeTypeDict.TryFindByKey(k => typeName.Contains(k),out typeStr))
                return typeStr;
            return typeName;
            /*
            // precision
            if (typeName == "NoInterpTexture") return "Texture";
            if (typeName == "TextureCurve") return "TextureCurve";

            // match 
            if (typeName.Contains("Float")) return "float";
            if (typeName.Contains("Int")) return "int";
            if (typeName.Contains("Bool")) return "bool";
            if (typeName.Contains("LayerMask")) return "LayerMask";
            if (typeName.Contains("Color")) return "Color";
            if (typeName.Contains("Object")) return "Object";

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

            if (typeName.Contains("Downscale")) return "BloomDownscaleMode";
            return typeName;
            */
        }


        public static void AnalystCodeString(string codeText, StringBuilder fieldsSB, StringBuilder setterSB, StringBuilder getterSB)
        {
            //public ClampedFloatParameter glitchHorizontalIntensity = new ClampedFloatParameter(0,0,1);
            // public DownscaleParameter downscale = new DownscaleParameter(BloomDownscaleMode.Half);
            const string linePattern = @"(//\s*\w+\s*)*(\w+)Parameter (\w+) *= ";
            var items = Regex.Matches(codeText, linePattern);
            foreach (Match match in items)
            {
                // comments line
                if (match.Groups[0].Value.Contains("//"))
                    continue;

                var varType = match.Groups[2].Value;
                var varName = match.Groups[3].Value;

                if(fieldsSB != null)
                    fieldsSB.AppendLine($"public {ConvertVolumeTypeName(varType)} {varName};");
                // setters
                //settings.baseLineMapIntensity.value = baseLineMapIntensity;
                if (setterSB != null)
                {
                    //settings.threshold.overrideState = threshold > 0;
                    setterSB.AppendLine($"settings.{varName}.overrideState = settings.{varName}.value != default;");
                    setterSB.AppendLine($"settings.{varName}.value = {varName};");
                }
                // getters
                //rotateRate = settings.rotateRate.value;
                if (getterSB != null)
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

    }
}
#endif