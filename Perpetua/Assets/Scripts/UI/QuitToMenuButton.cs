using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitToMenuButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( delegate { Time.timeScale = 1f; SceneManager.LoadScene(0); } );
    }
}
