using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(BoxAttribute))]
    public class BoxAttributeDrawer : PropertyDrawer
    {
        public const int PADDING = 3;
        public const int BORDER = 1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BoxAttribute att = (BoxAttribute)attribute;

            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            float padding = att.padding ? PADDING : 0f;
            float borderSize = att.border ? BORDER : 0f;

            float totalVertical = padding * 2f;

            Rect boxRect = new Rect(
                position.x - borderSize,
                position.y - borderSize,
                position.width + borderSize * 2f,
                propertyHeight + totalVertical + borderSize * 2f
            );

            Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUI.DrawRect(boxRect, backgroundColor);

            if (att.border)
            {
                Handles.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                Handles.DrawSolidRectangleWithOutline(boxRect, Color.clear, Handles.color);
            }
            else
            {
                EditorGUI.DrawRect(boxRect, new Color(0.2f, 0.2f, 0.2f, 1f));
            }

            Rect propertyRect = new Rect(
                position.x + padding,
                position.y + padding,
                position.width - padding * 2f,
                propertyHeight
            );

            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            BoxAttribute att = (BoxAttribute)attribute;
            float padding = att.padding ? PADDING : 0f;
            float borderSize = att.border ? BORDER : 0f;

            return EditorGUI.GetPropertyHeight(property, label, true)
                   + (padding * 2f) + (borderSize * 2f);
        }
    }
}
