using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPPostProcessingEx
{
    public abstract class PostExPass : ScriptableRenderPass
    {
        public ScriptableRenderer Renderer { set; get; }

    }
}
