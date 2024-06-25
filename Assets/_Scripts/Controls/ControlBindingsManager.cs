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
    [SerializeField] private Button bindingButton;
    [SerializeField] private TMP_Text activeBindingText;


    [Header("Input action for control reset")]
    [SerializeField] private InputActionAsset _inputActions;

    private void SwitchBindings(bool isGamepad)
    {
        bindingButton.onClick.RemoveAllListeners();
        keyboardBindings.SetActive(!isGamepad);
        gamepadBindings.SetActive(isGamepad);

        if (isGamepad)
        {
            activeBindingText.text = "Gamepad Controls";
            bindingButton.GetComponentInChildren<TMP_Text>().text = "Keyboard";
            bindingButton.onClick.AddListener(() => SwitchBindings(false));
        }
        else
        {
            activeBindingText.text = "Keyboard & Mouse Controls";
            bindingButton.GetComponentInChildren<TMP_Text>().text = "Gamepad";
            bindingButton.onClick.AddListener(() => SwitchBindings(true));
        }
    }

    public void GamepadBindingsActive()
    {
        SwitchBindings(true);
    }

    public void KeyboardBindingsActive()
    {
        SwitchBindings(false);
    }

    public void ResetAllBindings()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }
}