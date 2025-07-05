using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleAttributeDrawer : PropertyDrawer
    {
        private const float IconSize = 32f;
        private const float Padding = 6f;
        private const float TitleHeight = 18f;
        private const float SubtitleHeight = 14f;

        private string text = string.Empty;
        private bool isSubTitle = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            TitleAttribute attr = (TitleAttribute)attribute;
            float iconSize = 0;
            float padding = 0;

            iconSize = attr.iconPath != string.Empty ? IconSize : 0;

            padding = attr.subtitle != string.Empty ? Padding * 3 : 2f;

            float height = !property.isExpanded ? 
                base.GetPropertyHeight(property, label) : 
                EditorGUI.GetPropertyHeight(property, label, true);
            
            //Title Attrib Compatibility
            var otherAttributes = fieldInfo
                .GetCustomAttributes(typeof(PropertyAttribute), true)
                .OfType<PropertyAttribute>()
                .Where(a => !(a is TitleAttribute))
                .ToArray();

            if (otherAttributes.Length > 0)
                height += 8f;
            //Title Attrib Compatibility

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            text = label.text;

            TitleAttribute attr = (TitleAttribute)attribute;
            Texture2D icon = default;
            if (attr.iconPath != string.Empty)
            {
                // Load icon
                icon = EditorGUIUtility.Load(attr.iconPath) as Texture2D;
                // Icon Rect
                Rect iconRect = new Rect(position.x, position.y, IconSize, IconSize);

                // Title Rect
                Rect titleRect = new Rect(position.x + IconSize + Padding, position.y, position.width - IconSize - Padding, TitleHeight);

                // Subtitle Rect
                Rect subtitleRect = new Rect(titleRect.x, titleRect.y + TitleHeight, titleRect.width, SubtitleHeight);

                // Draw icon
                if (icon != null)
                    GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

                // Draw Title
                DrawTitle(titleRect);

                DrawSubTitle(subtitleRect, Color.gray);

                DrawField(position, property, label);
            }
            else
            {
                // Title Rect
                Rect titleRect = new Rect(position.x, position.y, position.width, TitleHeight);

                // Subtitle Rect
                Rect subtitleRect = new Rect(titleRect.x, titleRect.y + TitleHeight, titleRect.width, SubtitleHeight);

                // Draw
                DrawTitle(position);

                DrawSubTitle(subtitleRect, Color.gray);

                DrawField(position, property, label);
                
                
            }

            
        }

        private void DrawTitle(Rect position)
        {
            TitleAttribute attr = (TitleAttribute)attribute;

            // Title Rect
            Rect titleRect = new Rect(position.x, position.y, position.width, TitleHeight);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.alignment = TextAnchor.MiddleCenter;
            switch (attr.alignment) { 
                case TitleAlignment.Left: style.alignment = TextAnchor.MiddleLeft; break;
                case TitleAlignment.Center: style.alignment = TextAnchor.MiddleCenter; break;
                case TitleAlignment.Right: style.alignment = TextAnchor.MiddleRight; break;
            }

            // Draw title
            EditorGUI.LabelField(titleRect, attr.title, style);
        }

        private void DrawSubTitle(Rect position, Color color)
        {
            TitleAttribute attr = (TitleAttribute)attribute;

            GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
            style.alignment = TextAnchor.MiddleCenter;
            switch (attr.alignment)
            {
                case TitleAlignment.Left: style.alignment = TextAnchor.MiddleLeft; break;
                case TitleAlignment.Center: style.alignment = TextAnchor.MiddleCenter; break;
                case TitleAlignment.Right: style.alignment = TextAnchor.MiddleRight; break;
            }

            if (attr.subtitle != string.Empty)
            {
                GUI.color = Color.grey;
                EditorGUI.LabelField(position, attr.subtitle, style);
                GUI.color = Color.white;

                isSubTitle = true;
            }
            else
            {
                isSubTitle = false;
            }
        }

        private void DrawField(Rect position, SerializedProperty prop, GUIContent content)
        {
            float height = 0f;

            height = isSubTitle ? Padding * 3f : 4f;

            // Property field rect
            Rect fieldRect = new Rect(position.x, position.y + TitleHeight + height, position.width, EditorGUI.GetPropertyHeight(prop));

            Rect lineRect = EditorGUILayout.GetControlRect();

            lineRect.height = 1;
            lineRect.position = new Vector2(fieldRect.position.x, fieldRect.position.y - 5);

            EditorGUI.PropertyField(fieldRect, prop, new GUIContent(text, content.tooltip), true);

            EditorGUI.DrawRect(lineRect, Color.gray);
        }
    }
}
