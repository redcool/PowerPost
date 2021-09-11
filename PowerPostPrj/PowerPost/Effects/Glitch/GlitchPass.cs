namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class GlitchPass : BasePostExPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var settings = GetSettings<GlitchSettings>();
            if (!settings.IsActive())
                return;

            var cmd = CommandBufferUtils.GetBuffer(context,nameof(GlitchPass));
            cmd.BeginSample(cmd.name);



            cmd.EndSample(cmd.name);
            CommandBufferUtils.ReleaseBuffer(cmd);
        }
    }
}