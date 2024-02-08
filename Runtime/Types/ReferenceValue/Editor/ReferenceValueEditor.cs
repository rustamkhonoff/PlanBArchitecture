using Common.ReferenceValue;
using UnityEditor;
using UnityEngine;

namespace Packages.PlanBArchitecture.Runtime.Types.ReferenceValue.Editor
{
    [CustomPropertyDrawer(typeof(ReferenceValue<,>))]
    [CustomPropertyDrawer(typeof(ReferenceValue<,>), true)]
    public class ReferenceValueEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            const int padding = 5;


            SerializedProperty useReference = property.FindPropertyRelative("_useReference");
            string propertyName = useReference.boolValue ? "_value1" : "_value2";
            SerializedProperty propertyValue = property.FindPropertyRelative(propertyName);

            Rect labelRect = position;
            labelRect.width = position.width / 2f;
            labelRect.height = EditorGUIUtility.singleLineHeight;

            Rect useReferenceRect = position;
            useReferenceRect.width = 0;
            useReferenceRect.x = position.width - padding;
            useReferenceRect.height = EditorGUIUtility.singleLineHeight;

            Rect valueRect = position;
            valueRect.height = EditorGUI.GetPropertyHeight(propertyValue);
            valueRect.y += EditorGUIUtility.singleLineHeight;
            valueRect.width -= padding;

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(10, 10, 0, 0)
            };
            Rect indentedRect = EditorGUI.IndentedRect(position);

            indentedRect.x -= padding;
            indentedRect.width += padding;
            indentedRect.height -= padding * 2;

            EditorGUI.HelpBox(indentedRect, "", MessageType.None);

            EditorGUI.LabelField(labelRect, label);

            Rect useReferenceLabelRect = new(useReferenceRect.x - 100, useReferenceRect.y, 100, useReferenceRect.height);
            EditorGUI.LabelField(useReferenceLabelRect, "Use Reference", style);
            EditorGUI.PropertyField(useReferenceRect, useReference, GUIContent.none);

            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(valueRect, propertyValue, true);
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty useReference = property.FindPropertyRelative("_useReference");
            string propertyName = useReference.boolValue ? "_value1" : "_value2";
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(propertyName)) + EditorGUIUtility.singleLineHeight * 2f;
        }
    }
}