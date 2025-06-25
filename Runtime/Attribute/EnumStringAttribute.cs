using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class EnumStringAttribute : PropertyAttribute
    {
        public readonly string[] options;

        public EnumStringAttribute(params string[] options)
        {
            this.options = options;
        }
    }
}
