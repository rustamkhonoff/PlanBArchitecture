using System;

namespace Services.LocalizationService.Runtime
{
    public interface ILocalizationChangeNotifier
    {
        public event Action Changed;
        void Initialize();
        void Dispose();
    }
}