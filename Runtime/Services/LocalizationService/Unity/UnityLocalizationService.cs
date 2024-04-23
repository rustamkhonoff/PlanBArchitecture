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

        public UnityLocalizationService(ILocalizationChangeNotifier localizationChangeNotifier)
        {
            m_notifier = localizationChangeNotifier;
            m_notifier.Initialize();
        }

        public string GetString(string key, string tableName = "Default Localization Table", params object[] arguments)
        {
            return Internal_GetString(key, tableName, arguments);
        }

        public string GetStringByIndex(string key, int index, string tableName = "Default Localization Table",
            params object[] arguments)
        {
            string[] split = Internal_GetString(key, tableName, arguments).Split(SplitChar);

            if (split.Length != 0 && split.Length > index)
                return split[index];

            Debug.LogWarning($"No Translation or Index found, Key:{key}");
            return ErrorPlaceholder;
        }

        public IRuntimeLocalizedText ConvertToUnityRuntimeLocalizedTMP(TMP_Text text, string key,
            string table = "Base String Table", Func<object[]> argumentsFunc = null, string format = "{0}")
        {
            RuntimeLocalizedTMP component = text.gameObject.AddComponent<RuntimeLocalizedTMP>();
            component.Setup(text, key, table, format, argumentsFunc);
            component.Initialize(this, m_notifier);
            return component;
        }


        private static string Internal_GetString(string key, string table, params object[] arguments)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.Log($"Given empty key {key}");
                return ErrorPlaceholder;
            }

            string found = LocalizationSettings.StringDatabase.GetLocalizedString(table, key, arguments);
            return found;
        }
    }
}

#endif