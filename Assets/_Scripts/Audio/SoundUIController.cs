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

    [Header("UI Image Elements")]
    [SerializeField] private Image mainFill;
    [SerializeField] private Image musicFill;
    [SerializeField] private Image sfxFill;


    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        // Subscribe to slider events
        mainSlider.onValueChanged.AddListener(OnMainVolumeChange);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChange);


        OnSfxVolumeChange(0.8f);
        OnMusicVolumeChange(0.8f);
        OnMainVolumeChange(0.8f);

        soundManager.SetVolume(sfxSlider);
        soundManager.SetVolume(mainSlider);
        soundManager.SetVolume(musicSlider);
    }

    private void OnSfxVolumeChange(float value)
    {
        soundManager.SetVolume(sfxSlider);
        sfxFill.color = gradient.Evaluate(sfxFill.fillAmount);
    }

    private void OnMusicVolumeChange(float value)
    {
        soundManager.SetVolume(musicSlider);
        musicFill.color = gradient.Evaluate(musicFill.fillAmount);
    }

    private void OnMainVolumeChange(float value)
    {
        soundManager.SetVolume(mainSlider);
        mainFill.color = gradient.Evaluate(mainFill.fillAmount);
    }
}
