using Services.UIParticleEffectsService.Builder;

namespace Services.UIParticleEffectsService
{
    public interface IUIParticleEffectsService
    {
        void Attract(UIParticleAttractConfiguration configuration);
    }
}