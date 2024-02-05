using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetACGGroup : MonoBehaviour
{
    public ACGGroup UISounds;
    public ACGGroup Music;
    public ACGGroup Ambiance;

    public void SetUISoundsVolume(float volume)
    {
        SetACGGroupVolume(UISounds, volume);
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
}
