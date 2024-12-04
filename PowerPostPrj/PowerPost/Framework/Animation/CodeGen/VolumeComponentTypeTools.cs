using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUtilities
{
    /// <summary>
    /// handle VolumeComponent paramerters type
    /// read file : VolumeComponentParamaterTypes.txt
    /// </summary>
    public static class VolumeComponentTypeTools
    {
        // save precision types
        static Dictionary<string, string> precisionTypeDict = new();
        // save string match types
        static Dictionary<string, string> matchTypeTypeDict = new();


        public static void SetupComponentTypeInfoDict(string componentTypeInfo)
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

        public static string ConvertVolumeTypeName(string typeName)
        {
            // check precision dict
            if (precisionTypeDict.TryGetValue(typeName, out var typeStr))
                return typeStr;
            //check match mapping dict
            if (matchTypeTypeDict.TryFindByKey(k => typeName.Contains(k), out typeStr))
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
    }
}
