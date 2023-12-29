using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance;
    public GameObject InventoryPanel;
    public GameObject PartyPanel;
    public GameObject TabsChanger;

    public void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ProcessTabOpenClose(InventoryPanel);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ProcessTabOpenClose(PartyPanel);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableTabs();
        }
    }
    public void ProcessTabOpenClose(GameObject tab)
    {
        bool wasOpen = tab.activeInHierarchy;
        DisableTabs();
        if (wasOpen)
        {
            Time.timeScale = 1f;
            TabsChanger.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            TabsChanger.SetActive(true);
        }
        tab.SetActive(!wasOpen);
    }
    public void DisableTabs()
    {
        InventoryPanel.SetActive(false);
        PartyPanel.SetActive(false);
        TabsChanger.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SetTab(GameObject tab)
    {
        DisableTabs();
        Time.timeScale = 0f;
        TabsChanger.SetActive(true);
        tab.SetActive(true);
    }
}
