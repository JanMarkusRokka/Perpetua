using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ColorOverlay : MonoBehaviour
{
    public static ColorOverlay Instance;

    private UnityEngine.UI.Image img;
    void Awake()
    {
        if (FindObjectsOfType(typeof(TooltipManager)).Count() > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            img = GetComponent<UnityEngine.UI.Image>();
            img.color = Color.black;
        }
    }

    private void Start()
    {
        img.CrossFadeAlpha(0f, 0f, false);
    }

    public static void FadeToColor(Color toColor, float durationSeconds)
    {
        Instance.img.CrossFadeColor(toColor, durationSeconds, true, true);
    }

    public static void FadeToBlack()
    {
        FadeToColor(Color.black, 2f);
    }

    public static void FadeToTransparent()
    {
        Color transparent = Instance.img.color;
        transparent.a = 0f;
        FadeToColor(transparent, 3f);
    }
}
