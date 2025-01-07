using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MasterVolumeKey = "masterVolume";
    private const string MusicVolumeKey = "musicVolume";
    private const string SFXVolumeKey = "sfxVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(MasterVolumeKey) || PlayerPrefs.HasKey(MusicVolumeKey) || PlayerPrefs.HasKey(SFXVolumeKey))
        {
            LoadVolumeSettings();
        }
        else
        {
            InitializeDefaultVolumes();
        }
    }

    public void SetMasterVolume()
    {
        SetVolume(masterSlider.value, "master", MasterVolumeKey);
    }

    public void SetMusicVolume()
    {
        SetVolume(musicSlider.value, "music", MusicVolumeKey);
    }

    public void SetSFXVolume()
    {
        SetVolume(sfxSlider.value, "sfx", SFXVolumeKey);
    }

    private void LoadVolumeSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey);
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey);

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    private void InitializeDefaultVolumes()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    private void SetVolume(float sliderValue, string mixerParameter, string playerPrefKey)
    {
        audioMixer.SetFloat(mixerParameter, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(playerPrefKey, sliderValue);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
