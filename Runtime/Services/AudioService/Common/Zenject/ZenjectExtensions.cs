using Services.AudioService.Implementation;
using UnityEngine;
using Zenject;

namespace Services.AudioService.Common.Zenject
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
                .To<Implementation.AudioService>()
                .AsSingle()
                .WithArguments(Resources.Load("StaticData/Audio/AudioServiceStaticData"))
                .NonLazy();
        }
    }
}