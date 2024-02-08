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
            EditorGUILayout.PropertyField(property.FindPropertyRelative("_key"));
            SerializedProperty randomProperty = property.FindPropertyRelative("_randomClip");
            EditorGUILayout.PropertyField(randomProperty);
            bool random = randomProperty.boolValue;
            string fieldName = random ? "_randomClips" : "_clip";
            EditorGUILayout.PropertyField(property.FindPropertyRelative(fieldName));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0f;
    }
}