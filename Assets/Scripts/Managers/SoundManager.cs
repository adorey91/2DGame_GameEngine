using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField] AudioClip mainMenu;
    [SerializeField] AudioClip gamePlay;
    [SerializeField] AudioClip gameOver;
    [SerializeField] AudioClip gameWin;


    string previousLevel;

    public void Start()
    {
        audioSource.clip = mainMenu;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        audioSource.Play();
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
        audioSource.pitch = -1.2f;
    }

    void ChangeAudio(float volume, AudioClip clipName)
    {
        audioSource.volume = volume;
        audioSource.pitch = 1f;

        if (audioSource.clip != clipName)
        {
            audioSource.Stop();
            audioSource.clip = clipName;
            audioSource.Play();
        }
    }


}
