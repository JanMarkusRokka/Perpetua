using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public AudioClipGroup BackgroundSound;
    void Start()
    {
        BackgroundSound.Play();
    }
}
