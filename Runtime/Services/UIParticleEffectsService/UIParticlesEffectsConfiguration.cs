using UnityEngine;

namespace Services.UIParticleEffectsService
{
    [CreateAssetMenu(menuName = "Project/UI/Particles/Create UIParticlesEffectsConfiguration",
        fileName = "UIParticlesEffectsConfiguration", order = 0)]
    internal class UIParticlesEffectsConfiguration : ScriptableObject
    {
#if COFFEE_PARTICLES
        [SerializeField] private UIParticleAttractorView _particleAttractorViewPrefab;
#endif
        [SerializeField] private Canvas _canvasPrefab;
        [SerializeField] private int _maxAttractParticlesAmount = 20;
        [SerializeField] private ParticleSystem _defaultParticleSystemPrefab;
        [SerializeField] private Material _defaultReferenceMaterial;
#if COFFEE_PARTICLES
        public UIParticleAttractorView ParticleAttractorViewPrefab => _particleAttractorViewPrefab;
#endif
        public Canvas CanvasPrefab => _canvasPrefab;

        public int MaxAttractParticlesAmount => _maxAttractParticlesAmount;

        public Material DefaultReferenceMaterial => _defaultReferenceMaterial;

        public ParticleSystem DefaultParticleSystemPrefab => _defaultParticleSystemPrefab;
    }
}