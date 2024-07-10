using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsController : MonoBehaviour
{
    public List<GameObject> tabs;
    public int id;

    public static void ClearTab(GameObject tab)
    {
        Transform tabTransform = tab.transform;
        int children = tabTransform.childCount;
        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(tabTransform.GetChild(i).gameObject);
        }
    }
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
