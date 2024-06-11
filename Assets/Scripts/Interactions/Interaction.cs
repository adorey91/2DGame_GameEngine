using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] GameObject interactable = null;
    [SerializeField] InteractableObject interactableObject = null;
    [SerializeField] GameObject interactAnimation;
    GameManager _gameManager;
    bool _canInteract = false;

    public PlayerInput playerInput;

    [SerializeField] private Animator animator;
    Animation pickUpAnimation;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        interactAnimation.SetActive(false);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (_canInteract && context.performed && _gameManager.isPaused == false && interactableObject != null)
        {
            if (_gameManager.gameState != GameManager.GameState.Dialogue)
                CheckInteraction();
        }
    }


    void CheckInteraction()
    {
        if (interactableObject.interactType == InteractableObject.InteractType.Pickup)
        {
            animator.SetTrigger("isPickingUp");
            animator.SetBool("Moving", false);

            interactableObject.Pickup();
            if(pickUpAnimation.isPlaying)
            {
               playerInput.actions.FindAction("Move").Disable();
            }
            else
            {
                playerInput.actions.FindAction("Move").Enable();
            }

        }
        else if (interactableObject.interactType == InteractableObject.InteractType.Info)
            interactableObject.Info();
        else if (interactableObject.interactType == InteractableObject.InteractType.Dialogue)
            interactableObject.Dialogue();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        _canInteract = true;
        interactable = other.gameObject;
        interactableObject = interactable.GetComponent<InteractableObject>();
        if (interactableObject != null)
            interactAnimation.SetActive(true);
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        interactAnimation.SetActive(false);
        interactable = null;
        interactableObject = null;
    }
}