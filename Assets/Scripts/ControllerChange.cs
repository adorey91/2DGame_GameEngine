using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerChange : MonoBehaviour
{
    PlayerController controller;
    Interaction interaction;

    KeyCode runKey;
    KeyCode rollKey;
    KeyCode interactKey;

    private void Start()
    {
        controller = FindAnyObjectByType<PlayerController>();
        interaction = FindAnyObjectByType<Interaction>();

        runKey = controller.runKey;
        rollKey = controller.rollKey;
        interactKey = interaction.interactKey;
    }



}
