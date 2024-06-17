using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer playerThought;
    [SerializeField] private TMP_Text playerThoughtText;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject interactAnimation;

    private void Awake()
    {
        playerSprite = GameObject.Find("Player_Sprite").GetComponent<SpriteRenderer>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        Vector2 movement = playerController.GetMovement();

        if (movement != Vector2.zero)
        {
            playerAnim.SetBool("Moving", true);
            playerAnim.SetFloat("MoveInputX", movement.x);
            playerAnim.SetFloat("MoveInputY", movement.y);
        }
        else
        {
            playerAnim.SetBool("Moving", false);
        }
    }

    public void TogglePlayerThoughts(bool thoughtEnabled, string text)
    {
        //TogglePlayerInteract(!thoughtEnabled);
        playerThought.enabled = thoughtEnabled;
        playerThoughtText.enabled = thoughtEnabled;
        playerThoughtText.text = text;
    }

    public void TogglePlayer(bool playerEnabled)
    {
        playerSprite.enabled = playerEnabled;
    }
    public void TogglePlayerInteract(bool interactEnabled)
    {
        interactAnimation.SetActive(interactEnabled);
    }
}