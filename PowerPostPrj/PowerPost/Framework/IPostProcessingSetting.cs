﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    interface IPostProcessingSetting
    {
        public PostExPass CreateNewInstance();
    }
}