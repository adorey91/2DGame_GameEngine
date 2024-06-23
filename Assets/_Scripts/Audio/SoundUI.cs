using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private SoundManager soundManager;

    [Header("Audio Image Settings")]
    [SerializeField] private Image mainImage;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image sfxImage;
    [SerializeField] private Gradient gradient;


    public void SetVolume(Image image, float volume)
    {
        float normalizedVolume = Mathf.InverseLerp(-80f, 20f, volume);
        soundManager.gameMixer.SetFloat(image.name, volume);
        image.fillAmount = normalizedVolume;
        image.color = gradient.Evaluate(volume);
    }
}
