using System;
using LocalizationService.RuntimeLocalization;
using TMPro;

namespace LocalizationService
{
    public interface ILocalizationService
    {
        public string GetString(string key, string tableName = "Default Localization Table", params object[] arguments);

        public string GetStringByIndex(string key, int index, string tableName = "Default Localization Table",
            params object[] arguments);

        public IRuntimeLocalizedText ConvertToUnityRuntimeLocalizedTMP(TMP_Text text, string key,
            string table = "Base String Table", Func<object[]> argumentsFunc = null, string format = "{0}");
    }
}