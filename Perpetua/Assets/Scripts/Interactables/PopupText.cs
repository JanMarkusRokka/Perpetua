using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PopupText : MonoBehaviour
{
    public bool ActivatesOnce = false;
    public bool HasBubble = true;
    public GameObject SpeechBubble;

    private void OnTriggerEnter(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player) EnterRange();
    }

    private void OnTriggerExit(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player) ExitRange();
    }

    public void EnterRange()
    {
        if (HasBubble)
            SpeechBubble.SetActive(true);
    }

    public void ExitRange()
    {
        if (HasBubble)
            SpeechBubble.SetActive(false);
    }
}
