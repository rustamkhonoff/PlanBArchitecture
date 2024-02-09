using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Reactive
{
    [CustomPropertyDrawer(typeof(ReactiveVariable<>))]
    public sealed class ReactiveVariableEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(label);

                    if (GUILayout.Button("Invoke"))
                    {
                        object reactiveProperty = fieldInfo.GetValue(property.serializedObject.targetObject);
                        MethodInfo notifyMethod = reactiveProperty.GetType().GetMethod("Invoke");
                        notifyMethod?.Invoke(reactiveProperty, null);
                    }
                }
                EditorGUILayout.EndHorizontal();

                SerializedProperty foundProperty = property.FindPropertyRelative("_value");

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(foundProperty, new GUIContent("Value"), true);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}