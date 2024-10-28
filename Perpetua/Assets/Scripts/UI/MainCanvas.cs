using Editor;
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
    public GameObject ConsumableMenu;
    private TabsController _tc;
    public AudioClipGroup MenuSound;

    public void Awake()
    {
        Instance = this;

        _tc = GetComponent<TabsController>();
        _tc.tabs.Add(InventoryPanel);
        _tc.tabs.Add(PartyPanel);
        _tc.tabs.Add(OptionsPanel);
        _tc.tabs.Add(ConsumableMenu);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ProcessTabOpenClose(InventoryPanel);
            TabsChanger.transform.Find("InventoryTab").GetComponent<CustomSelectableButton>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ProcessTabOpenClose(PartyPanel);
            TabsChanger.transform.Find("PartyTab").GetComponent<CustomSelectableButton>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ProcessTabOpenClose(OptionsPanel);
            TabsChanger.transform.Find("OptionsTab").GetComponent<CustomSelectableButton>().Select();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TabsChanger.activeInHierarchy)
            {
                MenuSound.Play();
                DisableTabs();
            }
            else
            {
                TabsChanger.transform.Find("OptionsTab").GetComponent<CustomSelectableButton>().Select();
                ProcessTabOpenClose(OptionsPanel);
            }
        }
    }
    public void ProcessTabOpenClose(GameObject tab)
    {
        bool wasOpen = tab.activeInHierarchy;
        _tc.DisableTabs();
        MenuSound.Play();

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
        TabsChanger.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SetTab(GameObject tab)
    {
        MenuSound.Play();
        _tc.DisableTabs();
        Time.timeScale = 0f;
        TabsChanger.SetActive(true);
        _tc.SetTab(tab);
    }
}
