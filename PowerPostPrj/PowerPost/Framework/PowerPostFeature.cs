using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(PowerPostFeature))]
    public class PowerPostFeatureEditor : Editor
    {
        bool isShowSettingName;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var inst = target as PowerPostFeature;
            isShowSettingName = EditorGUILayout.Foldout(isShowSettingName, "Current Working Effects:", true);
            if (isShowSettingName)
            {
                //var settingsProp = serializedObject.FindProperty("powerPostExSettingTypes");
                //EditorGUILayout.PropertyField(settingsProp);
                EditorGUILayout.BeginVertical("Box");
                foreach (var item in inst.postExSettingTypes)
                {
                    EditorGUILayout.LabelField(item.FullName);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
#endif
    public class PowerPostFeature : ScriptableRendererFeature
    {
        static HashSet<Type> postExSettingSet = new HashSet<Type>();
        public List<Type> postExSettingTypes = new List<Type>();

        Dictionary<Type, BasePostExPass> postPassDict = new Dictionary<Type, BasePostExPass>();
        PowerPostFeaturePass postPass;
        public List<BasePostExPass> PostExPassList{private set;get;}

        public static void AddSetting(BasePostExSettings setting)
        {
            postExSettingSet.Add(setting.GetType());
        }

        //public static void RemoveSetting(BasePostExSettings setting)
        //{
        //    postExSettingSet.Remove(setting.GetType());
        //}

        void InitPostExSettingTypes()
        {
            postExSettingTypes.Clear();
            foreach (var item in postExSettingSet)
            {
                postExSettingTypes.Add(item);
            }
            
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (postExSettingTypes.Count != postExSettingSet.Count)
                InitPostExSettingTypes();

            AddPostPassByTypes(renderer, postExSettingTypes);

            //postPass.passList = PostExPassList;
            //renderer.EnqueuePass(postPass);
        }

        public override void Create()
        {
            InitPostExSettingTypes();
            postPass = new PowerPostFeaturePass();
            postPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            PostExPassList = new List<BasePostExPass>();
        }

        void AddPostPassByTypes(ScriptableRenderer renderer,List<Type> postSettingTypes)
        {
            var id = 0;
            PostExPassList.Clear();
            foreach (var t in postSettingTypes)
            {
                if (t == null)
                {
                    Debug.LogWarning($"PowerPostData.SettingsNames ,Element {id} can't found.");
                    continue;
                }

                var settings = VolumeManager.instance.stack.GetComponent(t) as BasePostExSettings;
                if (settings == null || !settings.IsActive())
                    continue;

                BasePostExPass pass;
                var settingType = settings.GetType();
                if (!postPassDict.TryGetValue(settingType, out pass))
                {
                    postPassDict[settingType] = pass = settings.CreateNewInstance();
                    //pass.ConfigureTarget(renderer.cameraColorTarget, renderer.cameraDepthTarget);
                    pass.Renderer = renderer;
                }
                renderer.EnqueuePass(pass);
                PostExPassList.Add(pass);


                id++;
            }
        }
    }


}
