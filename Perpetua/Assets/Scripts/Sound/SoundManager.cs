using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClipGroup BackgroundSound;

    [Header("Menu Navigation Sounds")]
    public AudioClipGroup MoveSelectionSound;
    public AudioClipGroup SelectableSubmitSound;
    public AudioClipGroup Select2Sound;
    public AudioClipGroup Deselect2Sound;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
    }

    public void PlayBackgroundSound()
    {
        BackgroundSound.Play();
    }

    public void PlayUINavigationSound()
    {
        MoveSelectionSound.Play();
    }

    public void PlaySubmitSound()
    {
        SelectableSubmitSound.Play();
    }

    public void PlayUISelect2Sound()
    {
        Select2Sound.Play();
    }

    public void PlayUIDeselect2Sound()
    {
        Deselect2Sound.Play();
    }
}
