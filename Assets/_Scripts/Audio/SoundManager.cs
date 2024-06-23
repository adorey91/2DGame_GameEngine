using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundUI soundUI;

    [Header("Audio Mixer & Audio Sources")]
    public AudioMixer gameMixer;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioSource sfxAudio;
    [SerializeField] private AudioSource playerAudio;

    [Header("Audio Clips for Background")]
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private AudioClip gameplayClip;

    [Header("Audio Clips for SFX")]
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private AudioClip frogSFX;
    [SerializeField] private AudioClip footstepsClip;


    private string previousAudio;
    private float originalVolume;
    private bool isVolumeReduced = false;
    [SerializeField] private float volumeReductionFactor = 0.67f;

    private void Start()
    {
        gameManager.OnStateChanged += HandleStateChanged;

        SetInitialVolume(mainSlider, mainImage, 0f);
        SetInitialVolume(musicSlider, musicImage, 0f);
        SetInitialVolume(sfxSlider, sfxImage, 0f);
    }

    private void OnDestroy()
    {
        gameManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameManager.Gamestate newState)
    {
       // if (newState == GameManager.Gamestate.MainMenu || newState == GameManager.Gamestate.Gameplay || newState == GameManager.Gamestate.Options)
        if (newState == GameManager.Gamestate.MainMenu || newState == GameManager.Gamestate.Gameplay)
            RestoreVolume();
        else
            ReduceVolume();
    }

    public void PlayAudio(string audio)
    {
        if (previousAudio != audio)
        {
            switch (audio)
            {
                case "Menu": backgroundAudio.clip = mainClip; break;
                case "Gameplay": backgroundAudio.clip = gameplayClip; break;
            }

            previousAudio = audio;
            backgroundAudio.loop = true;
            backgroundAudio.Play();
        }
    }

    public void PlayClip(string audio)
    {
        switch (audio)
        {
            case "frog": sfxAudio.PlayOneShot(frogSFX); break;
            case "pickup": sfxAudio.PlayOneShot(pickupSFX); break;
        }
    }

    public void PlayPlayerSFX()
    {
        playerAudio.PlayOneShot(footstepsClip);
    }

    private void ReduceVolume()
    {
        if (!isVolumeReduced)
        {
            gameMixer.GetFloat("Music", out originalVolume);
            float reducedVolume = originalVolume + Mathf.Log10(volumeReductionFactor) * 20;
            gameMixer.SetFloat("Music", reducedVolume);
            SetSliderAndImage(musicSlider, musicImage, reducedVolume);
            isVolumeReduced = true;
        }
    }

    private void RestoreVolume()
    {
        if (isVolumeReduced)
        {
            gameMixer.SetFloat("Music", originalVolume);
            SetSliderAndImage(musicSlider, musicImage, originalVolume);
            isVolumeReduced = false;
        }
    }

    private void SetSliderAndImage(Slider slider, Image image, float volume)
    {
        float normalizedVolume = Mathf.InverseLerp(-80f, 20f, volume); // Updated range
        slider.value = normalizedVolume;
        image.color = gradient.Evaluate(normalizedVolume);
    }

    private void SetInitialVolume(Slider slider, Image image, float initialVolume)
    {
        float normalizedVolume = Mathf.InverseLerp(-80f, 20f, initialVolume); // Updated range
        slider.value = normalizedVolume;
        gameMixer.SetFloat(slider.name, initialVolume);
        image.color = gradient.Evaluate(normalizedVolume);
    }

    public void SetVolume(Slider slider)
    {
        Image sliderImage = null;

        switch (slider.name)
        {
            case "Main":
                sliderImage = mainImage;
                break;
            case "Music":
                sliderImage = musicImage;
                break;
            case "SFX":
                sliderImage = sfxImage;
                break;
        }

        float volume = Mathf.Lerp(-80f, 20f, slider.value); // Updated range
        gameMixer.SetFloat(slider.name, volume);
        sliderImage.color = gradient.Evaluate(slider.value);
    }
}
