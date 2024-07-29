using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> panels;
    private GameObject panel;

    private void OnEnable()
    {
        if (PartyManager.Instance.party.showTutorial)
        {
            foreach(GameObject tutPanel in panels)
            {
                tutPanel.SetActive(false);
            } 
            PartyManager.Instance.party.showTutorial = false;
            ShowNextPanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowNextPanel()
    {
        if (panel)
        {
            Destroy(panel);
        }
        if (panels.Count > 0)
        {
            panel = panels[0];
            panel.SetActive(true);
            panels.RemoveAt(0);
            return;
        }
        Destroy(gameObject);
        // select action button
    }
}
