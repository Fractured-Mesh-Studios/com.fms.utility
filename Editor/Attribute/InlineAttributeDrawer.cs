using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(InlineAttribute))]
    public class InlineAttributeDrawer : PropertyDrawer
    {
        private bool foldout;
        private Editor editor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.BeginVertical(UtilityEditor.MINI_BOX);
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(property);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(2);
            DrawSettingsEditor(property.objectReferenceValue, ref foldout, ref editor);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
        }

        private void DrawSettingsEditor(Object @object, ref bool foldout, ref Editor editor)
        {
            if (@object == null) return;

            EditorGUILayout.BeginVertical();
            foldout = EditorGUILayout.InspectorTitlebar(foldout, @object);

            if (foldout)
            {
                EditorGUI.indentLevel++;

                if (editor == null || !editor.target.Equals(@object))
                {
                    Editor.CreateCachedEditor(@object, GetEditorType(@object), ref editor);
                }

                if (editor != null)
                {
                    editor.OnInspectorGUI();
                }
                else
                {
                    EditorGUILayout.HelpBox("No editor available for this object type.", MessageType.Warning);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private System.Type GetEditorType(Object @object)
        {
            if (@object is Material)
            {
                return typeof(MaterialEditor);
            }

            return typeof(Editor);
        }
    }
}