using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClipGroup Music;
    public bool NotLooping;

    void Start()
    {
        Play();
    }

    public void Play()
    {
        Music.Looping = !NotLooping;
        SoundManager.Instance.BackgroundSound = Music;
        SoundManager.Instance.PlayBackgroundSound();
    }

    public void PlayFromSetTime(float time)
    {
        Music.Looping = !NotLooping;
        SoundManager.Instance.BackgroundSound = Music;
        SoundManager.Instance.BackgroundSound.playFromTime = time;
        SoundManager.Instance.BackgroundSound.Play();
    }
}
