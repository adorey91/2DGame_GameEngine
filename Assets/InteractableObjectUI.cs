using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectUI : MonoBehaviour
{
    [SerializeField] private CircleCollider2D triggerCollider;
    [SerializeField] private GameObject colliderObject;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ToggleInteractable(bool interactable)
    {
        triggerCollider.enabled = interactable;
        colliderObject.SetActive(interactable);

        if(spriteRenderer != null)
            spriteRenderer.enabled = interactable;
    }
}
