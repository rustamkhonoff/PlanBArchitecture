﻿using System;
using AudioService.Datas;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioService
{
    public interface IAudioService: IDisposable
    {
        void Initialize();
        void Play(string key, float volume = 1f);
        void Play(AudioClip clip, float volume = 1f);
        void PlayWithSource(AudioSource source, string key, float volume = 1f);
        void PlayWithSource(AudioSource source, AudioClip audioClip, float volume = 1f);
        void PlayAt(string key, Vector3 position, float volume = 1f);
        void PlayAt(AudioClip clip, Vector3 position, float volume = 1f);
        void SetBackground(AudioClip clip, float volume = 1f, bool loop = true);
        void SetBackground(string key, float volume = 1f, bool loop = true);
        AudioSource PlayBackground(string key, float volume = 1f, bool loop = true, AudioMixerGroup audioMixer = null);
        AudioSource PlayBackground(AudioClip clip, float volume = 1f, bool loop = true, AudioMixerGroup audioMixer = null);
        AudioSource GetBackgroundAudioSource(string key);
        AudioSource GetBackgroundAudioSource(AudioClip clip);
        void SetBackgroundAudioSourceVolume(string key, float volume);
        void SetBackgroundAudioSourceVolume(AudioClip clip, float volume);
        void StopBackground(string key);
        void StopBackground(AudioClip clip);
        AudioData AudioDataFor(string key);
        AudioClip AudioClipFor(string key);
        void SetAudioMixerState(AudioMixerGroup audioMixerGroup, bool state, string paramKey);
        void SetAudioMixerVolume(AudioMixerGroup audioMixerGroup, float volume, string paramKey);
        void SetSoundVolume(float volume);
        void SetBackgroundVolume(float volume);
        void SetAllSoundVolume(float volume);
        void SetSoundState(bool state);
        void SetBackgroundState(bool state);
        void SetAllSoundState(bool state);
    }
}