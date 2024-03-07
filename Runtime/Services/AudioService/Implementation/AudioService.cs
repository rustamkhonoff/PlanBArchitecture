using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AudioService.Datas;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

[assembly: InternalsVisibleTo("AudioService.Zenject")]

namespace AudioService.Implementation
{
    internal class AudioService : IAudioService
    {
        private readonly Dictionary<string, AudioData> m_soundDatas;
        private readonly Dictionary<AudioClip, AudioSource> m_backgroundSources = new();
        private readonly AudioServiceStaticData m_audioServiceStaticData;
        private readonly IAudioServiceStateHelper m_stateHelper;

        private AudioSource m_baseSource;
        private AudioSource m_baseBackgroundSource;

        public AudioService(AudioServiceStaticData audioServiceStaticData, IAudioServiceStateHelper stateHelper)
        {
            m_stateHelper = stateHelper;
            m_audioServiceStaticData = audioServiceStaticData;
            m_soundDatas = CreateSoundDatasDictionary(m_audioServiceStaticData.AudioDatas);
            CreateBaseAudioSources();
            RegisterEvents();
            InitializeHelperDatas();
        }

        private void InitializeHelperDatas()
        {
            Impl_SetSoundVolume(m_stateHelper.SoundVolume());
            Impl_SetBackgroundVolume(m_stateHelper.BackgroundVolume());
            SetSoundState(m_stateHelper.SoundEnabled());
            SetBackgroundState(m_stateHelper.BackgroundEnabled());
        }

        private void RegisterEvents()
        {
            m_stateHelper.SoundStateChanged += SetSoundState;
            m_stateHelper.BackgroundStateChanged += SetBackgroundState;
            m_stateHelper.BackgroundVolumeChanged += Impl_SetBackgroundVolume;
            m_stateHelper.SoundVolumeChanged += Impl_SetSoundVolume;
        }

        private void Impl_SetBackgroundVolume(float obj)
        {
            SetBackgroundVolume(obj);
        }

        private void Impl_SetSoundVolume(float obj)
        {
            SetSoundVolume(obj);
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

        public void SetBackground(AudioClip clip, float volume = 1f, bool loop = true)
        {
            if (clip == null)
            {
                LogError("Clip equals NULL");
                return;
            }

            m_baseBackgroundSource.Stop();
            m_baseBackgroundSource.loop = loop;
            m_baseBackgroundSource.volume = volume;
            m_baseBackgroundSource.clip = clip;
            m_baseBackgroundSource.Play();
        }

        public void SetBackground(string key, float volume = 1, bool loop = true)
        {
            SetBackground(AudioClipFor(key), volume, loop);
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

        public void SetAllSoundVolume(float volume)
        {
            SetSoundVolume(volume);
            SetBackgroundVolume(volume);
        }

        public void SetSoundVolume(float volume)
        {
            SetAudioMixerVolume(m_audioServiceStaticData.SoundGroup, volume, DefaultSoundVolumeKey);
        }

        public void SetBackgroundVolume(float volume)
        {
            SetAudioMixerVolume(m_audioServiceStaticData.BackgroundGroup, volume, DefaultBackgroundVolumeKey);
        }

        public void SetAudioMixerVolume(AudioMixerGroup audioMixerGroup, float volume, string paramKey)
        {
            if (audioMixerGroup == null)
            {
                LogError("AudioMixerGroup is NULL");
                return;
            }

            audioMixerGroup.audioMixer.SetFloat(paramKey, Mathf.Lerp(-80, 0, volume));
        }

        public void SetBackgroundState(bool state)
        {
            SetAudioMixerState(m_audioServiceStaticData.BackgroundGroup, state, DefaultBackgroundVolumeKey);
        }

        public void SetSoundState(bool state)
        {
            SetAudioMixerState(m_audioServiceStaticData.SoundGroup, state, DefaultSoundVolumeKey);
        }

        public void SetAllSoundState(bool state)
        {
            SetBackgroundState(state);
            SetSoundState(state);
        }

        private void CreateBaseAudioSources()
        {
            if (m_baseSource == null)
            {
                m_baseSource = new GameObject("AudioService OneShootSource").AddComponent<AudioSource>();
                m_baseSource.spatialBlend = 0f;
                m_baseSource.playOnAwake = false;
                m_baseSource.outputAudioMixerGroup = m_audioServiceStaticData.SoundGroup;
                Object.DontDestroyOnLoad(m_baseSource.gameObject);
            }

            if (m_baseBackgroundSource == null)
            {
                m_baseBackgroundSource = new GameObject("AudioService BackgroundSource").AddComponent<AudioSource>();
                m_baseBackgroundSource.spatialBlend = 0f;
                m_baseBackgroundSource.playOnAwake = false;
                m_baseBackgroundSource.outputAudioMixerGroup = m_audioServiceStaticData.BackgroundGroup;
                Object.DontDestroyOnLoad(m_baseBackgroundSource.gameObject);
            }
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

        public void Dispose()
        {
            m_stateHelper.SoundStateChanged -= SetSoundState;
            m_stateHelper.BackgroundStateChanged -= SetBackgroundState;
            m_stateHelper.BackgroundVolumeChanged -= Impl_SetBackgroundVolume;
            m_stateHelper.SoundVolumeChanged -= Impl_SetSoundVolume;
        }

        private const string DefaultSoundVolumeKey = "soundVolume";
        private const string DefaultBackgroundVolumeKey = "bgVolume";
        private bool DebugEnabled => m_stateHelper.DebugEnabled();
        private bool SoundEnabled => m_stateHelper.SoundEnabled();
        private bool BackgroundEnabled => m_stateHelper.BackgroundEnabled();
    }
}