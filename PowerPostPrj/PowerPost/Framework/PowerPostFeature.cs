using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PowerUtilities;
using System.Reflection;

namespace PowerPost
{
    using System.Linq;
#if UNITY_EDITOR
    using UnityEditor;
    using static UnityEditor.Progress;

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
                foreach (var item in inst.PostSettingList)
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

        public List<BasePostExPass> PostExPassList{private set;get;}
        public List<Type> PostSettingList => postSettingList;


        static HashSet<Type> postSettingTypeSet = new HashSet<Type>();
        //save and sort
        List<Type> postSettingList = new List<Type>();
        
        // for cache post pass
        List<BasePostExPass> postPassList = new List<BasePostExPass>();

        // cache settingType and pass corresponded
        Dictionary<Type, BasePostExPass> postPassDict = new Dictionary<Type, BasePostExPass>();

        PowerPostFeaturePass postPass;

        public static void AddSetting(BasePostExSettings setting)
        {
            var type = setting.GetType();
            postSettingTypeSet.Add(type);
        }

        void TryInitPostSettingList()
        {
            if (postSettingList == default || postSettingList.Count == postSettingTypeSet.Count)
                return;

            postSettingList = postSettingTypeSet.ToList();
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            ref CameraData cameraData = ref renderingData.cameraData;
            if (!cameraData.postProcessEnabled)
                return;

            TryInitPostSettingList();

            postPassList = postSettingList.Select(type => GetPostExPass(type))
            .Where(item => item != default)
            .OrderBy(item => item.order)
            .ToList();

            var needSwapTarget = renderer.cameraColorTarget.IsTargetIdEquals(ShaderPropertyIds._CameraColorAttachmentB);

            postPassList.ForEach((item, id) => renderer.EnqueuePass(item.Init(id, postPassList.Count(), needSwapTarget)));
            //postPassList.ForEach((item)=> Debug.Log(item.ToString()));
        }

        public override void Create()
        {
            postPass = new PowerPostFeaturePass();
            postPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        BasePostExPass GetPostExPass(Type type)
        {
            if (type == default)
                return default;

            var settings = VolumeManager.instance.stack.GetComponent(type) as BasePostExSettings;
            if (settings == null || !settings.IsActive())
                return default;

            BasePostExPass pass;
            var settingType = settings.GetType();
            if (!postPassDict.TryGetValue(settingType, out pass))
            {
                postPassDict[settingType] = pass = settings.CreateNewInstance();
                pass.order = settings.Order;
            }
            return postPassDict[settingType];
        }
    }


}
