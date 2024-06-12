using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Services.AssetProviderService
{
    public class ResourcesAssetProvider : IAssetProvider
    {
#if ADDRESSABLES
        public Task<T> Load<T>(AssetReference assetReference) where T : Object
        {
            throw new NotImplementedException("To use AssetReference use Addressables implementation");
        }

        public Task<T> LoadAsComponent<T>(AssetReference assetReference) where T : Component
        {
            throw new NotImplementedException("To use AssetReference use Addressables implementation");
        }
#endif

        public async Task<T> Load<T>(string key) where T : Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(key);
            await Task.Yield();
            while (!resourceRequest.isDone) await Task.Yield();
            return resourceRequest.asset as T;
        }

        public async Task<T> LoadAsComponent<T>(string key) where T : Component
        {
            GameObject prefab = await Load<GameObject>(key);
            return prefab.GetComponent<T>();
        }

        public void Release()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}