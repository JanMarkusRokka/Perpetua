using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundBite : MonoBehaviour
{
    public AudioClipGroup SoundBite;
    public bool PlayAtStart;
    void Start()
    {
        if (PlayAtStart) SoundBite.Play();
    }

}
