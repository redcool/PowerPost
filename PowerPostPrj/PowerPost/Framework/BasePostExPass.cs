using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public abstract class BasePostExPass : ScriptableRenderPass
    {
        public ScriptableRenderer Renderer { set; get; }

        public T GetSettings<T>() where T : VolumeComponent
        {
            return VolumeManager.instance.stack.GetComponent<T>();
        }
    }
}
