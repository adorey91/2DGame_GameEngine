using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckKing : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] QuestAsset questAsset;

    private void Update()
    {
        if(questAsset.State == QuestAsset.QuestState.Completed)
            sprite.color = Color.white;
    }
}
