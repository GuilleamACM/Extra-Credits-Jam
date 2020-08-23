using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Linq;
using Unity.Collections;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sound[] _sounds;

    private bool isMusicPlaying = true;
    private bool isSoundPlaying = true;

    private void Start()
    {
        //PlayerData playerData = SaveSystemManager.Instance.GetPlayerData();
        //isMusicPlaying = playerData._BGMOn;
        //isSoundPlaying = playerData._SFXOn;

        PlayAudio("bgm");
    }

    ///<summary>
    ///This creates an audio source GameObject parented to the
    ///AudioManager's transform and plays the sound.
    ///</summary>
    public Sound PlayAudio(string name)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }

        Debug.Log("Playing " + s.name);

        if (s.source == null)
            SetAudioSource(s);

        if ((s.IsBGM && !IsMusicPlaying()) || (!s.IsBGM && !IsSoundPlaying()))
            s.source.mute = true;
        else
            s.source.mute = false;

        s.Play();
        return s;
    }

    ///<summary>
    ///This receives a GameObject which is used as the audio source to give
    ///a more 3D-ish effect to the sound that is going to be played.
    ///</summary>
    public Sound PlayAudio(string name, GameObject audioSource)
    {
        Sound s = GetSound(name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }

        Debug.Log("Playing " + s.name);

        SetAudioSource(s, audioSource);

        if ((s.IsBGM && !IsMusicPlaying()) || (!s.IsBGM && !IsSoundPlaying()))
            s.source.mute = true;
        else
            s.source.mute = false;

        s.source.Play();
        return s;
    }

    public Sound GetSound(string name) => Array.Find(_sounds, sound => sound.name == name);

    public bool ToggleMusic()
    {
        isMusicPlaying = !isMusicPlaying;
        var musics = Array.FindAll(_sounds, sound => sound.IsBGM == true);
        foreach (var music in musics)
        {
            if (music.source != null)
                music.source.mute = !isMusicPlaying;
        }
        //SaveMusicOn();
        return this.isMusicPlaying;
    }

    public bool IsMusicPlaying() => isMusicPlaying;

    //private void SaveMusicOn()
    //{
    //    var playerData = SaveSystemManager.Instance.GetPlayerData();
    //    playerData._BGMOn = isMusicPlaying;
    //    SaveSystemManager.Instance.Save();
    //}

    public bool ToggleSounds()
    {
        isSoundPlaying = !isSoundPlaying;
        var sounds = Array.FindAll(_sounds, sound => sound.IsBGM == false);
        foreach (var sound in sounds)
        {
            if (sound.source != null)
                sound.source.mute = !isSoundPlaying;
        }
        //SaveSoundOn();
        return this.isSoundPlaying;
    }

    public bool IsSoundPlaying() => isSoundPlaying;

    //private void SaveSoundOn()
    //{
    //    var playerData = SaveSystemManager.Instance.GetPlayerData();
    //    playerData._SFXOn = isSoundPlaying;
    //    SaveSystemManager.Instance.Save();
    //}

    private void SetAudioSource(Sound s)
    {
        var go = new GameObject(s.name + "_Sound");
        go.transform.SetParent(transform);
        var source = go.AddComponent<AudioSource>();

        if (source == null) return;

        s.source = source;
        SetSound(s);
    }

    private void SetAudioSource(Sound s, GameObject audioSource)
    {
        var objectSource = audioSource.GetComponent<AudioSource>();
        AudioSource source;
        if (objectSource == null)
            source = audioSource.AddComponent<AudioSource>();
        else
            source = objectSource;

        s.source = source;
        SetSound(s);
    }

    private void SetSound(Sound s)
    {
        s.source.clip = s.audioFile;
        s.source.pitch = s.pitch;
        s.source.volume = s.volume;
        s.source.loop = s.loop;
    }
}
