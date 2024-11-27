using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Play(SoundName.MainmenuSong);
    }

    [SerializeField] public Sound[] sounds;

    // List สำหรับเก็บ AudioSource ที่ไม่ได้อยู่ในระบบ SoundManager
    public List<AudioSource> externalAudioSources = new List<AudioSource>();

    [Serializable]
    public class Sound
    {
        public SoundName soundName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
        public bool loop;
       [HideInInspector] public AudioSource audioSource;
        public bool mute;
    }

    public enum SoundName
    {
        MainmenuSong,
        Click,
        hoverSound,
        CorrectItem,
        WrongItem,
        LoseSound,
        WinSound,
        CreditSong,
        Jump,
        Eat,
        CorrectPair,
        WrongPair,
        ChargeSound,
        BottlehitBinSound,
        EndLoop,
        RevealImageSound
    }

    public void Play(SoundName soundName)
    {
        Sound sound = GetSound(soundName);

        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }

        if (!sound.mute)
        {
            sound.audioSource.Play();
        }
    }

    public Sound GetSound(SoundName soundName)
    {
        return Array.Find(sounds, s => s.soundName == soundName);
    }

    public void MuteAllSounds(bool isMuted)
    {
        // Mute เสียงในระบบ SoundManager
        foreach (var sound in sounds)
        {
            sound.mute = isMuted;
            if (sound.audioSource != null)
            {
                sound.audioSource.mute = isMuted;
            }
        }

        // Mute เสียงใน externalAudioSources
        foreach (var audioSource in externalAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = isMuted;
            }
        }
    }

    public void MuteSound(SoundName soundName, bool isMuted)
    {
        Sound sound = GetSound(soundName);

        if (sound.audioSource == null)
        {
            Debug.LogWarning($"AudioSource for sound '{soundName}' not found.");
            return;
        }

        sound.mute = isMuted;
        sound.audioSource.mute = isMuted;
    }

    public void RegisterExternalAudioSource(AudioSource audioSource)
    {
        if (!externalAudioSources.Contains(audioSource))
        {
            externalAudioSources.Add(audioSource);
        }
    }
}
