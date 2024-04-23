#if LOCALIZATION_USE_UNITY

using System;
using LocalizationService.RuntimeLocalization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace LocalizationService.Unity.RuntimeLocalization
{
    internal class UnityLocalizationChangeNotifier : ILocalizationChangeNotifier
    {
        public event Action Changed;

        public void Initialize()
        {
            LocalizationSettings.SelectedLocaleChanged += Notify;
        }

        private void Notify(Locale obj)
        {
            Changed?.Invoke();
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= Notify;
        }
    }
}

#endif