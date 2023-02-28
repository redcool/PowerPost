#if UNITY_EDITOR
using PowerPost;
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
        const string PATH = "Assets/PowerPostAnimCodeGen/";

        [MenuItem(nameof(PowerUtilities) + "/PowerPost/Gen KeyFrame Mono")]
        public static void GenCode()
        {
            var fieldsSB = new StringBuilder();
            var settersSB = new StringBuilder();
            var gettersSB = new StringBuilder();

            PathTools.CreateAbsFolderPath(PATH);
            var settingTextAssets = GetPowerPostSettings();
            foreach (var textAsset in settingTextAssets)
            {
                AnalystCodeString(textAsset, fieldsSB, settersSB, gettersSB);

                var className = textAsset.name;
                var path = $"{PathTools.GetAssetAbsPath(PATH)}/{className}Control.cs";
                File.WriteAllText(path, string.Format(CODE_TEMPLATE, className, fieldsSB, settersSB,gettersSB));

                //break;
                fieldsSB.Clear();
                settersSB.Clear();
                gettersSB.Clear();
            }

        }

        private static void OutputVolumeParameters()
        {
            var paramList = TypeCache.GetTypesDerivedFrom<VolumeParameter>();
            var sb = new StringBuilder();
            paramList.ForEach(p => sb.AppendLine(p.Name));

            Debug.Log(sb);
        }

        static string ConvertVolumeParameter(string typeName)
        {
            if (typeName.Contains("Float")) return "float";
            if (typeName.Contains("Int")) return "int";
            if (typeName.Contains("Bool")) return "bool";
            if (typeName.Contains("LayerMask")) return "LayerMask";
            if (typeName.Contains("Color")) return "Color";
            if (typeName.Contains("Object")) return "object";

            if (typeName.Contains("Texture")) return "Texture";
            if (typeName.Contains("Texture2D")) return "Texture2D";
            if (typeName.Contains("Texture3D")) return "Texture3D";
            if (typeName.Contains("Cubemap")) return "Cubemap";
            if (typeName.Contains("RenderTexture")) return "RenderTexture";

            if (typeName.Contains("Vector2")) return "Vector2";
            if (typeName.Contains("Vector3")) return "Vector3";
            if (typeName.Contains("Vector4")) return "Vector4";

            if (typeName.Contains("AnimationCurve")) return "AnimationCurve";

            if (typeName.Contains("ToneMappingMode")) return "ToneMappingSettings.Mode";
            throw new ArgumentException(typeName);
        }

        static void AnalystCodeString1(Type type, StringBuilder fieldsSB, StringBuilder setterSB, StringBuilder getterSB)
        {

            //var inst = Activator.CreateInstance(type);
            var inst = ScriptableObject.CreateInstance(type);

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.FieldType.IsSubclassOf(typeof(VolumeParameter)))
                .OrderBy(t => t.MetadataToken);
            
            fields.ForEach(f =>
            {
                var defaultValue = f.GetValue(inst);
                var fieldValue = defaultValue.GetType().GetProperty("value").GetValue(defaultValue);
                var fieldString = $"public {ConvertVolumeParameter(f.FieldType.Name)} {f.Name};";
                fieldsSB.AppendLine(fieldString);
            });
        }
        
        static void AnalystCodeString(TextAsset textAsset, StringBuilder fieldsSB, StringBuilder setterSB, StringBuilder getterSB)
        {
            //public ClampedFloatParameter glitchHorizontalIntensity = new ClampedFloatParameter(0,0,1);
            const string linePattern = @"(//\s*\w+\s*)*(\w+)Parameter (\w+) ?= *\w+ *\w+\((\w+),?";
            var items = Regex.Matches(textAsset.text, linePattern);
            foreach (Match match in items)
            {
                // comments line
                if (match.Groups[0].Value.Contains("//"))
                    continue;

                var varType = match.Groups[2].Value;
                var varName = match.Groups[3].Value;
                var defaultValue = match.Groups[4].Value;
                fieldsSB.AppendLine($"public {ConvertVolumeParameter(varType)} {varName};");
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
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
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
    public class {0}Control : MonoBehaviour
    {{
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header(""Volume Parameters"")]
        public {0} settings;
        
        // variables
        {1}

        void Awake()
        {{
            RecordVars();
        }}

        // Start is called before the first frame update
        void OnEnable()
        {{
            if (!postVolume)
                postVolume = GetComponent<Volume>();

            if (postVolume && postVolume.profile)
            {{
                postVolume.profile.TryGet(out settings);
            }}

            intervalTime = 1f / updateCount;
            InvokeRepeating(nameof(UpdateVars), 0, intervalTime);
        }}
        private void OnDisable()
        {{
            if (IsInvoking(nameof(UpdateVars)))
                CancelInvoke(nameof(UpdateVars));
        }}
        void UpdateVars()
        {{
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            {2}
        }}

        public void RecordVars()
        {{
            if (!settings)
            {{
                postVolume = GetComponent<Volume>();
                if (postVolume && postVolume.profile)
                {{
                    postVolume.profile.TryGet(out settings);
                }}
            }}

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