using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    [Header("Manager")]
    public GameManager _gameManager;
    public Animator _animator;
    string priorScene;


    // Callback function to be invoked after fade animation completes
    private System.Action _fadeCallback;

    public void Start()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void LoadScene(string sceneName)
    {
        priorScene = SceneManager.GetActiveScene().name;
        Fade("FadeOut", () =>
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (sceneName.StartsWith("MainMenu"))
            {
                _gameManager.LoadState(sceneName);
            }
            else if (sceneName.StartsWith("Gameplay"))
                _gameManager.LoadState("Gameplay");
            // Can load one of two panels with the game end scene.
            else if (sceneName.StartsWith("GameEnd"))
            {
                if (sceneName.EndsWith("GameOver"))
                    _gameManager.LoadState("GameOver");
                else if (sceneName.EndsWith("GameWin"))
                    _gameManager.LoadState("GameWin");

                sceneName = "GameEnd";
            }

            SceneManager.LoadScene(sceneName);
        });
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _gameManager.MovePlayerToSpawnLocation(priorScene);
        Fade("FadeIn"); // Start fade in after scene is loaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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