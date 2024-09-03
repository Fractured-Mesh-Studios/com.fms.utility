using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class Preview3DAttribute : PropertyAttribute
    {
        public readonly int margin;

        public Preview3DAttribute(int margin = 4)
        {
            this.margin = margin; 
        }
    }
}
