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
            SerializedProperty useReference = property.FindPropertyRelative("_useReference");
            string propertyName = useReference.boolValue ? "_value1" : "_value2";

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(label);
                    GUIStyle style = new(GUI.skin.label)
                    {
                        padding = new RectOffset(10, 10, 0, 0),
                        alignment = TextAnchor.MiddleRight
                    };
                    EditorGUILayout.LabelField("Use Reference", style);
                    EditorGUILayout.PropertyField(useReference, GUIContent.none);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                string valueLabel = useReference.boolValue ? "Reference Value" : "Override Value";
                EditorGUILayout.PropertyField(property.FindPropertyRelative(propertyName), new GUIContent(valueLabel));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}