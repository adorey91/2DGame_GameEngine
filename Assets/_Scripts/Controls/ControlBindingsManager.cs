using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlBindingsManager : MonoBehaviour
{
    [Header("UI for Controls Page")]
    [SerializeField] private GameObject keyboardBindings;
    [SerializeField] private GameObject gamepadBindings;
    [SerializeField] private Button keyboardButton;
    [SerializeField] private Button gamepadButton;
    [SerializeField] private TMP_Text controlsText;


    [Header("Input action for control reset")]
    [SerializeField] private InputActionAsset _inputActions;

    public void GamepadBindingsActive()
    {
        keyboardBindings.SetActive(false);
        keyboardButton.enabled = true;
        gamepadBindings.SetActive(true);
        gamepadButton.enabled = false;
        controlsText.text = "Gamepad Controls";
    }

    public void KeyboardBindingsActive()
    {
        gamepadBindings.SetActive(false);
        keyboardBindings.SetActive(true);
        keyboardButton.enabled = false;
        controlsText.text = "Keyboard Controls";
    }

    public void ResetAllBindings()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }
}