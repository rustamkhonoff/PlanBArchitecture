using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Patterns.Pool
{
    public abstract class PoolBase
    {
        public abstract object Get();
        public abstract void ClearPool();
    }

    public sealed class PoolDictionary
    {
        private readonly Dictionary<Type, PoolBase> m_pools = new();

        public T Get<T>()
        {
            Type type = typeof(T);
            if (!m_pools.ContainsKey(type))
                throw new ArgumentException("Invalid type requested from pool,there is no pool with given type");

            PoolBase pool = m_pools[type];
            return (T)pool.Get();
        }

        public void AddPool<T>(PoolBase poolBase)
        {
            AddPool(typeof(T), poolBase);
        }

        public void AddPool(Type type, object poolObj)
        {
            if (poolObj is PoolBase poolBase)
            {
                if (m_pools.TryAdd(type, poolBase))
                    return;
                Debug.Log($"There is already pool for type {type}");
            }
            else
            {
                throw new InvalidCastException("Trying to add invalid pool");
            }
        }

        public void AddPool<T>(Func<T> createFunc, int warmupAmount = 0, Action<T> onGet = null,
            Func<T, bool> validation = null)
        {
            Type type = typeof(T);
            if (type.IsSubclassOf(typeof(Component)))
            {
                Type genericType = typeof(ComponentPool<>).MakeGenericType(typeof(T));
                AddPool(type, Activator.CreateInstance(genericType, createFunc, warmupAmount, onGet, validation));
            }
            else if (type == typeof(GameObject))
            {
                AddPool(type,
                    Activator.CreateInstance(typeof(GameObjectPool), createFunc, warmupAmount, onGet, validation));
            }
            else if (type == typeof(Transform))
            {
                AddPool(type,
                    Activator.CreateInstance(typeof(TransformPool), createFunc, warmupAmount, onGet, validation));
            }
            else
            {
                throw new Exception($"Can't create Pool for type {type}");
            }
        }
    }


    public abstract class PoolWrapper<T> : PoolBase
    {
        private readonly Func<T> m_createFunc;
        private readonly Action<T> m_onGet;
        private readonly Func<T, bool> m_validation;
        protected readonly List<T> m_cache;
        private readonly int m_warmupAmount;

        public PoolWrapper(Func<T> createFunc, int warmupAmount = 0, Action<T> onGet = null,
            Func<T, bool> validation = null)
        {
            m_warmupAmount = warmupAmount;
            m_createFunc = createFunc;
            m_onGet = onGet;
            m_cache = new List<T>();
            m_validation = validation ?? DefaultValidation;

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < m_warmupAmount; i++)
            {
                T instance = m_createFunc();
                DisableInstance(instance);
                m_cache.Add(instance);
            }
        }

        public override object Get()
        {
            return GetFromWrapper();
        }

        protected virtual void EnableInstance(T instance)
        {
        }

        protected virtual void DisableInstance(T instance)
        {
        }

        private T GetFromWrapper()
        {
            T result;
            if (m_cache.Find(Check) is { } found)
            {
                result = found;
                EnableInstance(result);
            }
            else
            {
                result = m_createFunc();
                m_cache.Add(result);
            }

            m_onGet?.Invoke(result);
            return result;
        }

        private bool Check(T obj)
        {
            return m_validation(obj);
        }

        protected virtual bool DefaultValidation(T instance)
        {
            return true;
        }
    }

    public class TransformPool : PoolWrapper<Transform>
    {
        public TransformPool(Func<Transform> createFunc, int warmupAmount = 0, Action<Transform> onGet = null,
            Func<Transform, bool> validation = null) : base(createFunc, warmupAmount, onGet, validation)
        {
        }

        public new Transform Get()
        {
            return (Transform)base.Get();
        }

        public override void ClearPool()
        {
            for (int i = m_cache.Count - 1; i >= 0; i--)
                Object.Destroy(m_cache[i].gameObject);
            m_cache.Clear();
        }

        protected override void EnableInstance(Transform instance)
        {
            instance.gameObject.SetActive(true);
        }

        protected override void DisableInstance(Transform instance)
        {
            instance.gameObject.SetActive(false);
        }

        protected override bool DefaultValidation(Transform instance)
        {
            return instance.gameObject.activeSelf == false;
        }
    }

    public class GameObjectPool : PoolWrapper<GameObject>
    {
        public GameObjectPool(Func<GameObject> createFunc, int warmupAmount = 0, Action<GameObject> onGet = null,
            Func<GameObject, bool> validation = null) : base(createFunc, warmupAmount, onGet, validation)
        {
        }

        public new GameObject Get()
        {
            return (GameObject)base.Get();
        }

        public override void ClearPool()
        {
            for (int i = m_cache.Count - 1; i >= 0; i--)
                Object.Destroy(m_cache[i].gameObject);
            m_cache.Clear();
        }

        protected override void DisableInstance(GameObject instance)
        {
            instance.SetActive(false);
        }

        protected override void EnableInstance(GameObject instance)
        {
            instance.SetActive(true);
        }

        protected override bool DefaultValidation(GameObject instance)
        {
            return instance.activeSelf == false;
        }
    }

    public class ComponentPool<T> : PoolWrapper<T> where T : Component
    {
        public ComponentPool(Func<T> createFunc, int warmupAmount = 0, Action<T> onGet = null,
            Func<T, bool> validation = null) : base(createFunc,
            warmupAmount, onGet, validation)
        {
        }

        protected override void DisableInstance(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        protected override void EnableInstance(T instance)
        {
            instance.gameObject.SetActive(true);
        }

        protected override bool DefaultValidation(T instance)
        {
            return instance.gameObject.activeSelf == false;
        }

        public override void ClearPool()
        {
            for (int i = m_cache.Count - 1; i >= 0; i--)
                Object.Destroy(m_cache[i].gameObject);
            m_cache.Clear();
        }
    }
}