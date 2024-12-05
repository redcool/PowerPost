///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
using PowerUtilities.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace PowerUtilities
{
    [ExecuteAlways]
    public class PowerPostVolumeControlMono : MonoBehaviour
    {
        // data fields
        //public Bloom_Data _Bloom_Data;
        public GlitchSettings_Data _GlitchSettings_Data;
public OutlineSettings_Data _OutlineSettings_Data;
public OutlineExtendSettings_Data _OutlineExtendSettings_Data;
public RadialBlurSettings_Data _RadialBlurSettings_Data;
public SimpleBloomSettings_Data _SimpleBloomSettings_Data;
public SimpleDOFSettings_Data _SimpleDOFSettings_Data;
public SketchSettings_Data _SketchSettings_Data;
public SSAOSettings_Data _SSAOSettings_Data;
public SSSSSettings_Data _SSSSSettings_Data;
public SunShaftSettings_Data _SunShaftSettings_Data;
public ToneMappingSettings_Data _ToneMappingSettings_Data;
public VignetteSettings_Data _VignetteSettings_Data;
public VolumeLightingSettings_Data _VolumeLightingSettings_Data;
public WhiteBalanceSettings_Data _WhiteBalanceSettings_Data;


        private void Update()
        {
            var clipVolume = gameObject.GetOrAddComponent<Volume>();

            //data update
            //VolumeDataTools.Update(clipVolume, _Bloom_Data);
              VolumeDataTools.Update(clipVolume, _GlitchSettings_Data);
  VolumeDataTools.Update(clipVolume, _OutlineSettings_Data);
  VolumeDataTools.Update(clipVolume, _OutlineExtendSettings_Data);
  VolumeDataTools.Update(clipVolume, _RadialBlurSettings_Data);
  VolumeDataTools.Update(clipVolume, _SimpleBloomSettings_Data);
  VolumeDataTools.Update(clipVolume, _SimpleDOFSettings_Data);
  VolumeDataTools.Update(clipVolume, _SketchSettings_Data);
  VolumeDataTools.Update(clipVolume, _SSAOSettings_Data);
  VolumeDataTools.Update(clipVolume, _SSSSSettings_Data);
  VolumeDataTools.Update(clipVolume, _SunShaftSettings_Data);
  VolumeDataTools.Update(clipVolume, _ToneMappingSettings_Data);
  VolumeDataTools.Update(clipVolume, _VignetteSettings_Data);
  VolumeDataTools.Update(clipVolume, _VolumeLightingSettings_Data);
  VolumeDataTools.Update(clipVolume, _WhiteBalanceSettings_Data);

        }
    }
}
