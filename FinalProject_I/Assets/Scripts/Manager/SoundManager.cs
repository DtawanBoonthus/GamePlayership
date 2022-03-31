using System;
using UnityEngine;

namespace Manager
{
    public class SoundManager :MonoBehaviour
    {
        [SerializeField] private SoundClip[] soundClips;

        public static SoundManager Instance { get; private set; }

        public void Awake()
        {
            Debug.Assert(soundClips != null && soundClips.Length != 0,"Sound clips need to be setup");

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);                
            }
        }

        private void Start()
        {
            Play(Sound.BGM);
        }

        public enum Sound
        {
            BGM,
            PlayerFire,
            PlayerExplode,
            EnemyFire,
            EnemyExplode,
            BossEnemyFire,
            BossEnemyExplode
        }
        
        [Serializable]
        public class SoundClip
        {
            public Sound sound;
            public AudioClip audioClip;
            [Range(0,1)] public float soundVolume;
            public bool loop = false;
            [HideInInspector]
            public AudioSource audioSource;
        }
        
        public void Play(Sound sound)
        {
            var soundClip = GetSoundClip(sound);
            if (soundClip.audioSource == null)
            {   
                soundClip.audioSource = gameObject.AddComponent<AudioSource>();
            }
            soundClip.audioSource.clip = soundClip.audioClip;
            soundClip.audioSource.volume = soundClip.soundVolume;
            soundClip.audioSource.loop = soundClip.loop;
            soundClip.audioSource.Play();
        }

        private SoundClip GetSoundClip(Sound sound)
        {
            foreach (var soundClip in soundClips)
            {
                if (soundClip.sound == sound)
                {
                    return soundClip;
                }
            }
            return null;
        }
    }
}