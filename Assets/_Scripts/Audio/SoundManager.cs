using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundUIController soundUIController;

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

    private void Start()
    {
        gameManager.OnStateChanged += HandleStateChanged;
     }

    private void OnDestroy()
    {
        gameManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameManager.Gamestate newState)
    {
        // if (newState == GameManager.Gamestate.MainMenu || newState == GameManager.Gamestate.Gameplay || newState == GameManager.Gamestate.Options)
        //if (newState == GameManager.Gamestate.MainMenu || newState == GameManager.Gamestate.Gameplay)
        //    RestoreVolume();
        //else
        //    ReduceVolume();
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

    public void SetVolume(Slider slider)
    {
        // Clamp fillAmount between 0 and 1
        slider.value = Mathf.Clamp01(slider.value);

        // Map fillAmount (0 to 1) to volume (-60 to +0)
        float mappedVolume = Mathf.Lerp(-60f, 0f, slider.value);

        // Clamp mappedVolume between -60 and +0
        float volume = Mathf.Clamp(mappedVolume, -60f, 0f);

        string mixer;
        switch (slider.name)
        {
            case "Master_Slider":
                mixer = "Main";
                break;
            case "Music_Slider":
                mixer = "Music";
                break;
            case "SFX_Slider":
                mixer = "SFX";
                break;
            default:
                Debug.LogWarning("Unknown slider name: " + slider.name);
                return;
        }

        gameMixer.SetFloat(mixer, volume);
    }
}
