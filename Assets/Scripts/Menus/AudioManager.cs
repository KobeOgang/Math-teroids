﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXsource;

    [Header("BGM Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameBGM;
    public AudioClip gameOverScreenMusic; // New

    [Header("SFX Clips")]
    public AudioClip missileLaunchSFX; // New
    public AudioClip asteroidExplosionSFX; // New
    public AudioClip earthImpactSFX; // New
    public AudioClip gameOverSFX;
    public AudioClip forceFieldHit;
    public AudioClip forceFieldOn;
    public AudioClip earthHealSFX;


    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMainMenuMusic();
    }

    public void PlayMainMenuMusic()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameBGM()
    {
        musicSource.clip = gameBGM;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameOverScreenMusic()
    {
        musicSource.clip = gameOverScreenMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        SFXsource.Stop();
    }

    // **New methods for playing SFX**
    public void PlayMissileLaunchSFX()
    {
        PlaySFX(missileLaunchSFX);
    }

    public void PlayForceFieldHitSFX()
    {
        PlaySFX(forceFieldHit);
    }
    public void PlayForceFieldOnSFX()
    {
        PlaySFX(forceFieldOn);
    }
    public void PlayEarthHealSFX()
    {
        PlaySFX(earthHealSFX);
    }

    public void PlayAsteroidExplosionSFX()
    {
        PlaySFX(asteroidExplosionSFX);
    }

    public void PlayEarthImpactSFX()
    {
        PlaySFX(earthImpactSFX);
    }

    public void PlayGameOverSFX()
    {
        PlaySFX(gameOverSFX);
    }


    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            SFXsource.PlayOneShot(clip);
        }
    }

}
