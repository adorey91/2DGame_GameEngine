using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip grassFootsteps;
    [SerializeField] AudioClip gravelFootsteps;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private Animator _animator;

    GameManager _gameManager;
    private Rigidbody2D _rb;
    private float moveSpeed;
    private Vector2 _movement;
    private bool isRunning;
    private bool isRolling;

    public void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }
    public void FixedUpdate()
    {
        HandleMove();
        HandleAnimation();
    }

    private void HandleMove()
    {
        if (isRunning)
            moveSpeed = walkSpeed * 2;
        else
            moveSpeed = walkSpeed;
        
        _rb.MovePosition(_rb.position + _movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleAnimation()
    {
        if (_movement != Vector2.zero)
        {
            _animator.SetFloat("MoveInputX", _movement.x);
            _animator.SetFloat("MoveInputY", _movement.y);
            _animator.SetBool("Moving", true);
     
            if(isRolling)
            {
                _animator.SetTrigger("Rolling");
                isRolling = false;
            }
            if (isRunning)
                _animator.speed = 2;
            else
                _animator.speed = 1;
        }
        else
            _animator.SetBool("Moving", false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if(context.performed)
            isRunning = true;
        if (context.canceled)
            isRunning = false;
    }


    public void Roll(InputAction.CallbackContext context)
    {
        if(context.performed)
            isRolling = true;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
            _gameManager.EscapeState();
    }
}
