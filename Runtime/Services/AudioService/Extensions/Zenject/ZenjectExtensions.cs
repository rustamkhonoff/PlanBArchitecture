#if ZENJECT
using System;
using AudioService.Implementation;
using UnityEngine;
using Zenject;

namespace AudioService.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        private const string DefaultAudioStaticData = "StaticData/Audio/AudioServiceStaticData";

        public static void AddSoundService(this DiContainer diContainer, string staticDataPath = DefaultAudioStaticData)
        {
            diContainer.AddSoundService<DummyAudioServiceStateHelper>(staticDataPath);
        }

        public static void AddSoundService<T>(this DiContainer diContainer, string staticDataPath = DefaultAudioStaticData)
            where T : IAudioServiceStateHelper
        {
            diContainer
                .BindInterfacesAndSelfTo(typeof(T))
                .AsSingle();

            diContainer
                .Bind<IAudioService>()
                .To<AudioService.Implementation.AudioService>()
                .AsSingle()
                .WithArguments(Resources.Load(staticDataPath))
                .NonLazy();
        }
    }
}
#endif