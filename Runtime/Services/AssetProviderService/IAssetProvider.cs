using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Services.AssetProviderService
{
    public interface IAssetProvider
    {
#if ADDRESSABLES
        Task<T> Load<T>(AssetReference assetReference) where T : Object;
        Task<T> LoadAsComponent<T>(AssetReference assetReference) where T : Component;
#endif
        Task<T> Load<T>(string key) where T : Object;
        Task<T> LoadAsComponent<T>(string key) where T : Component;
        void Release();
    }
}