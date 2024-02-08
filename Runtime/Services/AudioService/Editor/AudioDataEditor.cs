using Services.AudioService.Datas;
using UnityEditor;
using UnityEngine;

namespace Services.AudioService.Editor
{
    [CustomPropertyDrawer(typeof(AudioData))]
    public class AudioDataEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty randomProperty = property.FindPropertyRelative("_randomClip");
            bool random = randomProperty.boolValue;
            string fieldName = random ? "_randomClips" : "_clip";
            SerializedProperty valueProperty = property.FindPropertyRelative(fieldName);

            Rect keyRect = position;
            keyRect.height = EditorGUIUtility.singleLineHeight;
            
            Rect useRandomRect = position;
            useRandomRect.height = EditorGUIUtility.singleLineHeight;
            useRandomRect.y += EditorGUIUtility.singleLineHeight;
            
            Rect valueField = position;
            valueField.height = EditorGUI.GetPropertyHeight(valueProperty);
            valueField.y += EditorGUIUtility.singleLineHeight * 2;
            
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("_key"));
            EditorGUI.PropertyField(useRandomRect, property.FindPropertyRelative("_randomClip"));
            EditorGUI.PropertyField(valueField, valueProperty, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty randomProperty = property.FindPropertyRelative("_randomClip");
            bool random = randomProperty.boolValue;
            string fieldName = random ? "_randomClips" : "_clip";
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(fieldName)) + EditorGUIUtility.singleLineHeight * 2f;
        }
    }
}