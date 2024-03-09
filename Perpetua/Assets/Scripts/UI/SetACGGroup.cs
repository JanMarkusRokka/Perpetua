using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetACGGroup : MonoBehaviour
{
    public ACGGroup SFX;
    public ACGGroup Music;
    public ACGGroup Ambiance;

    public void SetSFXVolume(float volume)
    {
        SetACGGroupVolume(SFX, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetACGGroupVolume(Music, volume);
    }

    public void SetAmbianceVolume(float volume)
    {
        SetACGGroupVolume(Ambiance, volume);
    }

    private void SetACGGroupVolume(ACGGroup acgg, float volume)
    {
        acgg.VolumeMin = volume;
        acgg.VolumeMax = volume;
        acgg.SetSettings();
    }
    public void SetFPSTarget(float targetFPS)
    {
        if (targetFPS == 4)
            Application.targetFrameRate = -1;
        else
            Application.targetFrameRate = (int) targetFPS * 30;
    }
}
