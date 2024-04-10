using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] GameObject interactable = null;
    [SerializeField] InteractableObject interactableObject = null;
    [SerializeField] GameObject canInteract;
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        canInteract.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(_gameManager.interactKey) && interactableObject != null) && _gameManager.isPaused == false)
            CheckInteraction();
    }

    void CheckInteraction()
    {
        if (interactableObject.interactType == InteractableObject.InteractType.Pickup)
            interactableObject.Pickup();
        else if (interactableObject.interactType == InteractableObject.InteractType.Info)
            interactableObject.Info();
        else if (interactableObject.interactType == InteractableObject.InteractType.Dialogue)
            interactableObject.Dialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        interactable = other.gameObject;
        interactableObject = interactable.GetComponent<InteractableObject>();
        if (interactableObject != null)
            canInteract.SetActive(true);
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        canInteract.SetActive(false);
        interactable = null;
        interactableObject = null;
    }
}