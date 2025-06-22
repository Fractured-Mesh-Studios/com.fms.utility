using UnityEngine;

namespace UtilityEngine
{
    public class IndentAttribute : PropertyAttribute
    {
        public readonly int indent;

        public IndentAttribute(int indent)
        {
            this.indent = indent;
        }
    }
}
