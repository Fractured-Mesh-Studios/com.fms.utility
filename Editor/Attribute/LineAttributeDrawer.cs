using UnityEditor;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(LineAttribute))]
    public class LineAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LineAttribute att = (LineAttribute)attribute;

            float y;
            if (att.top) 
            {
                Rect propRect = position;

                y = position.y - att.height;
                position.y = y;
                position.height = att.height;

                propRect.y += att.height;

                EditorGUI.PropertyField(propRect, property, label, true);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);

                y = position.y + position.height - (att.height - att.height/2);
                position.y = y;
                position.height = att.height;
            }

            EditorGUI.DrawRect(position, Color.grey);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            LineAttribute att = (LineAttribute)attribute;

            return EditorGUI.GetPropertyHeight(property, label, true) + att.height;
        }
    }
}
