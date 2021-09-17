namespace PowerPost.KeyFrame
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;


    public class PowerPostKeyframeControl : MonoBehaviour
    {
        public Volume volume;

        
        [Header("---")]
        public float glitch_scanlineJitter = 0;
        public float glitch_snowFlakeAmplitude = 0;
        public float glitch_verticalJump = 0;
        public float glitch_horizontalShake = 0;
        public float glitch_colorDrift = 0;
        public LayerMask glitch_layer = 0;
        public int glitch_stencilRef = 6;
        [Header("---")]
        public Color ssss_strength = (Color.white);
        public Color ssss_falloff = Color.red;
        public float ssss_blurScale = 0.1f;
        public LayerMask ssss_layer = 0;
        public int ssss_stencilRef = 5;
        [Header("---")]
        public Vector2 radialBlur_center = new Vector2(.5f, .5f);
        public float radialBlur_radiusMin = 0;
        public float radialBlur_radiusMax = 1;
        public float radialBlur_blurSize = 0;
        public bool radialBlur_roundness = false;


        GlitchSettings glitchSettings;
        SSSSSettings ssssSettings;
        RadialBlurSettings radialBlurSettings;

        private void OnEnable()
        {
            if (!volume)
                return;

            volume.profile.TryGet(out glitchSettings);
            volume.profile.TryGet(out ssssSettings);
            volume.profile.TryGet(out radialBlurSettings);
        }

        private void Update()
        {
            if (!volume)
                return;

            UpdateGlitch();
            UpdateSSSS();
            UpdateRadialBlur();
        }

        private void UpdateRadialBlur()
        {
            if (!radialBlurSettings)
                return;
            radialBlurSettings.center.value = radialBlur_center;
            radialBlurSettings.blurSize.value = radialBlur_blurSize;
            radialBlurSettings.radiusMin.value = radialBlur_radiusMin;
            radialBlurSettings.radiusMax.value = radialBlur_radiusMax;
            radialBlurSettings.roundness.value = radialBlur_roundness;
        }

        private void UpdateSSSS()
        {
            if (!ssssSettings)
                return;
            ssssSettings.blurScale.value = ssss_blurScale;
            ssssSettings.falloff.value = ssss_falloff;
            ssssSettings.layer.value = ssss_layer;
            ssssSettings.stencilRef.value = ssss_stencilRef;
            ssssSettings.strength.value = ssss_strength;
        }

        private void UpdateGlitch()
        {
            if (!glitchSettings)
                return;

            glitchSettings.colorDrift.value = glitch_colorDrift;
            glitchSettings.horizontalShake.value = glitch_horizontalShake;
            glitchSettings.layer.value = glitch_layer;
            glitchSettings.scanlineJitter.value = glitch_scanlineJitter;
            glitchSettings.snowFlakeAmplitude.value = glitch_snowFlakeAmplitude;
            glitchSettings.stencilRef.value = glitch_stencilRef;
            glitchSettings.verticalJump.value = glitch_verticalJump;
        }
    }
}