using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PowerUtilities;
using System.Reflection;
using System.Linq;

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
            if (isShowSettingName && inst.postSettingTypeList != null)
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

        private static void Drawlist(IEnumerable<BasePostExSettings> list, string name)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(name, list.Count().ToString());
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
        /// (powerPost dll) effect passes,Type : BasePostExSettings
        /// </summary>
        public static HashSet<Type> postSettingTypeSet = new HashSet<Type>();

        /// <summary>
        /// (other dll) effect passes ,Type : BasePostExSettings
        /// </summary>
        public static HashSet<Type> externalSettingTypeSet = new HashSet<Type>();
        static List<Type> externalSettingTypeList;
        /// <summary>
        /// for sort
        /// </summary>
        public List<Type> postSettingTypeList = new List<Type>();

        // cache settingType and pass corresponded
        Dictionary<Type, BasePostExPass> postPassDict = new Dictionary<Type, BasePostExPass>();

        PowerPostFeaturePass postPass;

        void TryInitPostSettingTypeList(ref List<Type> list, ref HashSet<Type> set)
        {
            // find all again,when change RenderScale,will trigger this
            if (set.Count == 0)
            {
                FindAllSettingTypes(ref set);
            }

            AddExternalSettingTypes(ref set);

            if (list == default || list.Count == set.Count)
                return;

            list = set.ToList();
        }

        private static void AddExternalSettingTypes(ref HashSet<Type> set)
        {
            // lazy to list
            if (externalSettingTypeList == null)
                externalSettingTypeList = externalSettingTypeSet.ToList();

            // add external item
            foreach (var item in externalSettingTypeList)
            {
                set.Add(item);
            }
            // clear once
            externalSettingTypeList.Clear();
        }

        /// <summary>
        /// add T to postSettingTypeSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddEffectSetting<T>() where T : BasePostExSettings
        {
            externalSettingTypeSet.Add(typeof(T));
            externalSettingTypeList = null;
        }

        public static void FindAllSettingTypes(ref HashSet<Type> set)
        {
            var settingTypes = ReflectionTools.GetTypesDerivedFrom<BasePostExSettings>();

            foreach (var setting in settingTypes)
            {
                set.Add(setting);
            }
        }

        // keep 20, avoid list resize
        List<BasePostExPass> listNeedWriteTarget = new List<BasePostExPass>(20);
        List<BasePostExPass> listDontNeedWriteTarget = new List<BasePostExPass>(20);

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            ref CameraData cameraData = ref renderingData.cameraData;
            if (!cameraData.postProcessEnabled)
                return;

            listNeedWriteTarget.Clear();
            listDontNeedWriteTarget.Clear();

            TryInitPostSettingTypeList(ref postSettingTypeList, ref postSettingTypeSet);

            SetupPasses(listNeedWriteTarget, listDontNeedWriteTarget);

            //add sorted list
            Action<BasePostExPass, int> addPassesWithTarget = (item, id) => renderer.EnqueuePass(item.InitStatesForWrtieCameraTarget(id, listNeedWriteTarget.Count()));
            listNeedWriteTarget.ForEach(addPassesWithTarget);

            // add unsorted
            Action<BasePostExPass> addPasses = item => renderer.EnqueuePass(item);
            listDontNeedWriteTarget.ForEach(addPasses);

        }

        private void SetupPasses(List<BasePostExPass> listNeedWriteTarget, List<BasePostExPass> listDontNeedWriteTarget)
        {
            //postSettingTypeList.ForEach(AddPassTypes);
            for (int i = 0; i < postSettingTypeList.Count; i++)
            {
                AddPassTypes(postSettingTypeList[i], i);

                //if (i > 10)
                //    break;
            }

            // sort
            Comparison<BasePostExPass> sortPasses = (a, b) => a.order - b.order;
            listNeedWriteTarget.Sort(sortPasses);

            //----------- inner method
            void AddPassTypes(Type type,int id)
            {
                var settings = GetPassSettings(type);
                if (settings != default)
                {
                    var pass = GetPassInstance(type, settings);
                    if (settings.NeedWriteToTarget())
                        listNeedWriteTarget.Add(pass);
                    else
                        listDontNeedWriteTarget.Add(pass);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (postSettingTypeSet != null)
                postSettingTypeSet.Clear();

            if (externalSettingTypeSet != null)
                externalSettingTypeSet.Clear();

            if (externalSettingTypeList != null)
                externalSettingTypeList.Clear();
        }

        public override void Create()
        {
            //postPass = new PowerPostFeaturePass();
            //postPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public BasePostExSettings GetPassSettings(Type type, bool includeInactive = false)
        {
            if (type == default)
                return default;

            var settings = VolumeManager.instance.stack.GetComponent(type) as BasePostExSettings;
            if (settings == null || settings.IsActive() == includeInactive)
                return default;

            return settings;
        }

        public BasePostExPass GetPassInstance(Type settingType, BasePostExSettings settings)
        {

            return DictionaryTools.Get(postPassDict, settingType, CreateInstance);

            //if (!postPassDict.TryGetValue(settingType, out var pass))
            //{
            //    postPassDict[settingType] = pass = settings.CreateNewInstance();
            //    pass.order = settings.Order;
            //}
            //return postPassDict[settingType];

            //---------------
            BasePostExPass CreateInstance(Type settingType)
            {
                var pass = settings.CreateNewInstance();
                pass.order = settings.Order;
                return pass;
            }
        }
    }


}
