using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance;
    public GameObject InventoryPanel;
    public GameObject PartyPanel;
    public GameObject OptionsPanel;
    public GameObject TabsChanger;
    private TabsController _tc;

    public void Awake()
    {
        Instance = this;

        _tc = GetComponent<TabsController>();
        _tc.tabs.Add(InventoryPanel);
        _tc.tabs.Add(PartyPanel);
        _tc.tabs.Add(OptionsPanel);
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
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ProcessTabOpenClose(OptionsPanel);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableTabs();
        }
    }
    public void ProcessTabOpenClose(GameObject tab)
    {
        bool wasOpen = tab.activeInHierarchy;
        _tc.DisableTabs();
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
        _tc.DisableTabs();
        Time.timeScale = 1f;
    }
    public void SetTab(GameObject tab)
    {
        _tc.DisableTabs();
        Time.timeScale = 0f;
        TabsChanger.SetActive(true);
        _tc.SetTab(tab);
    }
}
