using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class BoxAttribute : PropertyAttribute
    {
        public readonly bool padding;
        public readonly bool border;

        public BoxAttribute()
        {
            this.padding = true;
            this.border = false;
        }

        public BoxAttribute(bool padding, bool border)
        {
            this.padding = padding;
            this.border = border;
        }
    }
}
