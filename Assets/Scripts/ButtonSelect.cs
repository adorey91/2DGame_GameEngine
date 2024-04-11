using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public Button primaryMenu;
    public Button primaryPause;
    public Button primaryOptions;
    public Button primaryGameComplete;
    public Button primaryDialogue;
    public Button primaryConfirmation;
    public Button primaryCredits;
    public Button primaryControls;


    public void Start()
    {
        primaryMenu.Select();
    }

    public void Change(string buttonName)
    {
        switch(buttonName)
        {
            case "MainMenu": primaryMenu.Select(); break;
            case "Pause": primaryPause.Select(); break;
            case "Options": primaryOptions.Select(); break;
            case "GameComplete": primaryGameComplete.Select(); break;
            case "Dialogue": primaryDialogue.Select(); break;
            case "Confirmation": primaryConfirmation.Select(); break;
            case "Credits": primaryCredits.Select(); break;
            case "Controls": primaryControls.Select(); break;
        }
    }
}
