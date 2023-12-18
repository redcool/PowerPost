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

    [CustomEditor(typeof(PowerPostFeature))]
    public class PowerPostFeatureEditor : Editor
    {
        bool isShowSettingName;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var inst = target as PowerPostFeature;
            isShowSettingName = EditorGUILayout.Foldout(isShowSettingName, "Current Working Effects:", true);
            if (isShowSettingName && inst.postSettingTypeList!=null)
            {
                var list = inst.postSettingTypeList
                    .Select(type => inst.GetPassSettings(type, true))
                    .Where(type => type != default)
                    .OrderBy(settings => settings.Order)
                    ;
                Drawlist(list.Where(settings => settings.NeedWriteToTarget()), "List need cameraTarget");
                Drawlist(list.Where(settings => !settings.NeedWriteToTarget()), "List dont need cameraTarget");
            }

        }

        private static void Drawlist(IEnumerable<BasePostExSettings> list,string name)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(name,list.Count().ToString());
            EditorGUI.indentLevel++;
            list.ForEach((item, id) =>
            {
                EditorGUILayout.SelectableLabel($"{id} : {item.GetType().FullName}", GUILayout.Height(18));
            });
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
#endif
    public class PowerPostFeature : ScriptableRendererFeature
    {
        /// <summary>
        /// passes need write to targetTex
        /// </summary>
        static HashSet<Type> postSettingTypeSet = new HashSet<Type>();
        //save and sort
        public List<Type> postSettingTypeList = new List<Type>();

        // cache settingType and pass corresponded
        Dictionary<Type, BasePostExPass> postPassDict = new Dictionary<Type, BasePostExPass>();

        PowerPostFeaturePass postPass;

        void TryInitPostSettingTypeList(ref List<Type> list,ref HashSet<Type> set)
        {
            // find all again,when change RenderScale,will trigger this
            if (set.Count == 0) 
            {
                FindAllSettingTypes(set);
            }

            if (list == default || list.Count == set.Count)
                return;

            list = set.ToList();
        }


        public static void FindAllSettingTypes(HashSet<Type> set)
        {
            var settingTypes = ReflectionTools.GetTypesDerivedFrom<BasePostExSettings>();

            foreach (var setting in settingTypes)
            {
                set.Add(setting);
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            ref CameraData cameraData = ref renderingData.cameraData;
            if (!cameraData.postProcessEnabled)
                return;

            TryInitPostSettingTypeList(ref postSettingTypeList,ref postSettingTypeSet);

            List<BasePostExPass> listNeedWriteTarget = new List<BasePostExPass>();
            List<BasePostExPass> listDontNeedWriteTarget = new List<BasePostExPass>();
            SetupPasses(listNeedWriteTarget, listDontNeedWriteTarget);

            //add sorted list
            listNeedWriteTarget.ForEach((item,id) => renderer.EnqueuePass(item.InitStatesForWrtieCameraTarget(id, listNeedWriteTarget.Count())));

            // add unsorted
            listDontNeedWriteTarget.ForEach(item=>renderer.EnqueuePass(item));
        }

        private void SetupPasses(List<BasePostExPass> listNeedWriteTarget, List<BasePostExPass> listDontNeedWriteTarget)
        {
            postSettingTypeList.ForEach(type => {
                var settings = GetPassSettings(type);
                if (settings != default)
                {
                    var pass = GetPassInstance(type, settings);
                    if (settings.NeedWriteToTarget())
                        listNeedWriteTarget.Add(pass);
                    else
                        listDontNeedWriteTarget.Add(pass);
                }
            });

            listNeedWriteTarget.Sort((a, b) => a.order - b.order);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            postSettingTypeSet.Clear();
        }

        public override void Create()
        {
            postPass = new PowerPostFeaturePass();
            postPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public BasePostExSettings GetPassSettings(Type type,bool includeInactive=false)
        {
            if (type == default)
                return default;

            var settings = VolumeManager.instance.stack.GetComponent(type) as BasePostExSettings;
            if (settings == null || settings.IsActive() == includeInactive)
                return default;

            return settings;
        }

        public BasePostExPass GetPassInstance(Type settingType,BasePostExSettings settings)
        {
            if (!postPassDict.TryGetValue(settingType, out var pass))
            {
                postPassDict[settingType] = pass = settings.CreateNewInstance();
                pass.order = settings.Order;
            }
            return postPassDict[settingType];
        }
    }


}
