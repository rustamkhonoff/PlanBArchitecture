using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.AssetProviderService
{
    public interface IAssetProvider
    {
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string key) where T : class;
        Task<T> LoadAsComponent<T>(AssetReference assetReference) where T : Component;
        Task<T> LoadAsComponent<T>(string key) where T : Component;
        void Release();
    }

    public class AddressableAssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> m_completedCache;
        private readonly Dictionary<string, List<AsyncOperationHandle>> m_handlers;

        public AddressableAssetProvider()
        {
            m_completedCache = new Dictionary<string, AsyncOperationHandle>();
            m_handlers = new Dictionary<string, List<AsyncOperationHandle>>();
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            return await Load<T>(assetReference.AssetGUID);
        }


        public async Task<T> Load<T>(string key) where T : class
        {
            if (m_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandler))
                return completedHandler.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            handle.Completed += h => m_completedCache[key] = h;

            if (!m_handlers.TryGetValue(key, out List<AsyncOperationHandle> handles))
            {
                handles = new List<AsyncOperationHandle>();
                m_handlers[key] = handles;
            }

            handles.Add(handle);

            return await handle.Task;
        }

        public async Task<T> LoadAsComponent<T>(AssetReference assetReference) where T : Component
        {
            GameObject prefab = await Load<GameObject>(assetReference);
            return prefab.GetComponent<T>();
        }

        public async Task<T> LoadAsComponent<T>(string key) where T : Component
        {
            GameObject prefab = await Load<GameObject>(key);
            return prefab.GetComponent<T>();
        }

        public void Release()
        {
            foreach (AsyncOperationHandle asyncOperationHandle in m_handlers.Values.SelectMany(item => item))
                Addressables.Release(asyncOperationHandle);

            m_completedCache.Clear();
            m_handlers.Clear();
        }
    }
}