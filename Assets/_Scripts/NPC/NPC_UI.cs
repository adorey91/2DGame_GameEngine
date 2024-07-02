using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_UI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject player;
    private float amount;

    private void Update()
    {
        if (player == null)
            player = GameObject.Find("Player");

        if (gameObject.name == "Dialogue - DuckKing" || gameObject.name == "Dialogue - CastleProtector")
            spriteRenderer.flipX = GetTargetDirection().x < 0;
        else
        {
            animator.SetFloat("TargetX", GetTargetDirection("TargetX"));
            animator.SetFloat("TargetY", GetTargetDirection("TargetY"));
        }
    }

    Vector2 GetTargetDirection()
    {
        return (player.transform.position - transform.position).normalized;
    }

    float GetTargetDirection(string direction)
    {
        switch (direction)
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
