using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesPresenter : MonoBehaviour
{


    private List<int> saves;
    public GameObject SaveLoadButtonPrefab;
    public MenuPresenter MenuPresenter;

    private void OnEnable()
    {
        saves = ReadSaves();
        DisplaySaves();
    }

    private void DisplaySaves()
    {
        int children = transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        foreach(int save in saves)
        {
            GameObject saveSelectButton = Instantiate(SaveLoadButtonPrefab, transform);
            //saveSelectButton.GetComponent<Button>().onClick.AddListener(delegate { LoadData(save); });
            saveSelectButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + save;
        }
    }

    private List<int> ReadSaves()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Saves/");

        DirectoryInfo[] saveFolders = di.GetDirectories();

        List<int> saves = new List<int>();

        foreach (DirectoryInfo folder in saveFolders)
        {
            saves.Add(int.Parse(folder.Name));
        }
        return saves;
    }

    /*
    public virtual void LoadData(int saveNum)
    {
        ScenarioData saveScenario = ScriptableObject.CreateInstance<ScenarioData>();
        saveScenario.StartingInventory = SerializationMethods.LoadInventory(saveNum);
        saveScenario.StartingParty = SerializationMethods.LoadParty(saveNum, saveScenario.StartingInventory);
        MenuPresenter.LoadSave(saveScenario);
    }
    */
}
