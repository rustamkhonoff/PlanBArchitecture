using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Reactive
{
    [CustomPropertyDrawer(typeof(ReactiveEvent))]
    public class ReactiveEventEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
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
        }
    }
}