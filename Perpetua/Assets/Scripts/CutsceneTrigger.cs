using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    private OverworldPlayer player;

    public OverworldEnemy dependentOnEnemy;
    public GameObject CutsceneCounterpart;

    public int timeToSkipTo;

    private void Start()
    {
        if (dependentOnEnemy != null)
            if (dependentOnEnemy.EnemyData.stats.HealthPoints <= 0)
            {
                PlayableDirector.Play();
                PlayableDirector.time = timeToSkipTo;
                GetComponent<BoxCollider>().enabled = false;
                CutsceneCounterpart.SetActive(false);
            } else
            {
                dependentOnEnemy.gameObject.SetActive(false);
            }
    }

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
        GetComponent<BoxCollider>().enabled = false;
        player.EnableMovement();
    }
}
