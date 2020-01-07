using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    public class SoundManager : MonoBehaviour, IInitializable
    {
        [SerializeField] private AudioMixer _masterMixer;
        
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceSFX;

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
            
        }
        
        public void PlaySoundSpatialSfx(string name, GameObject gameObject)
        {
            
        }
    }
}