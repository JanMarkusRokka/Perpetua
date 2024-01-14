using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsController : MonoBehaviour
{
    public List<GameObject> tabs;
    public void DisableTabs()
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
    }

    public void SetTab(GameObject tab)
    {
        DisableTabs();
        tab.SetActive(true);
    }
}
