using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestsLoader : MonoBehaviour
{
    public static ChestsLoader Instance;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(ChestsLoader)).Count() > 1)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public Dictionary<string, Chest> GetAllChests()
    {
        Dictionary<string, Chest> chests = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform chestTransform = transform.GetChild(i);
            chests.Add(chestTransform.name, chestTransform.GetComponent<Chest>());
        }
        return chests;
    }

    public void SetAllChests(Dictionary<string, ChestData> chestsData)
    {
        Dictionary<string, Chest> chests = GetAllChests();
        foreach(string chestName in chests.Keys)
        {
            if (chestsData.ContainsKey(chestName))
            {
                chests[chestName].SetupChest(chestsData[chestName]);
            }
        }
    }
}
