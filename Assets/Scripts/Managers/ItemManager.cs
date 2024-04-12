using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public InteractableObject[] Collectables;

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay_field")
        {
            Collectables = FindObjectsOfType<InteractableObject>();
        }

        CheckCollectedItems();
    }

    //Resets all Collectables to not collected
    public void ResetAllItems()
    {
        foreach (var item in Collectables)
        {
            item.GetComponent<InteractableObject>().SetCollected("false");
        }
    }

    public void CheckCollectedItems()
    {
        foreach (var item in Collectables)
        {
            if (item.IsCollected())
            {
                Debug.Log(item.gameObject.name + " is collected.");
                item.enabled = false;
            }
            else
            {
                Debug.Log(item.gameObject.name + " is not collected.");
                item.enabled = true;
            }
        }
    }
}
