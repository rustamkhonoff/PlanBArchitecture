using Common.ReferenceValue;
using UnityEngine;
using Zenject;

namespace Services.AudioService.Common.Zenject.Addons
{
    public class AudioInvocator : MonoBehaviour
    {
        [SerializeField] private ReferenceValue<string, AudioClip> _audioData;

        private IAudioService m_audioService;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            m_audioService = audioService;
        }

        public void PlaySound()
        {
            _audioData.InvokeAction(m_audioService.Play, m_audioService.Play);
        }
    }
}