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
using Object = UnityEngine.Object;

namespace PowerUtilities
{
    public class PowerPostKeyframeAnimGen
    {
        public const string
            POWER_POST_SAVE_PATH = "Assets/PowerPostAnimCodeGen/",
            URP_POST_SAVE_PATH = "Assets/URPPostAnimCodeGen/";

        //[MenuItem(nameof(PowerUtilities) + "/PowerPost/Gen KeyFrame Mono")]
        // look ProjectSettings/PostControlCodeGen
        //public static void GenCode(string codeTemplate, string componentTypeInfo)
        //{
        //    var settingTextAssets = GetPowerPostSettings();
        //    GenCode(POWER_POST_SAVE_PATH, settingTextAssets, codeTemplate, componentTypeInfo);
        //}


        /// <summary>
        /// generate post control save to outputFolderPath
        /// </summary>
        /// <param name="outputFolderPath"></param>
        public static void GenCode(string outputFolderPath, TextAsset[] settingTextAssets, string codeTemplate, string componentTypeInfo)
        {
            VolumeComponentTypeTools.SetupComponentTypeInfoDict(componentTypeInfo);

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

        /// <summary>
        /// gen struct data for VolumeControl(Timeline)
        /// </summary>
        /// <param name="outputFolderPath"></param>
        /// <param name="settingTextAssets"></param>
        /// <param name="structDataTemplate"></param>
        /// <param name="componentTypeInfo"></param>
        public static void GenCode_VolumeKeyFrame(string outputFolderPath, TextAsset[] settingTextAssets,
            string structDataTemplate,
            string componentTypeInfo,
            string volumeBehaviour_DataTemplate,
            string volumeControlMono_Template,
            string structDataFileName = "VolumeControl",
            string structDataFileNameSpaceAdd = "",
            string volumeControlBehaviour_VolumeDataFileName = "VolumeControlBehaviour_VolumeData",
            string volumeControlBehaviourClassName = "VolumeControlBehaviour",
            string volumeControlMonoFileName = "VolumeControlMono"

            )
        {
            VolumeComponentTypeTools.SetupComponentTypeInfoDict(componentTypeInfo);

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
                structDataCodeSB.Append(string.Format(structDataTemplate, className, fieldsSB, settersSB, gettersSB, structDataFileNameSpaceAdd));
                //break;
                fieldsSB.Clear();
                settersSB.Clear();
                gettersSB.Clear();

                classNameList.Add(className);
            }
            //--- output URPPostDataStructs
            //
            var path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/{structDataFileName}.cs";
            File.WriteAllText(path, structDataCodeSB.ToString());

            //--- output VolumeControLBehaviour_Data(partial)
            //
            GenVolumeDataUpdateCode(classNameList, fieldsSB, settersSB, gettersSB);

            var volumeDataUpdateCodeStr = string.Format(volumeBehaviour_DataTemplate, fieldsSB, settersSB, volumeControlBehaviourClassName);
            path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/{volumeControlBehaviour_VolumeDataFileName}.cs";
            File.WriteAllText(path, volumeDataUpdateCodeStr);

            //--- output VolumeControlMono.cs
            // 0: fields, 1: Update, 2 : className
            var volumeControlMonoCodeStr = string.Format(volumeControlMono_Template, fieldsSB, settersSB, volumeControlMonoFileName);
            path = $"{PathTools.GetAssetAbsPath(outputFolderPath)}/{volumeControlMonoFileName}.cs";
            File.WriteAllText(path, volumeControlMonoCodeStr);

            AssetDatabaseTools.SaveRefresh();

            //  ping output path
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(outputFolderPath));
        }

        public static void GenVolumeDataUpdateCode(List<string> classNameList, StringBuilder fieldSB, StringBuilder settersSB, StringBuilder gettersSB)
        {
            foreach (var className in classNameList)
            {
                var dataClassName = $"{className}_Data";
                if (fieldSB != null)
                    fieldSB.AppendLine($"public {dataClassName} _{dataClassName};");
                if (settersSB != null)
                {
                    //settersSB.AppendLine($"if(_{dataClassName}.isEnable) {{");
                    settersSB.AppendLine($"  VolumeDataTools.Update(clipVolume, _{dataClassName});");
                    //settersSB.AppendLine("}}");
                }
            }
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

                if (fieldsSB != null)
                    fieldsSB.AppendLine($"public {VolumeComponentTypeTools.ConvertVolumeTypeName(varType)} {varName};");
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

        public static TextAsset[] GetPowerPostSettings()
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