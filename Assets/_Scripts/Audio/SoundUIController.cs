using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    [Header("UI Elements")]
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        // Subscribe to slider events
        mainSlider.onValueChanged.AddListener(OnMainVolumeChange);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChange);

        // Initialize sliders to current volume levels
    }

    private void OnSfxVolumeChange(float value)
    {
        soundManager.SetVolume("Main", value);
    }

    private void OnMusicVolumeChange(float value)
    {
        soundManager.SetVolume("Music", value);
    }

    private void OnMainVolumeChange(float value)
    {
        soundManager.SetVolume("Main", value);
    }

    public void UpdateSliderValue(Slider slider, float value)
    {
        slider.value = value;
    }
}
