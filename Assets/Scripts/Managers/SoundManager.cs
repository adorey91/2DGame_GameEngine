using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainAudioSource;

    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip gamePlay;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip gameWin;


    public void Start()
    {
        mainAudioSource.clip = mainMenu;
        mainAudioSource.loop = true;
        mainAudioSource.volume = 0.1f;
        mainAudioSource.Play();
    }

    public void MainMenuAudio()
    {
        ChangeAudio(0.1f, mainMenu);
    }

    public void GameplayAudio()
    {
        ChangeAudio(0.1f, gamePlay);
    }

    public void GameWinAudio()
    {
        ChangeAudio(0.1f, gameWin);
    }

    public void GameOverAudio()
    {
        ChangeAudio(0.1f, gameOver);
        mainAudioSource.pitch = -1.2f;
    }

    void ChangeAudio(float volume, AudioClip clipName)
    {
        mainAudioSource.volume = volume;
        mainAudioSource.pitch = 1f;

        if (mainAudioSource.clip != clipName)
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = clipName;
            mainAudioSource.Play();
        }
    }
}
