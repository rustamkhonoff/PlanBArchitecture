using System;
using UnityEngine;

namespace LocalizationService.RuntimeLocalization
{
    public delegate void PerformTextSet(string text);

    public class RuntimeLocalizedTextSetAction : IRuntimeLocalizedText, IDisposable
    {
        private PerformTextSet m_performTextSetAction;
        private string m_key;
        private string m_table;
        private string m_format;
        private Func<object[]> m_argumentsFunc;

        private ILocalizationChangeNotifier m_notifier;
        private ILocalizationService m_localizationService;

        internal void LinkTo(GameObject gameObject)
        {
            DestroyEvent.Create(gameObject, Dispose);
        }

        internal void Initialize(ILocalizationService localizationService, ILocalizationChangeNotifier notifier)
        {
            m_localizationService = localizationService;
            m_notifier = notifier;
            m_notifier.Changed += UpdateText;

            UpdateText();
        }

        internal void Setup(PerformTextSet performTextSetAction, string key, string table, string format,
            Func<object[]> argumentsFunc)
        {
            m_performTextSetAction = performTextSetAction;
            m_argumentsFunc = argumentsFunc;
            m_format = format;
            m_table = table;
            m_key = key;
        }

        #region Interface

        public void UpdateText()
        {
            m_performTextSetAction.Invoke(string.Format(m_format,
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

        public void Dispose()
        {
            m_notifier.Changed -= UpdateText;
        }
    }
}