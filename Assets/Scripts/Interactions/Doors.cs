using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public QuestAsset quest;

    [SerializeField] GameObject door;
    [SerializeField] private Sprite newDoor;
    private SpriteRenderer doorSprite;
    private BoxCollider2D doorCollider;

    public void Start()
    {
        doorSprite = door.GetComponent<SpriteRenderer>();
        doorCollider = door.GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (quest.State == QuestAsset.QuestState.Completed)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        doorSprite.sprite = newDoor;
        doorSprite.sortingLayerName = "opening";
        doorCollider.enabled = false;
        door.transform.GetChild(0).gameObject.SetActive(true);
    }
}
