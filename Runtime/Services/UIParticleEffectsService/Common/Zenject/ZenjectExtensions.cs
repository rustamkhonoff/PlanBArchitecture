#if ZENJECT
using UnityEngine;
using Zenject;

namespace Services.UIParticleEffectsService.Common.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUIParticlesService(this DiContainer diContainer)
        {
            diContainer
                .Bind<IUIParticleEffectsService>()
                .To<Implementation.UIParticleEffectsService>()
                .AsSingle()
                .WithArguments(Resources.Load("StaticData/UI/UIParticlesEffectsConfiguration"))
                .NonLazy();
        }
    }
}
#endif