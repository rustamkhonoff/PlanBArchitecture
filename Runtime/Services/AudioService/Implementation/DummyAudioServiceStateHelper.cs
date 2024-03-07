using System;

namespace AudioService.Implementation
{
    internal class DummyAudioServiceStateHelper : IAudioServiceStateHelper
    {
        public event Action<bool> BackgroundStateChanged;
        public event Action<bool> SoundStateChanged;
        public event Action<float> BackgroundVolumeChanged;
        public event Action<float> SoundVolumeChanged;
        public Func<float> SoundVolume => () => 1f;
        public Func<float> BackgroundVolume => () => 1f;
        public Func<bool> DebugEnabled => () => true;
        public Func<bool> SoundEnabled => () => true;
        public Func<bool> BackgroundEnabled => () => true;
    }
}