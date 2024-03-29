using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    private OverworldPlayer player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<OverworldPlayer>();

        if (player)
        {
            player.DisableMovement();
            PlayableDirector.Play();
        }
    }

    public void CutsceneEnded()
    {
        player.EnableMovement();
    }
}
