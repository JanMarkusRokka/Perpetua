using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitToMenuButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( delegate { StartCoroutine(LoadMainMenu()); } );
    }

    private IEnumerator LoadMainMenu()
    {
        ColorOverlay.FadeToBlack();
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        MenuPresenter.Instance.gameObject.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
