using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemData[] itemData;

    [Header("Interactable")]
    [SerializeField] private GameObject interactable = null;
    [SerializeField] private InteractableObject interactableObject = null;
    [SerializeField] private GameObject interactAnimation;
    
    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    private InventoryManager inventoryManager;

    [Header("Player")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Animator animator;
    [SerializeField] private Animation pickUpAnimation;
    private bool canInteract = false;

    private Transform target;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        gameManager = FindObjectOfType<GameManager>();
        interactAnimation.SetActive(false);
    }

    // Used for debugging purposes
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        inventoryManager.AddItem(itemData[0]);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        inventoryManager.AddItem(itemData[1]);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        inventoryManager.AddItem(itemData[2]);
    //    }
    //}

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
        {
            FaceTarget();
            StartCoroutine(HandlePickUp());
        }
     
        if (interactableObject.interactableType == InteractableObject.Interactable.Dialogue)
            interactableObject.Dialogue();
    }

    private void FaceTarget()
    {
        animator.SetFloat("MoveInputX", (target.position.x - transform.position.x));
        animator.SetFloat("MoveInputY", (target.position.y - transform.position.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        target = other.transform;
        interactable = other.gameObject;
        interactableObject = interactable.GetComponent<InteractableObject>();
        
        if (interactableObject != null)
            playerUI.TogglePlayerInteract(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerUI.TogglePlayerInteract(false);
        target = null;
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
