using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    [Header("UI Elements")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Gradient gradient;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        
        gradient = new Gradient();

        // Blend color from red at 0% to blue at 100%
        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(Color.green, 0.0f);
        colors[1] = new GradientColorKey(Color.yellow, 0.5f);
        colors[2] = new GradientColorKey(Color.red, 1.0f);

        // Blend alpha from opaque at 0% to transparent at 100%
        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        gradient.SetKeys(colors, alphas);

        // Subscribe to slider events
        mainSlider.onValueChanged.AddListener(OnMainVolumeChange);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChange);

        // Initialize sliders to current volume levels
    }

    private void OnSfxVolumeChange(float value)
    {
        soundManager.SetVolume(sfxSlider, value);
    }

    private void OnMusicVolumeChange(float value)
    {
        soundManager.SetVolume(musicSlider, value);
    }

    private void OnMainVolumeChange(float value)
    {
        soundManager.SetVolume(mainSlider, value);
    }

    public void UpdateSliderValue(Slider slider, float value)
    {
        slider.value = value;
    }
}
