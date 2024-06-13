using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Serializable] 
    public struct Sound
    {
        public SoundName soundName;
        public AudioClip clip;
        [Range(0f,1f)] public float volume;
        public bool loop;
        [HideInInspector] public AudioSource audioSource;
    }
    
    public enum SoundName
    {
        MainmenuSong,
        Click,
        hoverSound,
        Correct,
        Wrong,
        LoseSound,
        WinSound,
        CreditSong
       
    }

    public void Play(SoundName soundName)
    {
        Sound sound = GetSound( soundName );

        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
        }

        sound.audioSource.clip = sound.clip;
        sound.audioSource.volume = sound.volume;
        sound.audioSource.loop = sound.loop;
        sound.audioSource.Play();
    }
    

    public Sound GetSound(SoundName soundName)
    {
        return Array.Find(sounds, s => s.soundName == soundName);
    }
    
    public void MuteAllSounds(bool isMuted)
    {
        // ปิดเสียงทั้งหมด
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.mute = isMuted;
        }
    }
    public void MuteSound(SoundName soundName, bool isMuted)
    {
        Sound sound = GetSound(soundName);
        if (sound.audioSource != null)
        {
            sound.audioSource.mute = isMuted;
        }
    }
    
    
}
