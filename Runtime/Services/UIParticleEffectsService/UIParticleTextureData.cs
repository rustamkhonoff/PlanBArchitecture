#if COFFEE_PARTICLES
using System.Collections.Generic;
using UnityEngine;

namespace UIParticleEffectsService
{
    public class UIParticleTextureData
    {
        public bool IsTextureSheetMode = false;
        public ParticleSystemAnimationMode Mode = ParticleSystemAnimationMode.Sprites;
        public Vector2Int GridTilesSize;
        public Texture DefaultTexture;
        public List<Sprite> Sprites;
        public Material Material;
    }
}
#endif