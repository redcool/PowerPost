namespace PowerPost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public static class CommandBufferUtils 
    {


        public static CommandBuffer Get(ref ScriptableRenderContext context,string name ="")
        {
            var cmd = CommandBufferPool.Get(name);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            //cmd.BeginSample(name);
            return cmd;
        }

        public static void ClearRelease(CommandBuffer cmd)
        {
            //cmd.EndSample(cmd.name);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

    }
}