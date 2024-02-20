using System;
using TMPro;
using UnityEngine;

namespace Services.LocalizationService.Runtime
{
    public class RuntimeLocalizedTMP : MonoBehaviour, IRuntimeLocalizedTMP
    {
        private TMP_Text m_text;
        private string m_key;
        private string m_table;
        private string m_format;
        private Func<object[]> m_argumentsFunc;

        public static RuntimeLocalizedTMP Convert(TMP_Text text,
            string key,
            string table = "Base String Table",
            string format = "{0}",
            Func<object[]> argumentsFunc = null)
        {
            RuntimeLocalizedTMP runtimeText = text.gameObject.AddComponent<RuntimeLocalizedTMP>();
            runtimeText.Setup(text, key, table, format, argumentsFunc);
            runtimeText.Initialize();
            return runtimeText;
        }

        private void Setup(TMP_Text text, string key, string table, string format,
            Func<object[]> argumentsFunc)
        {
            m_text = text;
            m_key = key;
            m_table = table;
            m_argumentsFunc = argumentsFunc;
            m_format = format;
        }

        private void Initialize()
        {
            Notifier.Changed += UpdateText;

            UpdateText();
        }

        #region Interface

        public void UpdateText()
        {
            if (LocalizationService != null)
                m_text.SetText(LocalizationService.GetString(m_key, m_table, m_format, m_argumentsFunc?.Invoke()));
        }


        public void UpdateKey(string key, bool autoUpdate)
        {
            m_key = key;
            if (autoUpdate)
                UpdateText();
        }

        public void UpdateTable(string table, bool autoUpdate)
        {
            m_table = table;
            if (autoUpdate)
                UpdateText();
        }

        public void UpdateFormat(string format, bool autoUpdate)
        {
            m_format = format;
            if (autoUpdate)
                UpdateText();
        }

        public void UpdateArguments(Func<object[]> func, bool autoUpdate)
        {
            m_argumentsFunc = func;
            if (autoUpdate)
                UpdateText();
        }

        #endregion

        private void OnDestroy()
        {
            Notifier.Changed -= UpdateText;
        }

        private static ILocalizationService LocalizationService => RuntimeLocalizationHelper.LocalizationService;
        private static ILocalizationChangeNotifier Notifier => RuntimeLocalizationHelper.Notifier;
    }
}