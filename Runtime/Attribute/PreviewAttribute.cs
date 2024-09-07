using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

namespace UtilityEngine
{
    public class PreviewAttribute : PropertyAttribute
    {
        public readonly int margin;

        public PreviewAttribute(int margin = 4)
        {
            this.margin = margin;
        }
    }
}
