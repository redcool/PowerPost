namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public static class CommandBufferUtils 
    {
        public static CommandBuffer GetBuffer(ScriptableRenderContext context,string name ="")
        {
            var cmd = CommandBufferPool.Get(name);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            return cmd;
        }

        public static void ReleaseBuffer(CommandBuffer cmd)
        {
            CommandBufferPool.Release(cmd);
        }
    }
}