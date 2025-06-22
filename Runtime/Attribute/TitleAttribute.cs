using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityEngine
{
    public enum TitleAlignment
    {
        Left,
        Center,
        Right
    }

    public class TitleAttribute : PropertyAttribute
    {
        public string title { get; }
        public string subtitle { get; }
        public string iconPath { get; } // Relative to "Assets/Editor Default Resources/..."

        public TitleAlignment alignment { get; }

        public TitleAttribute(string title)
        {
            this.title = title;
            this.subtitle = string.Empty;
            this.iconPath = string.Empty;
            this.alignment = alignment;
        }

        public TitleAttribute(string title, TitleAlignment alignment = TitleAlignment.Left)
        {
            this.title = title;
            this.subtitle = string.Empty;
            this.iconPath = string.Empty;
            this.alignment = alignment;
        }

        public TitleAttribute(string title, string subtitle, TitleAlignment alignment = TitleAlignment.Left)
        {
            this.title = title;
            this.subtitle = subtitle;
            this.iconPath = string.Empty;
            this.alignment = alignment;
        }

        public TitleAttribute(string title, string subtitle, string iconPath, TitleAlignment alignment = TitleAlignment.Left)
        {
            this.title = title;
            this.subtitle = subtitle;
            this.iconPath = iconPath;
            this.alignment = alignment;
        }
    }
}
