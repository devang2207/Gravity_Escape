using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource; // Handles background music
    public AudioSource sfxSource;   // Handles sound effects

    [Header("Default Music Clip")]
    [SerializeField] private AudioClip musicClip;

    [Header("Sound Effects")]
    public AudioClip deathClip;
    public AudioClip transitionClip;
    public AudioClip levelFinishedClip;
    public AudioClip onClickResponseClip;
    public AudioClip gemAudioClip;

    private void Awake()
    {
        // Singleton Pattern Implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }

    private void Start()
    {
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        if (musicSource == null || sfxSource == null)
        {
            AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
            if (audioSources.Length >= 2)
            {
                musicSource = audioSources[0];
                sfxSource = audioSources[1];

                // Set the music source to loop by default
                musicSource.loop = true;
            }
            else
            {
                Debug.LogError("AudioManager: Not enough AudioSource components found in children.");
                return;
            }
        }

        // Assign and play default music clip
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: MusicSource or default musicClip is missing.");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioManager: SFX source or clip is null.");
        }
    }
    public void PlayButtonSFX()
    {
        sfxSource.PlayOneShot(onClickResponseClip);
    }
}
