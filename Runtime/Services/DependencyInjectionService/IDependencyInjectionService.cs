using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DependencyInjectionService
{
    public interface IDependencyInjectionService
    {
        T Resolve<T>();
        void Inject(object o);
        T Create<T>();
        object Create(Type type);
        GameObject InstantiatePrefab(GameObject prefab);
        GameObject InstantiatePrefab(GameObject prefab, Transform parent);
        T InstantiatePrefabForComponent<T>(T prefab) where T : Object;
        T InstantiatePrefabForComponent<T>(T prefab, Transform parent) where T : Object;
        void Update(object container);
        void RegisterInstantiateHandler<T>(Action<T> onInstantiated);
        void RegisterAnyInstantiateHandler(Action<object> onInstantiated);
        void UnRegisterInstantiateHandler<T>(Action<T> onInstantiated);
        void UnRegisterAnyInstantiateHandler(Action<object> onInstantiated);
    }
}