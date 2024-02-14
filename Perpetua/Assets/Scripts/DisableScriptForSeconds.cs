using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableScriptForSeconds : MonoBehaviour
{
    public void DisableOverworldEnemy(float secs)
    {
        StartCoroutine(DisableAndEnableOverworldEnemy(secs));
    }

    IEnumerator DisableAndEnableOverworldEnemy(float secs)
    {
        OverworldEnemy overworldEnemy = GetComponent<OverworldEnemy>();
        overworldEnemy.enabled = false;
        yield return new WaitForSeconds(secs);
        overworldEnemy.enabled = true;
    }

    public void DisableNavAgent(float secs)
    {
        StartCoroutine(DisableAndEnableNavAgent(secs));
    }

    IEnumerator DisableAndEnableNavAgent(float secs)
    {
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        navAgent.enabled = false;
        yield return new WaitForSeconds(secs);
        navAgent.enabled = true;
    }
}
