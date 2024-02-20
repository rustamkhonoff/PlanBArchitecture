using System;
using System.Runtime.CompilerServices;
using Services.LocalizationService.Runtime;
using UnityEngine;
using UnityEngine.Localization.Settings;

[assembly: InternalsVisibleTo("LocalizationService.Unity.Zenject")]

namespace Services.LocalizationService.Unity
{
    internal class UnityLocalizationService : ILocalizationService, IDisposable
    {
        private const char SplitChar = ';';
        private const string ErrorPlaceholder = "No translation";


        public UnityLocalizationService()
        {
            RuntimeLocalizationHelper.Construct(this, new UnityLocalizationChangeNotifier());
            RuntimeLocalizationHelper.Initialize();
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

        private string Internal_GetString(string key, string table, params object[] arguments)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.Log($"Given empty key {key}");
                return ErrorPlaceholder;
            }

            string found = LocalizationSettings.StringDatabase.GetLocalizedString(table, key, arguments);
            return found;
        }

        public void Dispose()
        {
            RuntimeLocalizationHelper.Dispose();
        }
    }
}