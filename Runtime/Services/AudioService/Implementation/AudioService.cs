﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Services.AudioService.Datas;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("AudioService.Zenject")]

namespace Services.AudioService.Implementation
{
    internal class AudioService : IAudioService
    {
        private readonly Dictionary<string, AudioData> m_soundDatas;
        private readonly Dictionary<AudioClip, AudioSource> m_backgroundSources = new();
        private readonly AudioServiceStaticData m_audioServiceStaticData;
        private readonly IAudioServiceStateHelper m_stateHelper;

        private AudioSource m_baseSource;

        public AudioService(AudioServiceStaticData audioServiceStaticData, IAudioServiceStateHelper stateHelper)
        {
            m_stateHelper = stateHelper;
            m_audioServiceStaticData = audioServiceStaticData;
            m_soundDatas = CreateSoundDatasDictionary(m_audioServiceStaticData.AudioDatas);
            CreateBaseAudioSource();
        }

        public void Play(AudioClip clip, float volume = 1)
        {
            if (!SoundEnabled)
                return;

            if (clip == null)
            {
                LogError("Clip equals NULL");
                return;
            }

            m_baseSource.PlayOneShot(clip, volume);
        }

        public void Play(string key, float volume = 1)
        {
            Play(AudioClipFor(key), volume);
        }

        public void PlayWithSource(AudioSource source, AudioClip audioClip, float volume = 1f)
        {
            if (!SoundEnabled)
                return;

            if (audioClip == null || source == null)
            {
                LogError("Clip or AudioSource equals NULL");
                return;
            }

            source.PlayOneShot(audioClip, volume);
        }

        public void PlayWithSource(AudioSource source, string key, float volume = 1)
        {
            PlayWithSource(source, AudioClipFor(key), volume);
        }

        public void PlayAt(AudioClip clip, Vector3 position, float volume = 1)
        {
            if (!SoundEnabled)
                return;

            if (clip == null)
            {
                LogError("Clip equals NULL");
                return;
            }

            AudioSource.PlayClipAtPoint(clip, position, volume);
        }

        public void PlayAt(string key, Vector3 position, float volume = 1)
        {
            PlayAt(AudioClipFor(key), position, volume);
        }

        public AudioSource PlayBackground(string key, float volume = 1F, bool loop = true, AudioMixerGroup audioMixer = null)
        {
            return PlayBackground(AudioClipFor(key), volume, loop, audioMixer);
        }

        public AudioSource PlayBackground(AudioClip clip, float volume = 1F, bool loop = true, AudioMixerGroup audioMixer = null)
        {
            if (!BackgroundEnabled)
                return null;

            if (clip == null)
            {
                LogError("Clip equals NULL");
                return null;
            }

            if (m_backgroundSources.ContainsKey(clip))
            {
                LogError($"There is already background with clip, Clip {clip}");
                return null;
            }

            audioMixer ??= m_audioServiceStaticData.BackgroundGroup;

            AudioSource newSource = CreateAudioSource();
            newSource.loop = loop;
            newSource.volume = volume;
            newSource.clip = clip;
            newSource.outputAudioMixerGroup = audioMixer;
            newSource.Play();

            m_backgroundSources.Add(clip, newSource);

            return newSource;
        }

        public AudioSource GetBackgroundAudioSource(string key)
        {
            AudioClip soundData = AudioClipFor(key);
            return GetBackgroundAudioSource(soundData);
        }

        public AudioSource GetBackgroundAudioSource(AudioClip clip)
        {
            if (clip == null)
            {
                LogError("AudioClip is NULL");
                return null;
            }

            if (m_backgroundSources.TryGetValue(clip, out AudioSource audioSource))
                return audioSource;

            LogError("There is no audio source for given clip, cl");
            return null;
        }

        public void SetBackgroundAudioSourceVolume(string key, float volume)
        {
            GetBackgroundAudioSource(key).volume = volume;
        }

        public void SetBackgroundAudioSourceVolume(AudioClip clip, float volume)
        {
            GetBackgroundAudioSource(clip).volume = volume;
        }

        public void StopBackground(string key)
        {
            AudioClip soundData = AudioClipFor(key);
            StopBackground(soundData);
        }

        public void StopBackground(AudioClip clip)
        {
            if (!m_backgroundSources.TryGetValue(clip, out AudioSource source))
                return;

            source.Stop();
            Object.Destroy(source);
            m_backgroundSources.Remove(clip);
        }

        public AudioData AudioDataFor(string key)
        {
            if (m_soundDatas.TryGetValue(key, out AudioData data))
                return data;
            LogError($"AudioData for given key {key} is NULL");
            return null;
        }

        public AudioClip AudioClipFor(string key)
        {
            return AudioDataFor(key).Clip;
        }

        public void SetAudioMixerState(AudioMixerGroup audioMixerGroup, bool state, string paramKey = "volume")
        {
            if (audioMixerGroup == null)
            {
                LogError("AudioMixerGroup is NULL");
                return;
            }

            audioMixerGroup.audioMixer.SetFloat(paramKey, state ? 0f : -80f);
        }

        public void SetAllSoundVolume(float volume, string paramKey = "volume")
        {
            SetSoundVolume(volume, paramKey);
            SetBackgroundSoundVolume(volume, paramKey);
        }

        public void SetSoundVolume(float volume, string paramKey = "volume")
        {
            SetAudioMixerVolume(m_audioServiceStaticData.SoundGroup, volume, paramKey);
        }

        public void SetBackgroundSoundVolume(float volume, string paramKey = "volume")
        {
            SetAudioMixerVolume(m_audioServiceStaticData.BackgroundGroup, volume, paramKey);
        }

        public void SetAudioMixerVolume(AudioMixerGroup audioMixerGroup, float volume, string paramKey = "volume")
        {
            if (audioMixerGroup == null)
            {
                LogError("AudioMixerGroup is NULL");
                return;
            }

            audioMixerGroup.audioMixer.SetFloat(paramKey, Mathf.Lerp(-80, 0, volume));
        }

        public void SetBackgroundSoundState(bool state)
        {
            SetAudioMixerState(m_audioServiceStaticData.BackgroundGroup, state);
        }

        public void SetSoundState(bool state)
        {
            SetAudioMixerState(m_audioServiceStaticData.SoundGroup, state);
        }

        public void SetAllSoundState(bool state)
        {
            SetBackgroundSoundState(state);
            SetSoundState(state);
        }

        private void CreateBaseAudioSource()
        {
            if (m_baseSource != null)
                return;
            m_baseSource = new GameObject("AudioService OneShootSource").AddComponent<AudioSource>();
            m_baseSource.spatialBlend = 0f;
            m_baseSource.playOnAwake = false;
            m_baseSource.outputAudioMixerGroup = m_audioServiceStaticData.SoundGroup;
            Object.DontDestroyOnLoad(m_baseSource.gameObject);
        }

        private AudioSource CreateAudioSource()
        {
            return m_baseSource.gameObject.AddComponent<AudioSource>();
        }

        private Dictionary<string, AudioData> CreateSoundDatasDictionary(IEnumerable<AudioData> audioDatas)
        {
            return audioDatas.ToDictionary(soundHolderData => soundHolderData.Key,
                soundHolderData => soundHolderData);
        }

        private void LogError(string text)
        {
            if (DebugEnabled)
                Debug.LogError(text);
        }

        private bool DebugEnabled => m_stateHelper.DebugEnabled();
        private bool SoundEnabled => m_stateHelper.SoundEnabled();
        private bool BackgroundEnabled => m_stateHelper.BackgroundEnabled();
    }
}