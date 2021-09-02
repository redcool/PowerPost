using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PowerPost
{
    [Serializable]
    public class PowerPostData : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Rendering/Universal Render Pipeline/PowerPostData", priority = CoreUtils.assetCreateMenuPriority3 + 1)]
        static void CreateSaveAsset()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder(path)){
                path = Path.GetDirectoryName(path);
            }

            var inst = CreateInstance<PowerPostData>();
            AssetDatabase.CreateAsset(inst, $"{path}/{nameof(PowerPostData)}.asset");
            Selection.activeObject = inst;
        }
#endif
        [SerializeField] string[] passNames;
        public string[] PassNames => passNames;

        Type[] types;
        public Type[] GetSettingTypes()
        {
            if(types == null || types.Length != passNames.Length)
            {
                types = new Type[passNames.Length];
                for (int i = 0, count = passNames.Length; i < count; i++)
                {
                    types[i] = Type.GetType(passNames[i]);
                }
            }
            return types;
        }
    }
}
