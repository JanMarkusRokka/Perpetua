using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public float maxBrightness;
    public float minBrightness;
    public float Speed;
    private float changeTime;
    private Image image;
    private Color startColor;
    private Color endColor;
    private Color currentColor;
    private bool cyclePart;

    void Start()
    {
        image = GetComponent<Image>();
        startColor = image.color * minBrightness;
        endColor = image.color * maxBrightness;
        currentColor = startColor;
        cyclePart = false;
    }

    void Update()
    {
        if (Time.time > changeTime)
        {
            changeTime = Time.time + Speed;
            image.CrossFadeColor(currentColor, Speed, false, false);
            if (cyclePart)
            {
                currentColor = startColor;
            }
            else
            {
                currentColor = endColor;
            }
            cyclePart = !cyclePart;
        }
    }
}
