using Services.UIParticleEffectsService.Builder;
using UnityEngine;
using Zenject;

namespace Services.UIParticleEffectsService.Testing
{
    public class UIParticlesTesting : MonoBehaviour
    {
        [SerializeField] private UIParticleAttractConfiguration _configuration;
        
        private IUIParticleEffectsService m_particleEffectsService;

        [Inject]
        private void Construct(IUIParticleEffectsService particleEffectsService)
        {
            m_particleEffectsService = particleEffectsService;
        }

        [ContextMenu(nameof(Test))]
        private void Test()
        {
            m_particleEffectsService.Attract(_configuration);
        }
    }
}