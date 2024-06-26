using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioData audioData;

    private float masterVolume = 1f;
    private float musicVolume = 0.5f;
    private float sfxVolume = 0.3f;

    void Awake()
    {
        SetupSingleton();
    }
    private void Start()
    {
        RegisterAudioOnOccasions();
        GameEvents.TriggerTitleScreen();
    }

    private void SetupSingleton()
    {
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

    public void PlayMusic(MusicTrack track, bool loop = true)
    {
        AudioClip clip = audioData.GetMusicClip(track);
        /*if (GameManager.Instance.isTitleScreen)
        {
            clip = audioData.GetMusicClip(MusicTrack.TitleScreen);
        }*/
        if (clip != null)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlaySFX(SoundEffect effect, bool loop = false)
    {
        AudioClip clip = audioData.GetSFXClip(effect);
        if (clip != null)
        {
            if (!loop)
            {
                sfxSource.clip = clip;
                sfxSource.loop = loop;
                sfxSource.volume = musicVolume * masterVolume;
                sfxSource.Play();
            }
            else
            {
                sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        // No need to update volumes here since SFX volume is applied when played

        // Adjust volume for all registered SFX audio sources
        foreach (var source in SFXVolumeAdjuster.AllSFXSources)
        {
            source.AdjustVolume(volume);
        }
    }

    private void UpdateVolumes()
    {
        musicSource.volume = musicVolume * masterVolume; // Update music volume
        // Note: As mentioned, adjusting the volume of currently playing SFX is not straightforward due to PlayOneShot. Future SFX will use the new volume.
    }

    private void RegisterAudioOnOccasions()
    {
        GameEvents.OnTitleScreen += () => PlayMusic(MusicTrack.TitleScreen);
        GameEvents.OnTowerDefense += () => PlayMusic(MusicTrack.PrepareBattle);
        GameEvents.OnEnemySpawn += () => PlayMusic(MusicTrack.StartLevel);
        GameEvents.OnBreedingStart += () => PlayMusic(MusicTrack.StartBreeding);
        GameEvents.OnBreedingComplete += () => PlayMusic(MusicTrack.PrepareBattle);
        GameEvents.OnLevelComplete += () => PlayMusic(MusicTrack.Victory, false);
        GameEvents.OnLevelFail += () => PlayMusic(MusicTrack.Defeat, false);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}


