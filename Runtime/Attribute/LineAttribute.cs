using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class LineAttribute : PropertyAttribute
    {
        public readonly int height;
        public readonly bool top;

        public LineAttribute(int height = 2, bool top = true)
        {
            this.height = Mathf.Clamp(height, 2, 8);
            this.top = top;
        }
    }
}
