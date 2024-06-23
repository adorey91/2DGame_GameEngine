using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private Singleton singleton;

    [Header("Camera & Bounding shape")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Collider2D foundBoundingShape;
    [SerializeField] private CinemachineConfiner2D confiner2D;

    [Header("Player Spawn Location")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerSpawn;

    [Header("Scene Fade")]
    [SerializeField] private Animator fadeAnimator;

    private string previousScene;
    private bool isLoadingScene = false;

    // Callback function to be invoked after fade animation completes
    private System.Action fadeCallback;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fadeAnimator = GetComponent<Animator>();
        fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void LoadScene(string sceneName)
    {
        if (isLoadingScene) return;

        isLoadingScene = true;
        previousScene = SceneManager.GetActiveScene().name;

        Fade("FadeOut", () =>
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            switch (sceneName)
            {
                case "MainMenu":
                    gameManager.LoadState("MainMenu");
                    singleton.ClearInstance();
                    break;
                case string name when name.StartsWith("Gameplay"):
                    gameManager.LoadState("Gameplay");
                    break;
                case "GameEnd":
                    gameManager.LoadState("GameEnd");
                    break;
            }
            SceneManager.LoadScene(sceneName);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (scene.name == "MainMenu")
        {
            isLoadingScene = false;
            return;
        }

        foundBoundingShape = GameObject.FindWithTag("Confiner")?.GetComponent<Collider2D>();
        if (foundBoundingShape != null)
            confiner2D.m_BoundingShape2D = foundBoundingShape;

        player = GameObject.FindWithTag("Player");

        if (previousScene == "Gameplay_DarkCastle")
            playerSpawn = GameObject.Find("SpawnPoint_ReturnFromDarkCastle")?.GetComponent<Transform>();
        else
            playerSpawn = GameObject.Find("SpawnPoint")?.GetComponent<Transform>();

        if (playerSpawn != null)
            player.transform.position = playerSpawn.position;

        if (scene.name.EndsWith("field"))
            FindAnyObjectByType<QuestUI>().FindItems();
        if (scene.name.EndsWith("DarkCastle"))
            FindAnyObjectByType<QuestUI>().FindDuckKing();

        Fade("FadeIn", () => isLoadingScene = false);
    }

    private void Fade(string fadeDir, System.Action callback = null)
    {
        fadeCallback = callback;
        fadeAnimator.SetTrigger(fadeDir);
    }

    public void FadeAnimationComplete()
    {
        fadeCallback?.Invoke();
        fadeCallback = null; // Clear the callback to prevent it from being invoked again
    }
}
