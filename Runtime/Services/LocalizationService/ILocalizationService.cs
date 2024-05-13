using System;
using LocalizationService.RuntimeLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalizationService
{
    public interface ILocalizationService
    {
        public string GetString(string key, string tableName = "", params object[] arguments);

        public string GetStringByIndex(string key, int index, string tableName = "",
            params object[] arguments);

        public IRuntimeLocalizedText ConvertToRuntimeLocalizedText(TMP_Text text, string key,
            string table = "", Func<object[]> argumentsFunc = null, string format = "{0}") =>
            ConvertToRuntimeLocalizedText(input => text.SetText(input), key, text.gameObject, table, argumentsFunc, format);

        public IRuntimeLocalizedText ConvertToRuntimeLocalizedText(Text text, string key,
            string table = "", Func<object[]> argumentsFunc = null, string format = "{0}") =>
            ConvertToRuntimeLocalizedText(input => text.text = input, key, text.gameObject, table, argumentsFunc, format);

        //When linked object is being destroyed IRuntimeLocalizedText will cleanup subscriptions to ILocalizationChangeNotifier
        public IRuntimeLocalizedText ConvertToRuntimeLocalizedText(PerformTextSet performTextSet, string key, GameObject linkedObject,
            string table = "", Func<object[]> argumentsFunc = null, string format = "{0}");
    }
}