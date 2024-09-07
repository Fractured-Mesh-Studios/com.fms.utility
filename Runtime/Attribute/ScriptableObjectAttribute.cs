using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class InlineAttribute : PropertyAttribute
    {
        public string label;

        public InlineAttribute(string label = "")
        {
            this.label = label;
        }
    }
}