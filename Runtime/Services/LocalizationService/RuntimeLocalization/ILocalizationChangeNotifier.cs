using System;

namespace LocalizationService.RuntimeLocalization
{
    public interface ILocalizationChangeNotifier : IDisposable
    {
        public event Action Changed;
        void Initialize();
    }
}