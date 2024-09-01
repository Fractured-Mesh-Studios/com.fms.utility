using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(ScriptableObjectAttribute))]
    public class ScriptableObjectDrawer : PropertyDrawer
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
                    DrawSettingsEditor(property.objectReferenceValue, null, ref foldout, ref editor);
                    EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            EditorGUI.EndProperty();
        }

        private void DrawSettingsEditor(Object @object, System.Action action, ref bool foldout, ref Editor editor)
        {
            if (@object == null) return;

            EditorGUILayout.BeginVertical();
            foldout = EditorGUILayout.InspectorTitlebar(foldout, @object);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    EditorGUI.indentLevel++;
                    Editor.CreateCachedEditor(@object, null, ref editor);
                    editor.DrawDefaultInspector();

                    if (check.changed)
                    {
                        if (action != null)
                            action();
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}