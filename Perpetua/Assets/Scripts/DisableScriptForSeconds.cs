using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScriptForSeconds : MonoBehaviour
{
    public void DisableOverworldEnemy<Type>(float secs)
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
}
