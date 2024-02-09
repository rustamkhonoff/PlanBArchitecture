using System;

namespace Services.AudioService
{
    public interface IAudioServiceStateHelper
    {
        public event Action<bool> BackgroundStateChanged;
        public event Action<bool> SoundStateChanged;
        public event Action<float> BackgroundVolumeChanged;
        public event Action<float> SoundVolumeChanged;
        public Func<bool> DebugEnabled { get; }
        public Func<bool> SoundEnabled { get; }
        public Func<bool> BackgroundEnabled { get; }
    }
}