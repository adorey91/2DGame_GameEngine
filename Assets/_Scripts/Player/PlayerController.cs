using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float walkSpeed = 5f;

    private bool noMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (!noMoving)
        {
            rb.MovePosition(rb.position + movement.normalized * walkSpeed * Time.fixedDeltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void SkipText(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FindObjectOfType<DialogueManager>().skipText = true;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
            gameManager.EscapeState();
    }

    public void PlayerMovement(string action)
    {
        switch (action)
        {
            case "MainMenu":
            case "Pause":
            case "Dialogue":
                noMoving = true;
                break;
            case "Move":
                noMoving = false;
                break;
        }
    }

    public Vector2 GetMovement()
    {
        return movement;
    }
}
