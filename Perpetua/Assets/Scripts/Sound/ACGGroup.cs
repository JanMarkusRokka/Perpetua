using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(menuName = "ACGGRoup")]
public class ACGGroup : ScriptableObject
{
    [Range(0, 2)]
    public float VolumeMin = 1;
    [Range(0, 2)]
    public float VolumeMax = 1;
    [Range(0, 2)]
    public float PitchMin = 1;
    [Range(0, 2)]
    public float PitchMax = 1;

    public List<AudioClipGroup> AudioClipGroups;

    private void OnEnable()
    {
    }

    public void SetSettings()
    {
        foreach(AudioClipGroup acg in AudioClipGroups)
        {
            acg.VolumeMin = VolumeMin;
            acg.VolumeMax = VolumeMax;
            acg.PitchMin = PitchMin;
            acg.PitchMax = PitchMax;
        }
    }
}
