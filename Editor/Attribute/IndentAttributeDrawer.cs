using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(IndentAttribute))]
    public class IndentAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IndentAttribute attr = (IndentAttribute)attribute;
                
            EditorGUI.indentLevel = attr.indent;
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.indentLevel = 0;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
