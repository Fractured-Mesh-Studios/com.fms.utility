using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(EnumStringAttribute))]
    public class EnumStringAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var stringEnum = (EnumStringAttribute)attribute;

            string[] options = (stringEnum.options != null && stringEnum.options.Length > 0)
                ? stringEnum.options
                : new string[] { "None" };

            // Buscar el índice actual
            int currentIndex = System.Array.IndexOf(options, property.stringValue);

            // Si no se encuentra el valor actual, usar la primera opción
            if (currentIndex < 0)
                currentIndex = 0;

            // Dibujar popup
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

            // Asignar valor seleccionado
            property.stringValue = options[selectedIndex];
        }
    }
}
