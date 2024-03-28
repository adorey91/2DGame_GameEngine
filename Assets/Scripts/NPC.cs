using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    GameObject player;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        player = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(player != null)
        {
            spriteRenderer.flipX = GetTargetDirection().x < 0;

        }
    }

    Vector2 GetTargetDirection()
    {
        return (player.transform.position - transform.position).normalized;
    }
}
