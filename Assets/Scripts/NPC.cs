using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float amount;

    private void Awake()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null && animator != null)
        {
            animator.SetFloat("TargetX", GetTargetDirection("TargetX"));
            animator.SetFloat("TargetY", GetTargetDirection("TargetY"));
        }
        if(animator == null)
            spriteRenderer.flipX = GetTargetDirection().x < 0;
    }

    Vector2 GetTargetDirection()
    {
        return (player.transform.position - transform.position).normalized;
    }

    float GetTargetDirection(string direction)
    {
        
        switch(direction)
        {
            case "TargetX":
                amount = player.transform.position.x - transform.position.x;
                break;
            case "TargetY":
                amount = player.transform.position.y - transform.position.y;
                break;
        }

        return amount;
    }
}
