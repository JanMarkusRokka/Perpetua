using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCanvas : MonoBehaviour
{
    public GameObject InventoryPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryPanel.activeInHierarchy)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
            InventoryPanel.SetActive(!InventoryPanel.activeInHierarchy);
        }
    }

}
