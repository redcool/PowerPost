using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public class PowerPostFeaturePass : ScriptableRenderPass
    {
        public List<BasePostExPass> passList;

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            return;

            //if (passList == null)
            //    return;

            //ref CameraData cameraData = ref renderingData.cameraData;
            //var targetDesc = cameraData.cameraTargetDescriptor;

            //foreach (var item in passList)
            //{
            //    var name = item.GetType().Name;
            //    var cmd = CommandBufferUtils.GetBufferBeginSample(context, name);


            //    item.OnCameraSetup(cmd, ref renderingData);

            //    // execute pass
            //    item.Configure(cmd, targetDesc);

            //    item.Execute(context, ref renderingData);

            //    item.FrameCleanup(cmd);

            //    context.ExecuteCommandBuffer(cmd);

            //    CommandBufferUtils.ReleaseBufferEndSample(cmd);
            //}
        }
    }
}
