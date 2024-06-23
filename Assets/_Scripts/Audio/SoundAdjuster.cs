using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAdjuster : MonoBehaviour
{
    [SerializeField] private Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        SetGradient();
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetGradient()
    {
        gradient = new Gradient();

        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(Color.green, 0.0f);
        colors[1] = new GradientColorKey(Color.yellow, 0.5f);
        colors[2] = new GradientColorKey(Color.red, 1.0f);

        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        gradient.SetKeys(colors, alphas);
    }
}
