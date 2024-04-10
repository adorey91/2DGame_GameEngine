using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public QuestAsset quest;

    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    private SpriteRenderer door1Sprite;
    private SpriteRenderer door2Sprite;
    [SerializeField] Sprite newDoor;
    BoxCollider2D door1collider;
    BoxCollider2D door2collider;

    public void Start()
    {
        door1Sprite = door1.GetComponent<SpriteRenderer>();
        door2Sprite = door2.GetComponent<SpriteRenderer>();
        door1collider = door1.GetComponent<BoxCollider2D>();
        door2collider = door2.GetComponent<BoxCollider2D>();
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
        door1Sprite.sprite = newDoor;
        door2Sprite.sprite = newDoor;
        door1Sprite.sortingLayerName = "opening";
        door2Sprite.sortingLayerName = "opening";
        door1collider.enabled = false;
        door2collider.enabled = false;
        door1.transform.GetChild(0).gameObject.SetActive(true);
        door2.transform.GetChild(0).gameObject.SetActive(true);
    }
}
