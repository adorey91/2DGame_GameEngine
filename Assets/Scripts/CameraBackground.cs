using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBackground : MonoBehaviour
{
    [Header("RGBA Settings")]
    [SerializeField] private Slider redSlider;
    [SerializeField] private Slider greenSlider;
    [SerializeField] private Slider blueSlider;

    public Camera playerCamera;


    public void Start()
    {
        playerCamera = GetComponent<Camera>();
        redSlider.value = 0.5f;
        greenSlider.value = 0.7f;
        blueSlider.value = 1f;
    }

    public void Update()
    {
        ColorChange();
    }

    public void ColorChange()
    {
        playerCamera.backgroundColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
    }
}
