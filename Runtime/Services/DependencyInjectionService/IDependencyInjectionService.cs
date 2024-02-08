using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.DependencyInjectionService
{
    public interface IDependencyInjectionService
    {
        public T Resolve<T>();
        public void Inject(object o);
        public T Create<T>();
        public object Create(Type type);
        public GameObject InstantiatePrefab(GameObject prefab);
        public T InstantiatePrefabForComponent<T>(T prefab) where T : Object;
        public T InstantiatePrefabForComponent<T>(T prefab, Transform parent) where T : Object;
        public void Update(object container);
    }
}