using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    
    [Header("Camera & Bounding shape")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Collider2D foundBoundingShape;
    [SerializeField] private CinemachineConfiner2D confiner2D;

    [Header("Player Spawn Location")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSpawn;

    [Header("Scene Fade")]
    [SerializeField] private Animator fadeAnimator;


    // callback function to be invoked after fade animation completes
    private System.Action fadeCallback;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fadeAnimator = GetComponent<Animator>();
        fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void LoadScene(string sceneName)
    {
        Fade("FadeOut", () =>
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            switch(sceneName)
            {
                case "MainMenu":
                    gameManager.LoadState("MainMenu");
                    break;
                case string name when name.StartsWith("Gameplay"):
                    gameManager.LoadState("Gameplay");
                    break;
                case "GameEnd":

                    break;
            }
            SceneManager.LoadScene(sceneName);
       });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }
        else
        {
            foundBoundingShape = null;
            foundBoundingShape = GameObject.FindWithTag("Confiner").GetComponent<Collider2D>();
            confiner2D.m_BoundingShape2D = foundBoundingShape;
            
            player = GameObject.FindWithTag("Player");
            playerSpawn = GameObject.Find("SpawnPoint").GetComponent<Transform>();
            player.transform.position = playerSpawn.position;
            
            fadeAnimator = gameObject.GetComponent<Animator>();

            if (scene.name.StartsWith("Gameplay"))
                FindAnyObjectByType<QuestManager>().FindItems();

            Fade("FadeIn");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Fade(string fadeDir, System.Action callback = null)
    {
        fadeCallback = callback;
        fadeAnimator.SetTrigger(fadeDir);
    }

    public void FadeAnimationComplete()
    {
        fadeCallback?.Invoke();
    }
}
