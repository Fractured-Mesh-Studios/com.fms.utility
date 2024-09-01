using UnityEngine;

namespace UtilityEngine
{
    public class EnumAttribute : PropertyAttribute
    {
        public float minHeight { get; private set; }
        public float maxHeight { get; private set; }
        public bool showLabel { get; private set; }

        public EnumAttribute(float minHeight = 100f, float maxHeight = 300f, bool showLabel = false)
        {
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.showLabel = showLabel;
        }
    }
}