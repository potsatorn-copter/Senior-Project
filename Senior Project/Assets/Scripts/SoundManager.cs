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

    [SerializeField] private Sound[] sounds;

    [Serializable]
    public struct Sound
    {
        public SoundName soundName;
        public AudioClip clip;
        public float volume;
        public bool loop;
        public AudioSource audioSource;
    }

    public enum SoundName
    {
        MainmenuSong,
        Click,
        CreditSong

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Play(SoundName soundName)
    {
        Sound sound = GetSound(soundName);

        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
        }

        sound.audioSource.clip = sound.clip;
        sound.audioSource.loop = sound.loop;
        sound.audioSource.Play();
    }

    private Sound GetSound(SoundName soundName)
    {
        return Array.Find(sounds, s => s.soundName == soundName);
    }

    // Update is called once per frame

    [SerializeField] private Song[] song;

    [Serializable]
    public struct Song
    {
        public SongName songName;
        public AudioClip clips;
        public float volumes;
        public bool loops;
        public AudioSource audioSources;
    }

    public enum SongName
    {
        MenuSong,
        CreditSong
    }

    public void PlaySong(SongName songname)
    {
        Song song = GetSong(songname);

        if (song.audioSources == null)
        {
            song.audioSources = gameObject.AddComponent<AudioSource>();
        }

        song.audioSources.clip = song.clips;
        song.audioSources.loop = song.loops;
        song.audioSources.Play();
    }

    private Song GetSong(SongName songName)
    {
        return Array.Find(song, t => t.songName == songName);
    }
}
    



