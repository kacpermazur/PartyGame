using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    public class SoundManager : MonoBehaviour, IInitializable
    {
        private static readonly string name = typeof(SoundManager).Name;
        
        [SerializeField] private AudioMixer _masterMixer;
        
        private AudioSource _audioSourceMusic;
        private AudioSource _audioSourceSFX;

        [SerializeField] private SoundClip[] _soundClipsSFX;
        [SerializeField] private SoundClip[] _soundClipsMusic;
        [SerializeField] private SoundClip[] _soundClipsUI;

        public enum SoundType
        {
            SFX,
            MUSIC,
            UI
        }
        
        public void Initialize()
        {
            DontDestroyOnLoad(this);

            _audioSourceMusic = gameObject.AddComponent<AudioSource>();
            _audioSourceSFX = gameObject.AddComponent<AudioSource>();
            
            _audioSourceMusic.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("Music")[0];
            _audioSourceSFX.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("SFX")[0];
        }

        public void PlaySound(string name, SoundType type)
        {
            SoundClip soundClip;
            
            switch (type)
            {
                case SoundType.SFX:
                    soundClip = Array.Find(_soundClipsSFX, clip => clip.name == name);
                    SetSettings(ref _audioSourceSFX, soundClip);
                    _audioSourceSFX.Play();
                    break;
                case SoundType.UI:
                    soundClip = Array.Find(_soundClipsUI, clip => clip.name == name);
                    SetSettings(ref _audioSourceSFX, soundClip);
                    _audioSourceSFX.Play();
                    break;
                case SoundType.MUSIC:
                    soundClip = Array.Find(_soundClipsMusic, clip => clip.name == name);
                    SetSettings(ref _audioSourceMusic, soundClip);
                    _audioSourceMusic.Play();
                    break;
                default:
                    soundClip = null;
                    Log("clip not found!");
                    return;
            }
        }
        
        public void PlaySoundSpatialSfx(string name, GameObject targetObject)
        {
            SoundClip soundClip = Array.Find(_soundClipsSFX, clip => clip.name == name);
            
            if(soundClip.source == null)
            {
                soundClip.source = targetObject.AddComponent<AudioSource>();
                
                soundClip.source.clip = soundClip.clip;
                soundClip.source.loop = soundClip.loop; 
                soundClip.source.volume = soundClip.volume;
                soundClip.source.spatialBlend = soundClip.spacialBlend; 

                Log("new audio source SET!");
            }

            soundClip.source.Play();
        }
        
        public void StopSound(string name, SoundType type)
        {
            SoundClip soundClip;
            
            switch (type)
            {
                case SoundType.SFX:
                    soundClip = Array.Find(_soundClipsSFX, clip => clip.name == name);
                    SetSettings(ref _audioSourceSFX, soundClip);
                    _audioSourceSFX.Stop();
                    break;
                case SoundType.UI:
                    soundClip = Array.Find(_soundClipsUI, clip => clip.name == name);
                    SetSettings(ref _audioSourceSFX, soundClip);
                    _audioSourceSFX.Stop();
                    break;
                case SoundType.MUSIC:
                    soundClip = Array.Find(_soundClipsMusic, clip => clip.name == name);
                    SetSettings(ref _audioSourceMusic, soundClip);
                    _audioSourceMusic.Stop();
                    break;
                default:
                    soundClip = null;
                    Log("clip not found!");
                    return;
            }
        }

        private void SetSettings(ref AudioSource source, SoundClip clip)
        {
            source.clip = clip.clip;
            source.loop = clip.loop; 
            source.volume = clip.volume;
            source.spatialBlend = clip.spacialBlend; 
        }
        
        private static void Log(string message)
        {
            Debug.Log("<color=blue>" + name + "</color> : " + message);
        }
    }
}