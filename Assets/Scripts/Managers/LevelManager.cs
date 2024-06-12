using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Manager")]
    public GameManager _gameManager;
    public Animator _animator;
    internal string priorScene;

    [Header("Camera & Bounding")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Collider2D boundingShape;
    [SerializeField] CinemachineConfiner2D confiner2D;

    [Header("Player Spawn")]
    [SerializeField] private GameObject player;
    private Transform playerSpawn;



    // Callback function to be invoked after fade animation completes
    private System.Action _fadeCallback;

    public void Start()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime; // So animator works when timescale is set to 0
    }

    // Loads Scene depending on the sceneName.
    public void LoadScene(string sceneName)
    {
        priorScene = SceneManager.GetActiveScene().name;
        Fade("FadeOut", () =>
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            switch(sceneName)
            {
                case "MainMenu":
                    _gameManager.LoadState("MainMenu"); break;
                case string s when s.StartsWith("Gameplay") && !priorScene.StartsWith("Gameplay"):
                    _gameManager.LoadState("GamePlay"); break;
                default: break;
            }
            SceneManager.LoadScene(sceneName);
        });
    }

    // Moves player to the spawn location of the prior scene or to the main location in game.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }
        else
        {
            // used to find binding shape and set it to the virtual camera
            boundingShape = null;
            boundingShape = GameObject.FindWithTag("Confiner").GetComponent<Collider2D>();
            confiner2D.m_BoundingShape2D = boundingShape;

                playerSpawn = GameObject.Find("SpawnPoint").GetComponent<Transform>();
            player.transform.position = playerSpawn.position;

            Fade("FadeIn"); // Start fade in after scene is loaded
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // Controls the game scene fade in and out
    public void Fade(string fadeDir, System.Action callback = null)
    {
        _fadeCallback = callback; // Set the callback

        _animator.SetTrigger(fadeDir);
    }

    // Method to be called from animation event when fade animation completes
    public void FadeAnimationComplete()
    {
        // Invoke the callback if it's not null
        _fadeCallback?.Invoke();
    }
}