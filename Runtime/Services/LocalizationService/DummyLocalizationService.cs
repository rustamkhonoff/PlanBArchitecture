using System;
using LocalizationService.RuntimeLocalization;
using UnityEngine;

namespace LocalizationService
{
    public class DummyLocalizationService : ILocalizationService
    {
        private readonly DummyLocalizationChangeNotifier m_localizationChangeNotifier;

        public DummyLocalizationService()
        {
            m_localizationChangeNotifier = new DummyLocalizationChangeNotifier();
        }

        public string GetString(string key, string tableName = "Default Localization Table", params object[] arguments)
        {
            return key.ToUpper();
        }

        public string GetStringByIndex(string key, int index, string tableName = "Default Localization Table", params object[] arguments)
        {
            return key.ToUpper();
        }

        public IRuntimeLocalizedText ConvertToRuntimeLocalizedText(PerformTextSet performTextSet, string key, GameObject linkedObject,
            string table = "",
            Func<object[]> argumentsFunc = null,
            string format = "{0}")
        {
            RuntimeLocalizedTextSetAction instance = new();
            instance.Setup(performTextSet, key, table, format, argumentsFunc);
            instance.Initialize(this, m_localizationChangeNotifier);
            instance.LinkTo(linkedObject);
            return instance;
        }
    }
}