using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Rigidbody2D rb;
    private Vector2 movement;

    internal bool noMoving = false;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private Animator animator;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public void FixedUpdate()
    {
        if(noMoving == false)
        {
            HandleMove();
            HandleAnimation();
        }
    }

    private void HandleMove()
    {
        rb.MovePosition(rb.position + movement.normalized * walkSpeed * Time.fixedDeltaTime);
    }

    private void HandleAnimation()
    {
        if (movement != Vector2.zero)
        {
            animator.SetFloat("MoveInputX", movement.x);
            animator.SetFloat("MoveInputY", movement.y);
        }
        else
            animator.SetBool("Moving", false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void SkipText(InputAction.CallbackContext context)
    {
        // should this be here?
    }


    public void Pause(InputAction.CallbackContext context)
    {
        // used for pause. should this be here?
    }
}
