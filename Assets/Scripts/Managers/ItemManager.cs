using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public InteractableObject[] Collectables;
    LevelManager levelManager;
    bool checkedForItems = false;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay_field")
        {
            Collectables = FindObjectsOfType<InteractableObject>();
        }
        if (levelManager.priorScene == "Gameplay_DarkCastle" && checkedForItems == false)
        {
            CheckCollectedItems();
            checkedForItems = true;
            
        }
        else if(SceneManager.GetActiveScene().name == "Gameplay_DarkCastle")
            checkedForItems = false;
    }

    //Resets all interactable objects to not collected
    public void ResetAllItems()
    {
        foreach (var item in Collectables)
        {
            item.GetComponent<InteractableObject>().SetCollected("false");
        }
    }

    // checks if any collectable items have been collected and destroys them if so.
    public void CheckCollectedItems()
    {
        foreach (var item in FindObjectsOfType<InteractableObject>())
        {
            if (item.IsCollected())
            {
                Debug.Log("Deactivating collected item: " + item.gameObject.name);

                item.gameObject.SetActive(false);
            }
        }
    }
}
