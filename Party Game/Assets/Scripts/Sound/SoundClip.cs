using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        
        [Range(0.0f, 1.0f)] public float volume;
        [Range(0.0f, 1.0f)] public float spacialBlend;

        [HideInInspector] public AudioSource source;
    }
}