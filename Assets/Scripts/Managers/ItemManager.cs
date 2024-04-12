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

        foreach (InteractableObject co in Collectables)
        {
            if (co.GetComponent<InteractableObject>().IsCollected())
            {
                co.gameObject.SetActive(false);
            }
        }
    }

    //Resets all Collectables to not collected
    public void ResetAllItems()
    {
        foreach (var item in Collectables)
        {
            item.GetComponent<InteractableObject>().SetCollected(false);
        }
    }
}
