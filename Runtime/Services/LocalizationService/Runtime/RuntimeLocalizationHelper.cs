using System;
using TMPro;

namespace Services.LocalizationService.Runtime
{
    public static class RuntimeLocalizationHelper
    {
        public static ILocalizationService LocalizationService { get; private set; }
        public static ILocalizationChangeNotifier Notifier { get; private set; }

        public static void Construct(ILocalizationService service, ILocalizationChangeNotifier notifier)
        {
            LocalizationService = service;
            Notifier = notifier;
        }

        public static void Initialize()
        {
            Notifier.Initialize();
        }

        public static void Dispose()
        {
            Notifier.Dispose();
        }

        public static IRuntimeLocalizedTMP ConvertToUnityRuntimeLocalizedTMP(this TMP_Text text, string key,
            string table = "Base String Table", string format = "{0}", Func<object[]> argumentsFunc = null)
        {
            return RuntimeLocalizedTMP.Convert(text, key, table, format, argumentsFunc);
        }
    }
}