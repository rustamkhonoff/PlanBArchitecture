using System;

namespace Services.AudioService
{
    public interface IAudioServiceStateHelper
    {
        public Func<bool> DebugEnabled { get; }
        public Func<bool> SoundEnabled { get; }
        public Func<bool> BackgroundEnabled { get; }
    }
}