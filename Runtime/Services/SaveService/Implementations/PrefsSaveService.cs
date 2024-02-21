using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("SaveService.Zenject")]

namespace Services.SaveService.Implementations
{
    internal class PrefsSaveService : ISaveService
    {
        private readonly Dictionary<Type, string> m_cachedMap;

        public PrefsSaveService(params ISavesMap[] savesMaps)
        {
            m_cachedMap = new Dictionary<Type, string>();
            foreach (ISavesMap savesMap in savesMaps)
                AddSavesMap(savesMap);
        }

        public void Save<T>(T data, Func<T, string> customDataFormatter = null) where T : class
        {
            if (m_cachedMap.TryGetValue(typeof(T), out string key))
            {
                string textData = customDataFormatter != null ? customDataFormatter(data) : JsonUtility.ToJson(data);
                PlayerPrefs.SetString(key, textData);
            }
            else
            {
                Debug.LogError($"[SAVE] There is no Key for given type {typeof(T)}");
            }
        }

        public T Load<T>(Func<T> newInstanceFunc = null, Func<string, T> customDataFormatter = null) where T : class
        {
            if (m_cachedMap.TryGetValue(typeof(T), out string key))
            {
                if (!PlayerPrefs.HasKey(key))
                    return Internal_TryCreateInstance(newInstanceFunc);

                string textData = PlayerPrefs.GetString(key);
                T data = customDataFormatter != null ? customDataFormatter(textData) : JsonUtility.FromJson<T>(textData);
                return data;
            }

            Debug.LogError($"[SAVE] There is no Key for given type {typeof(T)}");
            return Internal_TryCreateInstance(newInstanceFunc);
        }

        private T Internal_TryCreateInstance<T>(Func<T> newInstanceFunc = null) where T : class
        {
            Debug.Log($"Creating new instance of type {typeof(T)}");
            if (newInstanceFunc != null)
                return newInstanceFunc();
            if (typeof(T).GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance<T>();
            Debug.LogError($"Can't create new instance of type {typeof(T)}");
            return null;
        }

        public void AddSavesMap(ISavesMap savesMap)
        {
            foreach (KeyValuePair<Type, string> keyValuePair in savesMap.Map)
                AddSaveMap(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddSaveMap(Type type, string key)
        {
            m_cachedMap.TryAdd(type, key);
        }
    }
}