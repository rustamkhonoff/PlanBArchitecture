using System;
using LocalizationService.RuntimeLocalization;
using TMPro;

namespace LocalizationService
{
    public class DummyLocalizationService : ILocalizationService
    {
        public string GetString(string key, string tableName = "Default Localization Table", params object[] arguments)
        {
            return key.ToUpper();
        }

        public string GetStringByIndex(string key, int index, string tableName = "Default Localization Table", params object[] arguments)
        {
            return key.ToUpper();
        }

        public IRuntimeLocalizedText ConvertToUnityRuntimeLocalizedTMP(TMP_Text text, string key, string table = "Base String Table",
            Func<object[]> argumentsFunc = null,
            string format = "{0}")
        {
            RuntimeLocalizedTMP instance = text.gameObject.AddComponent<RuntimeLocalizedTMP>();
            instance.Setup(text, key, table, format, argumentsFunc);
            instance.Initialize(this, new DummyLocalizationChangeNotifier());
            return instance;
        }
    }
}