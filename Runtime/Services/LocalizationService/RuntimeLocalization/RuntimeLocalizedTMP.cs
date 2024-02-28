using System;
using TMPro;
using UnityEngine;

namespace LocalizationService.RuntimeLocalization
{
    public class RuntimeLocalizedTMP : MonoBehaviour, IRuntimeLocalizedText
    {
        private TMP_Text m_text;
        private string m_key;
        private string m_table;
        private string m_format;
        private Func<object[]> m_argumentsFunc;
        private ILocalizationChangeNotifier m_notifier;
        private ILocalizationService m_localizationService;


        internal void Setup(TMP_Text text, string key, string table, string format,
            Func<object[]> argumentsFunc)
        {
            m_text = text;
            m_key = key;
            m_table = table;
            m_argumentsFunc = argumentsFunc;
            m_format = format;
        }

        internal void Initialize(ILocalizationService localizationService, ILocalizationChangeNotifier notifier)
        {
            m_localizationService = localizationService;
            m_notifier = notifier;
            m_notifier.Changed += UpdateText;

            UpdateText();
        }

        #region Interface

        public void UpdateText()
        {
            m_text.SetText(string.Format(m_format,
                m_localizationService.GetString(m_key, m_table, m_argumentsFunc?.Invoke())));
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
            m_notifier.Changed -= UpdateText;
        }
    }
}