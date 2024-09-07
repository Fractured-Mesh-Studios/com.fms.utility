using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : PropertyDrawer
    {
        private bool m_foldout;
        private int m_margin = 4;

        private Color skin => EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.8f, 0.8f, 0.8f);

        private float singleLineHeight => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attrib = (PreviewAttribute)attribute;
            m_margin = attrib.margin;

            Rect outerRect = new Rect(position.x, position.y, position.width, GetPropertyHeight(property, label));
            Rect innerRect = new Rect(outerRect.x + m_margin + 10, outerRect.y + m_margin, outerRect.width - 2 * m_margin - 10, singleLineHeight);
            Rect objectFieldRect = new Rect(innerRect.x + 15, innerRect.y, innerRect.width - 15, singleLineHeight);

            EditorGUI.DrawRect(outerRect, skin);

            m_foldout = EditorGUI.Foldout(innerRect, m_foldout, GUIContent.none, true);

            EditorGUI.PropertyField(objectFieldRect, property, label);

            innerRect.y += singleLineHeight + 2;

            if (property.objectReferenceValue != null && CanBePreviewed(property.objectReferenceValue))
            {
                if (m_foldout)
                {
                    Rect previewRect = new Rect(outerRect.x + m_margin, innerRect.y + 2, outerRect.width - 2 * m_margin, 256);

                    GUIStyle previewStyle = new GUIStyle();
                    previewStyle.normal.background = EditorGUIUtility.whiteTexture;

                    Texture2D texture = property.objectReferenceValue as Texture2D;

                    GUI.DrawTexture(previewRect, texture, ScaleMode.ScaleToFit);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = this.singleLineHeight + m_margin * 2;

            if (m_foldout && property.objectReferenceValue != null && CanBePreviewed(property.objectReferenceValue))
            {
                height += 256 + m_margin;
            }

            return height;
        }

        private bool CanBePreviewed(Object obj)
        {
            return obj is Texture || obj is Sprite;
        }
    }
}
