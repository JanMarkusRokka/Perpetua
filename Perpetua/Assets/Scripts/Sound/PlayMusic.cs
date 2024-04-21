using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClipGroup Music;
    private float timestamp;
    void Start()
    {
        Play();
    }

    private void Update()
    {
        if (Time.time >= timestamp) 
        {
            Play(); 
        }
    }

    public void Play()
    {
        SoundManager.Instance.BackgroundSound = Music;
        SoundManager.Instance.PlayBackgroundSound();
        timestamp = Time.time + Music.Clips[0].length;
    }
}
