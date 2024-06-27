using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetBattleMusic : MonoBehaviour
{
    public AudioClipGroup AudioClipGroup;
    public PlayableDirector playableDirector;
    public float GetTime()
    {
        Debug.Log(playableDirector.time);
        return (float)playableDirector.time;
    }
}
