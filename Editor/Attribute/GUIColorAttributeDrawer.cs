using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(GUIColorAttribute))]
    public class GUIColorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Obtener el atributo
            GUIColorAttribute colorField = (GUIColorAttribute)attribute;

            // Guardar el color original del GUI
            Color previousColor = GUI.color;

            // Cambiar el color del GUI al especificado en el atributo
            GUI.color = colorField.color;

            // Dibujar el campo en el inspector
            EditorGUI.PropertyField(position, property, label);

            // Restaurar el color original del GUI
            GUI.color = previousColor;
        }
    }
}
