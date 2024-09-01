using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(EnumAttribute))]
    public class EnumSearchableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                EditorGUI.BeginProperty(position, label, property);

                // Obtener las propiedades del atributo
                EnumAttribute enumAttribute = (EnumAttribute)attribute;

                // Determinar el tamaño del label y del dropdown
                Rect labelRect, dropdownRect;

                if (enumAttribute.showLabel)
                {
                    // Mostrar el label si está habilitado
                    labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
                    dropdownRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);
                    EditorGUI.LabelField(labelRect, label);
                }
                else
                {
                    // Ajustar la posición del dropdown si el label está desactivado
                    labelRect = new Rect();
                    dropdownRect = position;
                }

                // Botón de enum desplegable
                if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(property.enumDisplayNames[property.enumValueIndex]), FocusType.Keyboard))
                {
                    // Pasar las propiedades del atributo al popup
                    EnumPopup.Show(dropdownRect, property, enumAttribute.minHeight, enumAttribute.maxHeight);
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [Enum] Attribute with 'Enum' properties only.");
            }
        }
    }

    public class EnumPopup : EditorWindow
    {
        private static SerializedProperty s_currentProperty;
        private static string[] s_enumNames;
        private string m_filter = "";
        private Vector2 m_scrollPosition;
        private static EnumPopup s_window;
        private static Texture2D s_borderTexture;
        private static Texture2D s_separatorTexture;

        private static float s_minHeight;
        private static float s_maxHeight;
        private const float k_itemHeight = 20f; // Altura de cada item en la lista
        private const float k_separatorHeight = 1f;
        private const float k_separatorMargin = 5f;
        private float m_borderThickness = 2f;

        public static void Show(Rect rect, SerializedProperty property, float minHeightParam, float maxHeightParam)
        {
            s_currentProperty = property;
            s_enumNames = property.enumNames;
            s_minHeight = minHeightParam;
            s_maxHeight = maxHeightParam;

            // Crear y mostrar la ventana del popup justo debajo del rectángulo del enum en el inspector
            s_window = CreateInstance<EnumPopup>();

            // Calcular la altura basada en el número de elementos filtrados
            float filteredItemCount = GetFilteredItemCount();
            float calculatedHeight = Mathf.Clamp(filteredItemCount * k_itemHeight + k_separatorHeight + 2 * k_separatorMargin, s_minHeight, s_maxHeight);

            // Convertir la posición de rect en coordenadas de pantalla
            Rect screenRect = new Rect(GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y)), rect.size);

            // Calcular la posición del popup debajo del rectángulo del campo del enum
            s_window.ShowAsDropDown(screenRect, new Vector2(rect.width, calculatedHeight));

            // Crear una textura para el borde si no existe
            if (s_borderTexture == null)
            {
                s_borderTexture = new Texture2D(1, 1);
                s_borderTexture.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.12f)); // Color del borde
                s_borderTexture.Apply();
            }

            // Crear una textura para la línea separadora si no existe
            if (s_separatorTexture == null)
            {
                s_separatorTexture = new Texture2D(1, 1);
                s_separatorTexture.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f)); // Color tenue para la línea separadora
                s_separatorTexture.Apply();
            }
        }

        private static float GetFilteredItemCount()
        {
            int count = 0;
            for (int i = 0; i < s_enumNames.Length; i++)
            {
                if (string.IsNullOrEmpty(s_window.m_filter) || s_enumNames[i].ToLower().Contains(s_window.m_filter.ToLower()))
                {
                    count++;
                }
            }
            return count;
        }

        private void OnGUI()
        {
            // Área total del popup
            Rect popupRect = new Rect(0, 0, position.width, position.height);

            // Dibujar el borde del popup
            DrawBorder(popupRect, m_borderThickness);

            // Ajustar el área de la ScrollView para dejar espacio para el borde
            Rect scrollViewRect = new Rect(m_borderThickness, m_borderThickness, position.width - 2 * m_borderThickness, position.height - 2 * m_borderThickness);

            // Añadir un margen interior
            GUILayout.BeginArea(scrollViewRect);
            GUILayout.Space(k_separatorMargin);

            // Campo de texto para el filtro
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter:", GUILayout.Width(50));
            m_filter = EditorGUILayout.TextField(m_filter);
            EditorGUILayout.EndHorizontal();

            // Dibujar la línea separadora con margen adicional
            Rect separatorRect = new Rect(0, GUILayoutUtility.GetLastRect().yMax + k_separatorMargin, position.width, k_separatorHeight);
            GUI.DrawTexture(separatorRect, s_separatorTexture);

            // Espacio adicional después de la línea separadora
            GUILayout.Space(k_separatorMargin);

            // Scroll view para la lista de opciones
            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            // Mostrar las opciones filtradas
            for (int i = 0; i < s_enumNames.Length; i++)
            {
                string enumName = s_enumNames[i];

                if (string.IsNullOrEmpty(m_filter) || enumName.ToLower().Contains(m_filter.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal();

                    // Mostrar el icono de verificación si la opción está seleccionada
                    if (i == s_currentProperty.enumValueIndex)
                    {
                        GUILayout.Label("✔", GUILayout.Width(20)); // Usar un símbolo de verificación
                    }
                    else
                    {
                        GUILayout.Label("", GUILayout.Width(20)); // Espacio en blanco para opciones no seleccionadas
                    }

                    // Botón para seleccionar el enum
                    GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        alignment = TextAnchor.MiddleLeft // Alinear el texto hacia la izquierda
                    };
                    if (GUILayout.Button(enumName, buttonStyle))
                    {
                        s_currentProperty.enumValueIndex = i;
                        s_currentProperty.serializedObject.ApplyModifiedProperties();
                        Close();
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawBorder(Rect rect, float thickness)
        {
            // Dibuja un borde alrededor del rectángulo especificado
            Rect topBorder = new Rect(rect.x, rect.y, rect.width, thickness);
            Rect bottomBorder = new Rect(rect.x, rect.y + rect.height - thickness, rect.width, thickness);
            Rect leftBorder = new Rect(rect.x, rect.y, thickness, rect.height);
            Rect rightBorder = new Rect(rect.x + rect.width - thickness, rect.y, thickness, rect.height);

            GUI.DrawTexture(topBorder, s_borderTexture);
            GUI.DrawTexture(bottomBorder, s_borderTexture);
            GUI.DrawTexture(leftBorder, s_borderTexture);
            GUI.DrawTexture(rightBorder, s_borderTexture);
        }

        private void OnLostFocus()
        {
            // Cerrar la ventana del popup si pierde el foco
            Close();
        }
    }
}