using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AudioManager : MonoBehaviour
{
    public static B_AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip[] bgmClips;

    [Header("SFX Clips")]
    public AudioClip[] sfxClips;

    [Header("Volume")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        sfxSource.playOnAwake = false;

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    // ================= BGM =================
    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
        {
            Debug.LogWarning("BGM index out of range: " + index);
            return;
        }

        if (bgmSource.clip == bgmClips[index] && bgmSource.isPlaying)
            return;

        bgmSource.clip = bgmClips[index];
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // ================= SFX =================
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length)
        {
            Debug.LogWarning("SFX index out of range: " + index);
            return;
        }

        sfxSource.PlayOneShot(sfxClips[index], sfxVolume);
    }

    // ================= VOLUME =================
    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }
}
