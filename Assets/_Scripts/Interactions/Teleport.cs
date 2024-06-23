using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private LevelManager levelManager;
    public string levelName;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void Load()
    {
        levelManager.LoadScene(levelName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Load();
    }
}
