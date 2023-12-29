using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCanvas : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject PartyPanel;
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
    private void ProcessTabOpenClose(GameObject tab)
    {
        bool wasOpen = tab.activeInHierarchy;
        DisableTabs();
        if (wasOpen)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
        tab.SetActive(!wasOpen);
    }
    private void DisableTabs()
    {
        InventoryPanel.SetActive(false);
        PartyPanel.SetActive(false);
        Time.timeScale = 0f;
    }
}
