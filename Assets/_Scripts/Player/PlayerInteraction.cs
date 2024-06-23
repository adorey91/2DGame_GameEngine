using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] private ItemData[] itemData;

    [SerializeField] private GameObject interactable = null;
    [SerializeField] private InteractableObject interactableObject = null;
    [SerializeField] private GameObject interactAnimation;

    [SerializeField] private GameManager gameManager;
    private bool canInteract = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Animator animator;
    [SerializeField] private Animation pickUpAnimation;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        gameManager = FindObjectOfType<GameManager>();
        interactAnimation.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventoryManager.AddItem(itemData[0]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventoryManager.AddItem(itemData[1]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventoryManager.AddItem(itemData[2]);
        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (canInteract && context.performed && interactableObject != null)
        {
            if (gameManager.gameState != GameManager.Gamestate.Dialogue)
                CheckInteraction();
        }
    }

    private void CheckInteraction()
    {
        if (interactableObject.interactableType == InteractableObject.Interactable.None)
            Debug.Log("This is set to none. Nothing will happen");
        
        if (interactableObject.interactableType == InteractableObject.Interactable.Info)
            interactableObject.Info();
        
        if (interactableObject.interactableType == InteractableObject.Interactable.Pickup)
            StartCoroutine(HandlePickUp());
     
        if (interactableObject.interactableType == InteractableObject.Interactable.Dialogue)
            interactableObject.Dialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        interactable = other.gameObject;
        interactableObject = interactable.GetComponent<InteractableObject>();
        
        if (interactableObject != null)
            playerUI.TogglePlayerInteract(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerUI.TogglePlayerInteract(false);
        interactable = null;
        interactableObject = null;
    }

    private IEnumerator HandlePickUp()
    {
        // turn movement off
        playerInput.actions.FindAction("Move").Disable();
        animator.SetTrigger("isPickingUp");
        yield return new WaitForSeconds(0.2f);
        interactableObject.Pickup();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        //turn movement back on
        playerInput.actions.FindAction("Move").Enable();
    }

}
