using System;

namespace AudioService
{
    public interface IAudioServiceStateHelper : IDisposable
    {
        public event Action<bool> BackgroundStateChanged;
        public event Action<bool> SoundStateChanged;
        public event Action<float> BackgroundVolumeChanged;
        public event Action<float> SoundVolumeChanged;
        public Func<float> SoundVolume { get; }
        public Func<float> BackgroundVolume { get; }
        public Func<bool> DebugEnabled { get; }
        public Func<bool> SoundEnabled { get; }
        public Func<bool> BackgroundEnabled { get; }
    }
}