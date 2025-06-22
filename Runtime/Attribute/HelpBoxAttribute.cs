using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public enum HelpBoxType
    {
        Info,
        Warning,
        Error
    }

    public class HelpBoxAttribute : PropertyAttribute
    {
        public string Message { get; private set; }
        public HelpBoxType Type { get; private set; }

        public HelpBoxAttribute(string message, HelpBoxType type = HelpBoxType.Info)
        {
            Message = message;
            Type = type;
        }
    }
}
