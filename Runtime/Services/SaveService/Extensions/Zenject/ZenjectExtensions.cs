#region ZENJECT

using Services.SaveService.Implementations;
using Zenject;

namespace Services.SaveService.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddSaveService(this DiContainer container, params ISavesMap[] savesMap)
        {
            container
                .Bind<ISaveService>()
                .To<PrefsSaveService>()
                .AsSingle()
                .WithArguments(savesMap);
        }
    }
}

#endregion