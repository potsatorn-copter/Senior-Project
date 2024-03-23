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

    [SerializeField] private Sound[] sounds;
    
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
       Click
    }

    public void PlaySound(SoundName soundName)
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

    private Sound GetSound(SoundName soundName)
    {
        return Array.Find(sounds, s => s.soundName == soundName);
    }
    
    
   
    [SerializeField] private Song[] songs;
    
    [Serializable] 
    public struct Song
    {
        public SongName songName;
        public AudioClip clips;
        [Range(0f,1f)] public float volumes;
        public bool loops;
        [HideInInspector] public AudioSource audioSources;
    }
    
    public enum SongName
    {
        MenuSong,
        CreditSong
    }

    public void PlaySong(SongName songname)
    {
        Song song = GetSong( songname );

        if (song.audioSources == null)
        {
            song.audioSources = gameObject.AddComponent<AudioSource>();
        }

        song.audioSources.clip = song.clips;
        song.audioSources.volume = song.volumes;
        song.audioSources.loop = song.loops;
        song.audioSources.Play();
    }

    private Song GetSong(SongName songName)
    {
        return Array.Find(songs, t => t.songName == songName);
    }

    
}
