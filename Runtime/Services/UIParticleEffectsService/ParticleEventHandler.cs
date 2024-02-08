using System;
using UnityEngine;

namespace Services.UIParticleEffectsService
{
    internal class ParticleEventHandler : MonoBehaviour
    {
        public static ParticleEventHandler Create(ParticleSystem particleSystem)
        {
            return particleSystem.gameObject.AddComponent<ParticleEventHandler>();
        }

        public event Action ParticleStopped;

        private void OnParticleSystemStopped()
        {
            ParticleStopped?.Invoke();
        }
    }
}