using System;

namespace Services.AudioService.Implementation
{
    public class DummyAudioServiceStateHelper : IAudioServiceStateHelper
    {
        public Func<bool> DebugEnabled => () => true;
        public Func<bool> SoundEnabled => () => true;
        public Func<bool> BackgroundEnabled => () => true;
    }
}