using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private AudioMixerGroup mainAudio;
    [SerializeField] private AudioMixerGroup musicAudio;
    [SerializeField] private AudioMixerGroup SFXAudio;

    [Header("Audio Level Sliders")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Image Settings")]
    [SerializeField] private Image mainImage;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image sfxImage;
    [SerializeField] private Gradient gradient;

    private void Start()
    {
        SetVolume(mainSlider);
        SetVolume(musicSlider);
        SetVolume(sfxSlider);
    }


    public void SetVolume(Slider slider)
    {
        switch(slider.name)
        {
            case "Main":
                SetVolume(slider, mainImage);
                break;
            case "Music":
                SetVolume(slider, musicImage);
                break;
            case "SFX":
                SetVolume(slider, sfxImage);
                break;
        }
    }

    private void SetVolume(Slider slider, Image sliderFill)
    {
        //if (slider.value < 1)
        //    slider.value = 0.001f;

        gameMixer.SetFloat(slider.name, Mathf.Log10(slider.value) * 20);
        sliderFill.color = gradient.Evaluate(sliderFill.fillAmount);
    }
}
