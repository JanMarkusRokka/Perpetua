using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float nextCheckTime;
    public float CheckEverySeconds;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        nextCheckTime = Time.time + CheckEverySeconds;
    }
    void Update()
    {
        if (Time.time > nextCheckTime)
        {
            text.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
            nextCheckTime = Time.time + CheckEverySeconds;
        }
    }
}
