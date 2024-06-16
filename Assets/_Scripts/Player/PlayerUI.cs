using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private SpriteRenderer playerThought;
    [SerializeField] private TMP_Text playerThoughtText;

    public void TogglePlayerThoughts(bool thoughtEnabled)
    {
        playerThought.enabled = thoughtEnabled;
        playerThoughtText.enabled = thoughtEnabled;
    }
}