using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D triggerCollider;
    [SerializeField] private GameObject colliderObject;
    
    public void ToggleInteractable(bool interactable)
    {
        triggerCollider.enabled = interactable;
        spriteRenderer.enabled = interactable;
        colliderObject.SetActive(interactable);
    }
}
