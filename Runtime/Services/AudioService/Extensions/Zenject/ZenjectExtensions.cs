#if ZENJECT
using AudioService.Implementation;
using UnityEngine;
using Zenject;

namespace AudioService.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddSoundService(this DiContainer diContainer)
        {
            diContainer.AddSoundService<DummyAudioServiceStateHelper>();
        }

        public static void AddSoundService<T>(this DiContainer diContainer) where T : IAudioServiceStateHelper
        {
            diContainer
                .Bind<IAudioServiceStateHelper>()
                .To<T>()
                .AsSingle();

            diContainer
                .Bind<IAudioService>()
                .To<global::AudioService.Implementation.AudioService>()
                .AsSingle()
                .WithArguments(Resources.Load("StaticData/Audio/AudioServiceStaticData"))
                .NonLazy();
        }
    }
}
#endif