using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( delegate { Time.timeScale = 1f; Application.Quit(); });
    }
}
