﻿#if ZENJECT
using UnityEngine;
using UnityEngine.UI;

namespace AudioService.Extensions.Zenject.Addons
{
    [RequireComponent(typeof(Button))]
    public class AudioButton : AudioInvocator
    {
        private Button m_button;

        private void OnEnable()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(PlaySound);
        }

        private void OnDisable()
        {
            m_button.onClick.RemoveListener(PlaySound);
        }
    }
}
#endif