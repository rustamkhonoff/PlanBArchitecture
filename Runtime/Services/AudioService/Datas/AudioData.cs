using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioService.Datas
{
    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _randomClip;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioClip[] _randomClips;

        public AudioClip Clip => _randomClip ? _randomClips[Random.Range(0, _randomClips.Length)] : _clip;
        public string Key => _key;
    }
}