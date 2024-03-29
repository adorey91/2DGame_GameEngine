using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] SpriteRenderer door1Sprite;
    [SerializeField] SpriteRenderer door2Sprite;
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

    public void OpenDoor()
    {
        door1Sprite.sprite = newDoor;
        door2Sprite.sprite = newDoor;
        door1Sprite.sortingLayerName = "opening";
        door2Sprite.sortingLayerName = "opening";
        door1collider.enabled = false;
        door2collider.enabled = false;
        door1.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        door2.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
