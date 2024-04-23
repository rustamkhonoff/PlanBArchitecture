using System;
using LocalizationService.RuntimeLocalization;

namespace LocalizationService
{
    public class DummyLocalizationChangeNotifier : ILocalizationChangeNotifier
    {
        public void Dispose()
        {
            //ignore
        }

        public event Action Changed;

        public void Initialize()
        {
            //ignore
        }
    }
}