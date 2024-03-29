﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioService.Datas
{
    [CreateAssetMenu(menuName = "Project/Audio/Create AudioService Static Data", fileName = "AudioServiceStaticData", order = 0)]
    internal class AudioServiceStaticData : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup _soundGroup;
        [SerializeField] private AudioMixerGroup _backgroundGroup;
        [SerializeField] private List<AudioData> _audioDatas;

        public IEnumerable<AudioData> AudioDatas => _audioDatas;
        public AudioMixerGroup SoundGroup => _soundGroup;
        public AudioMixerGroup BackgroundGroup => _backgroundGroup;
    }
}