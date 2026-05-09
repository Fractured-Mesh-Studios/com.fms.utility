using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UtilityEngine;

namespace UtilityEditor
{
    [CustomPropertyDrawer(typeof(AnimatorStateAttribute))]
    public class AnimatorStateAttributeDrawer : PropertyDrawer
    {
        private string[] stateNames;
        private int selectedIndex;
        private const float HelpBoxHeight = 40f;
        private const float FieldHeight = 18f;
        private const float Spacing = 2f;
        private const float ToggleButtonWidth = 30f;

        // Almacenar estado de modo por propiedad
        private static Dictionary<string, bool> propertyModes = new Dictionary<string, bool>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return HelpBoxHeight + FieldHeight + Spacing;

            var attribute = (AnimatorStateAttribute)this.attribute;
            Animator animator = GetAnimatorFromTarget(property, attribute.animatorFieldName);

            if (animator == null || !HasValidStates(animator))
                return HelpBoxHeight + FieldHeight + Spacing;

            return FieldHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                Rect helpBoxRect = new Rect(position.x, position.y, position.width, HelpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, "AnimatorStateAttribute only works with string properties.", MessageType.Error);
                
                Rect fieldRect = new Rect(position.x, position.y + HelpBoxHeight + Spacing, position.width, FieldHeight);
                EditorGUI.PropertyField(fieldRect, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            var attribute = (AnimatorStateAttribute)this.attribute;
            Animator animator = GetAnimatorFromTarget(property, attribute.animatorFieldName);
            
            string propertyKey = property.propertyPath;
            if (!propertyModes.ContainsKey(propertyKey))
                propertyModes[propertyKey] = true;

            bool useDropdown = propertyModes[propertyKey];

            if (animator == null)
            {
                useDropdown = false;

                Rect helpBoxRect = new Rect(position.x, position.y, position.width, HelpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, "Animator not found in the target or its hierarchy.", MessageType.Warning);
                
                Rect fieldRect = new Rect(position.x, position.y + HelpBoxHeight + Spacing, position.width, FieldHeight);
                EditorGUI.PropertyField(fieldRect, property, label);
                EditorGUI.EndProperty();
                return;
            }

            stateNames = GetAnimatorStates(animator);

            if (stateNames == null || stateNames.Length == 0)
            {
                useDropdown = false;

                Rect helpBoxRect = new Rect(position.x, position.y, position.width, HelpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, "The Animator has no states defined.", MessageType.Info);
                
                Rect fieldRect = new Rect(position.x, position.y + HelpBoxHeight + Spacing, position.width, FieldHeight);
                EditorGUI.PropertyField(fieldRect, property, label);
                EditorGUI.EndProperty();
                return;
            }

            // Layout with toggle button
            Rect fieldRect2 = new Rect(position.x, position.y, position.width - ToggleButtonWidth - Spacing, FieldHeight);
            Rect toggleButtonRect = new Rect(position.x + position.width - ToggleButtonWidth, position.y, ToggleButtonWidth, FieldHeight);

            if (useDropdown)
            {
                selectedIndex = System.Array.IndexOf(stateNames, property.stringValue);
                if (selectedIndex < 0)
                    selectedIndex = 0;

                int newIndex = EditorGUI.Popup(fieldRect2, label.text, selectedIndex, stateNames);

                if (newIndex != selectedIndex || string.IsNullOrEmpty(property.stringValue))
                {
                    property.stringValue = stateNames[newIndex];
                }
            }
            else
            {
                EditorGUI.PropertyField(fieldRect2, property, label);
            }

            // Toggle button
            string buttonLabel = useDropdown ? "◆" : "✎";
            if (GUI.Button(toggleButtonRect, buttonLabel, EditorStyles.miniButton))
            {
                propertyModes[propertyKey] = !useDropdown;
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Obtiene el Animator del target o su jerarquía.
        /// </summary>
        private Animator GetAnimatorFromTarget(SerializedProperty property, string fieldName)
        {
            var targetObject = property.serializedObject.targetObject;
            
            if (targetObject == null)
                return null;

            // Si es un Component, buscar en su jerarquía
            if (targetObject is Component component)
            {
                return FindAnimatorInHierarchy(component.gameObject);
            }

            return null;
        }

        /// <summary>
        /// Busca Animator en el target y su jerarquía (padres e hijos).
        /// </summary>
        private Animator FindAnimatorInHierarchy(GameObject target)
        {
            if (target == null)
                return null;

            // Buscar en el mismo objeto
            Animator animator = target.GetComponent<Animator>();
            if (animator != null)
                return animator;

            // Buscar en los padres
            Transform parent = target.transform.parent;
            while (parent != null)
            {
                animator = parent.GetComponent<Animator>();
                if (animator != null)
                    return animator;
                parent = parent.parent;
            }

            // Buscar en los hijos
            animator = target.GetComponentInChildren<Animator>();
            if (animator != null)
                return animator;

            return null;
        }

        /// <summary>
        /// Verifica si el Animator tiene estados válidos.
        /// </summary>
        private bool HasValidStates(Animator animator)
        {
            if (animator == null)
                return false;

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            if (controller == null)
                return false;

            AnimatorController animatorController = controller as AnimatorController;
            return animatorController != null && animatorController.layers.Length > 0;
        }

        /// <summary>
        /// Obtiene todos los nombres de estados del Animator.
        /// </summary>
        private string[] GetAnimatorStates(Animator animator)
        {
            if (animator == null)
                return new string[] { };

            List<string> stateNames = new List<string>();

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            if (controller == null)
                return new string[] { };

            AnimatorController animatorController = controller as AnimatorController;
            if (animatorController == null)
                return new string[] { };

            foreach (AnimatorControllerLayer layer in animatorController.layers)
            {
                if (layer.stateMachine == null)
                    continue;

                CollectStates(layer.stateMachine, stateNames);
            }

            return stateNames.ToArray();
        }

        /// <summary>
        /// Recolecta recursivamente todos los nombres de estados.
        /// </summary>
        private void CollectStates(AnimatorStateMachine stateMachine, List<string> stateNames)
        {
            if (stateMachine == null)
                return;

            foreach (ChildAnimatorState state in stateMachine.states)
            {
                if (state.state != null && !string.IsNullOrEmpty(state.state.name))
                {
                    if (!stateNames.Contains(state.state.name))
                        stateNames.Add(state.state.name);
                }
            }

            foreach (ChildAnimatorStateMachine subMachine in stateMachine.stateMachines)
            {
                if (subMachine.stateMachine != null)
                    CollectStates(subMachine.stateMachine, stateNames);
            }
        }
    }
}
