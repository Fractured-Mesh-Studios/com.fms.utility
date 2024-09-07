using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public class GUIColorAttribute : PropertyAttribute
    {
        public string black = Color.black.ToString(); 

        public readonly Color color;

        public GUIColorAttribute()
        {
            this.color = Color.white;
        }

        public GUIColorAttribute(float r, float g, float b, float a = 1.0f)
        {
            color = new Color(r, g, b, a);
        }

        public GUIColorAttribute(string hexColor)
        {
            ColorUtility.TryParseHtmlString(hexColor, out Color color);
            this.color = color;
        }
    }
}
