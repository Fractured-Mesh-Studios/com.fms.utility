using UnityEditor;
using UnityEngine;
using UtilityEngine;

[CustomPropertyDrawer(typeof(HelpBoxAttribute))]
public class HelpBoxDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HelpBoxAttribute helpBox = (HelpBoxAttribute)attribute;
        float helpBoxHeight = EditorGUIUtility.singleLineHeight * 2.5f;
        float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
        return helpBoxHeight + propertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HelpBoxAttribute helpBox = (HelpBoxAttribute)attribute;

        // Rects
        Rect helpBoxRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 2.5f);
        Rect propertyRect = new(position.x, position.y + helpBoxRect.height, position.width, EditorGUI.GetPropertyHeight(property, label, true));

        // Draw HelpBox
        MessageType unityMsgType = MessageType.Info;
        switch (helpBox.Type)
        {
            case HelpBoxType.Warning: unityMsgType = MessageType.Warning; break;
            case HelpBoxType.Error: unityMsgType = MessageType.Error; break;
        }

        EditorGUI.HelpBox(helpBoxRect, helpBox.Message, unityMsgType);

        // Draw property field
        EditorGUI.PropertyField(propertyRect, property, label, true);
    }
}
