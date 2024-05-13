#if LOCALIZATION_USE_UNITY

using System;
using System.Runtime.CompilerServices;
using LocalizationService.RuntimeLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

[assembly: InternalsVisibleTo("LocalizationService.Unity.Zenject")]

namespace LocalizationService.Unity
{
    internal class UnityLocalizationService : ILocalizationService
    {
        private const char SplitChar = ';';
        private const string ErrorPlaceholder = "No translation";
        private readonly ILocalizationChangeNotifier m_notifier;
        private readonly LocalizationConfiguration m_localizationConfiguration;

        public UnityLocalizationService(ILocalizationChangeNotifier localizationChangeNotifier,
            LocalizationConfiguration localizationConfiguration)
        {
            m_localizationConfiguration = localizationConfiguration;
            m_notifier = localizationChangeNotifier;
            m_notifier.Initialize();
        }

        public string GetString(string key, string tableName = "", params object[] arguments)
        {
            return Internal_GetString(key, tableName, arguments);
        }

        public string GetStringByIndex(string key, int index, string table = "",
            params object[] arguments)
        {
            string[] split = Internal_GetString(key, table, arguments).Split(SplitChar);

            if (split.Length != 0 && split.Length > index)
                return split[index];

            Debug.LogWarning($"No Translation or Index found, Key:{key}");
            return ErrorPlaceholder;
        }

        private string GetFixedTableName(string input)
        {
            if (string.IsNullOrEmpty(input)) input = m_localizationConfiguration.DefaultTableName;
            return input;
        }

        public IRuntimeLocalizedText ConvertToRuntimeLocalizedText(PerformTextSet performTextSet, string key, GameObject linkedObject,
            string table = "",
            Func<object[]> argumentsFunc = null,
            string format = "{0}")
        {
            table = GetFixedTableName(table);
            RuntimeLocalizedTextSetAction instance = new();
            instance.Setup(performTextSet, key, table, format, argumentsFunc);
            instance.Initialize(this, m_notifier);
            instance.LinkTo(linkedObject);
            return instance;
        }

        private string Internal_GetString(string key, string table, params object[] arguments)
        {
            table = GetFixedTableName(table);
            string found = LocalizationSettings.StringDatabase.GetLocalizedString(table, key, arguments);
            return found;
        }
    }
}

#endif