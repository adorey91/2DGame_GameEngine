using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup mainAudio;
    [SerializeField] private AudioMixerGroup musicAudio;
    [SerializeField] private AudioMixerGroup SFXAudio;


    // Start is called before the first frame update
    void Start()
    {
        
    }

}
