#if LOCALIZATION_USE_UNITY

using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace LocalizationService.Unity.Addons
{
    public class UnityLocaleSwap : MonoBehaviour
    {
        [SerializeField] private Button _prevButton, _nextButton;
        [SerializeField] private TMP_Text _value;

        private ILocalesProvider m_localesProvider;
        private int m_localesCount;
        private int m_currentLocaleIndex;

        private void Start()
        {
            m_localesProvider = LocalizationSettings.AvailableLocales;
            m_currentLocaleIndex = m_localesProvider.Locales.IndexOf(LocalizationSettings.Instance.GetSelectedLocale());
            m_localesCount = m_localesProvider.Locales.Count;

            if (_prevButton != null)
                _prevButton.onClick.AddListener(PrevLocale);
            if (_nextButton != null)
                _nextButton.onClick.AddListener(NextLocale);

            UpdateValue();
        }

        private void OnDestroy()
        {
            if (_prevButton != null)
                _prevButton.onClick.RemoveListener(PrevLocale);
            if (_nextButton != null)
                _nextButton.onClick.RemoveListener(NextLocale);
        }

        private void NextLocale()
        {
            m_currentLocaleIndex += 1;
            m_currentLocaleIndex %= m_localesCount;
            SetLocale(m_currentLocaleIndex);
        }

        private void PrevLocale()
        {
            m_currentLocaleIndex -= 1;
            m_currentLocaleIndex = Mathf.Abs(m_currentLocaleIndex) % m_localesCount;
            SetLocale(m_currentLocaleIndex);
        }

        private void SetLocale(int index)
        {
            SetLocale(m_localesProvider.Locales[index]);
        }

        private void SetLocale(Locale locale)
        {
            LocalizationSettings.Instance.SetSelectedLocale(locale);
            UpdateValue();
        }

        private void UpdateValue()
        {
            if (_value != null)
                _value.SetText(LocalizationSettings.Instance.GetSelectedLocale().LocaleName.Split("(")[0]);
        }
    }
}

#endif