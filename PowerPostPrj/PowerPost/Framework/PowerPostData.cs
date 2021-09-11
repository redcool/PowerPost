namespace PowerPost
{
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
    [CustomEditor(typeof(PowerPostData))]
    public class PowerPostDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var inst = target as PowerPostData;
            if (GUILayout.Button("Refresh"))
            {
                inst.InitSettingTypes();
            }
        }
    }
#endif

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
        [SerializeField] string[] settingNames;
        public string[] SettingNames => settingNames;

        Type[] settingTypes;
        public Type[] GetSettingTypes()
        {
            if(settingTypes == null || settingTypes.Length != settingNames.Length || settingTypes.Contains(null))
            {
                InitSettingTypes();
            }
            return settingTypes;
        }
        public void InitSettingTypes()
        {
            settingTypes = new Type[settingNames.Length];
            for (int i = 0, count = settingNames.Length; i < count; i++)
            {
                settingTypes[i] = Type.GetType(settingNames[i]);
            }
        }
    }
}
