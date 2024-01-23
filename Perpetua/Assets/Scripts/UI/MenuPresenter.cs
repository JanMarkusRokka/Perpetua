using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    public static MenuPresenter Instance;
    public GameObject TitleScreen;
    public GameObject SavesTab;
    public Button NewCampaignButton;
    public Button LoadSaveButton;
    public Button SettingsButton;
    public Button ExitButton;
    public ScenarioData StartingScenario;
    private ScenarioData currentScenario;
    private List<ScenarioData> saves;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        NewCampaignButton.onClick.AddListener(NewCampaign);
        LoadSaveButton.onClick.AddListener(OpenSavesTab);
        ExitButton.onClick.AddListener(ExitGame);
        currentScenario = null;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            gameObject.SetActive(true);
            Debug.Log("active main menu");
            Debug.Log(gameObject.activeInHierarchy);
            return;
        }
        gameObject.SetActive(false);
        Events.SelectScenario(currentScenario);
    }

    private void OpenSavesTab()
    {
        TitleScreen.SetActive(false);
        SavesTab.SetActive(true);
    }

    public void BackToMainScreen()
    {
        TitleScreen.SetActive(true);
        SavesTab.SetActive(false);
    }

    public void LoadSave(ScenarioData scenario)
    {
        currentScenario = scenario;
        SceneManager.LoadScene(scenario.scene);
    }

    private void NewCampaign()
    {
        currentScenario = StartingScenario;
        SceneManager.LoadScene(StartingScenario.scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
