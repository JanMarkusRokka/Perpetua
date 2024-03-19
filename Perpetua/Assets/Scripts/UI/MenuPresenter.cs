using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    public static MenuPresenter Instance;
    public GameObject TitleScreen;
    public GameObject SavesTab;
    public GameObject SettingsTab;
    public GameObject LoadingScreen;
    public Button NewCampaignButton;
    public Button LoadSaveButton;
    public Button SettingsButton;
    public Button ExitButton;
    public ScenarioData StartingScenario;
    public ScenarioData currentScenario;
    public GameObject FPSCounter;
    public TabsController _tc;
    private List<ScenarioData> saves;

    private void Awake()
    {
        if (MenuPresenter.Instance == null)
        {
            Instance = this;
            Debug.Log(Instance == this);
            DontDestroyOnLoad(gameObject);
            NewCampaignButton.onClick.AddListener(NewCampaign);
            LoadSaveButton.onClick.AddListener(OpenSavesTab);
            ExitButton.onClick.AddListener(ExitGame);
            SettingsButton.onClick.AddListener(OpenSettingsTab);
            _tc = GetComponent<TabsController>();
            _tc.tabs = new List<GameObject> { TitleScreen, SavesTab, SettingsTab, LoadingScreen };
            currentScenario = null;
        }
        else if (Instance != this)
        {
            Debug.Log("Destroying menu, duplicate");
            Destroy(this);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (Instance == this)
        {
            if (level == 0)
            {
                ColorOverlay.FadeToTransparent();
                gameObject.SetActive(true);
                _tc.SetTab(TitleScreen);
                return;
            }
            gameObject.SetActive(false);
            Debug.Log("Setting " + currentScenario.name);
            Events.SelectScenario(currentScenario);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        Destroy(gameObject);
    }

    private void OpenSavesTab()
    {
        _tc.SetTab(SavesTab);
    }

    public void BackToMainScreen()
    {
        _tc.SetTab(TitleScreen);
    }

    public void OpenSettingsTab()
    {
        _tc.SetTab(SettingsTab);
    }

    public void LoadSave(ScenarioData scenario)
    {
        currentScenario = scenario;
        gameObject.SetActive(true);
        _tc.SetTab(LoadingScreen);
        StartCoroutine(WaitBeforeLoad(scenario));
    }

    private IEnumerator WaitBeforeLoad(ScenarioData scenario)
    {
        ColorOverlay.FadeToBlack();
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(scenario.scene);
    }

    private void NewCampaign()
    {
        LoadSave(StartingScenario);
        //currentScenario = StartingScenario;
        //SceneManager.LoadScene(StartingScenario.scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
