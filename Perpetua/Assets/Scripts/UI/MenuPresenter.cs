using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    public static MenuPresenter Instance;
    public Button NewCampaignButton;
    public Button ContinueButton;
    public Button SettingsButton;
    public Button ExitButton;

    private void Awake()
    {
        NewCampaignButton.onClick.AddListener(NewCampaign);
        ExitButton.onClick.AddListener(ExitGame);
        if (Instance != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(false);
    }

    private void NewCampaign()
    {
        SceneManager.LoadScene("SnowScene");
        Debug.Log("New Campaign");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
