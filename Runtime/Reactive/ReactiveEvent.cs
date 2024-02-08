using System;
using System.Linq;
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

    [Serializable]
    public class ReactiveEvent
    {
        private event Action OnInvoke;

        public ReactiveEvent()
        {
            OnInvoke = default;
        }

        public void AddListener(Action action, bool forceSubscribe = false)
        {
            if (forceSubscribe)
                OnInvoke += action;
            else if (OnInvoke != null && !OnInvoke.GetInvocationList().Contains(action))
                OnInvoke += action;
            else if (OnInvoke == null)
                OnInvoke += action;
        }

        public void RemoveListener(Action action)
        {
            OnInvoke -= action;
        }

        public virtual void Invoke()
        {
            OnInvoke?.Invoke();
        }
    }
}