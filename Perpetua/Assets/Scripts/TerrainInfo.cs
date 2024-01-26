using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainInfo : MonoBehaviour
{
    public string BattleScene;
    //battle scene for the type of area
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<OverworldPlayer>())
        {
            Events.SetBattleScene(BattleScene);
        }
    }
}
