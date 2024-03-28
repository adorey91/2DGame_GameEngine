using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip grassFootsteps;
    [SerializeField] AudioClip gravelFootsteps;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private Animator _animator;
    BoxCollider2D playerCollider;

    private Rigidbody2D _rb;
    private float moveSpeed;
    private Vector2 _movement;
    private bool isRunning;
    private bool isRolling;

    public LayerMask roadLayer;
    public LayerMask grassLayer;
    bool onGrass;
    bool onRoad;

    [Header("Controls")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode rollKey = KeyCode.R;
    

    public void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(rollKey))
            isRolling = true;
    }

    public void FixedUpdate()
    {
        HandleMove();
        HandleAnimation();
    }

    private void HandleMove()
    {
        if (Input.GetKey(runKey))
        {
            moveSpeed = walkSpeed * 2;
            isRunning = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            isRunning = false;
        }
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


    

    public void WalkingAudio()
    {
        if (onGrass)
            audioSource.clip = grassFootsteps;
        else if (onRoad)
            audioSource.clip = gravelFootsteps;
        else
            audioSource.clip = null;

        if(audioSource.clip != null)
            audioSource.Play();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            onGrass = true;
            onRoad = false;
            Debug.Log("Player entered grass!");
        }
        else if (other.CompareTag("Road"))
        {
            onRoad = true;
            onGrass = false;
            Debug.Log("Player entered road!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            onGrass = false;
            Debug.Log("Player left grass!");
        }
        else if (other.CompareTag("Road"))
        {
            onRoad = false;
            Debug.Log("Player left road!");
        }
    }
}
