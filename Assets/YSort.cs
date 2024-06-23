using UnityEngine;

public class YSort : MonoBehaviour
{
    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -(int) (transform.position.y * 100);
    }
}
