#if COFFEE_PARTICLES
using UIParticleEffectsService.Builder;

namespace UIParticleEffectsService
{
    public interface IUIParticleEffectsService
    {
        void Attract(UIParticleAttractConfiguration configuration);
    }
}
#endif