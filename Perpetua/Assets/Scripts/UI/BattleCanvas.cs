using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public BattleManager BattleManager;
    public GameObject OptionsPanel;
    public GameObject PartyPresenter;
    public GameObject ActionsPresenter;
    public GameObject PartyMemberPresenterPrefab;
    public GameObject ActionOptionPresenterPrefab;
    public GameObject AttackEffectsPresenter;
    public GameObject ThanksForPlaying;
    public TextMeshProUGUI TextPresenter;
    public List<Sprite> BattleEffects;
    public Sprite CrossImage;
    private Color pmPresDefaultColor;
    private Color partyMemberHighlightColor = Color.green;
    private TabsController _tc;
    private IEnumerator RevealTextCoroutine = null;
    private bool SelectEnm;
    private string currentActionType;

    void Start()
    {
        _tc = GetComponent<TabsController>();
        _tc.tabs = new List<GameObject>
        {
            OptionsPanel
        };
        partyMemberHighlightColor.a = 0.5f;
        pmPresDefaultColor = PartyMemberPresenterPrefab.GetComponent<Image>().color;
        TextPresenter.text = "";
        SelectEnm = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsPanel.activeInHierarchy)
                _tc.DisableTabs();
            else _tc.SetTab(OptionsPanel);
        }
        if (SelectEnm && Input.GetMouseButtonDown(0))
        {
            Transform selected = SelectRay(BattleManager.Enemies);
            if (selected != null)
            {
                SelectEnemy(selected);
            }
        }
    }

    // Finds transform from list of parent's children
    private Transform SelectRay(Transform parent)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            foreach(Transform child in parent)
            {
                if (child.GetInstanceID() == hit.collider.GetComponent<Transform>().GetInstanceID())
                {
                    return child;
                }
            }
        }
        return null;
    }

    public void DisplayAttackEffect()
    {
        AttackEffectsPresenter.GetComponent<SpriteRenderer>().sprite = BattleEffects[UnityEngine.Random.Range(0, BattleEffects.Count)];
        AttackEffectsPresenter.SetActive(true);
    }
    public void StopDisplayingAttackEffect()
    {
        AttackEffectsPresenter.SetActive(false);
    }

    public void SetTurn()
    {
        PopulatePartyTab();
        DisplayMainActionOptions();
    }

    private void DisplayMainActionOptions()
    {
        ClearTab(ActionsPresenter);
        GameObject attackButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
        attackButton.GetComponent<Button>().onClick.AddListener( delegate { StartSelectEnemy("Atk"); } );
        GameObject b = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        b.GetComponentInChildren<TextMeshProUGUI>().text = "More options coming soon";
        /*
        GameObject skillsButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        skillsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skills";
        GameObject guardButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        guardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Guard";
        GameObject itemButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Item";*/
    }

    private void StartSelectEnemy(string actionType)
    {
        StopTextCoroutine();
        RevealTextCoroutine = TextMethods.RevealText("Select Enemy", TextPresenter, 0.1f);
        StartCoroutine(RevealTextCoroutine);
        currentActionType = actionType;
        SelectEnm = true;
        SetEnemyHoverHighlight(true);

        //BattleManager.AddActionToQueue(actionType.New(BattleManager.GetCurrentTurnTaker(),));
    }

    private void SetEnemyHoverHighlight(bool value)
    {
        Transform Enemies = BattleManager.Enemies;
        for (int i = 0; i < Enemies.childCount; i++)
        {
            Transform child = Enemies.GetChild(i);
            child.GetComponent<HoverHighlight>().SetActiveValue(value);
        }
    }

    private void SelectEnemy(Transform Enemy)
    {
        SelectEnm = false;
        SetEnemyHoverHighlight(false);
        StopTextCoroutine();
        TextPresenter.text = "";
        BattleAction action = CreateDoubleParticipantAction(BattleManager.agilityOrder[BattleManager.currentTurn], BattleManager.agilityOrder[int.Parse(Enemy.name)], currentActionType);
        BattleManager.AddActionToQueue(action);
    }

    private BattleAction CreateDoubleParticipantAction(BattleParticipant giver, BattleParticipant recipient, string action)
    {
        if (action == "Atk")
        {
            // Temp
            BattleAction a = Attack.New(giver, recipient);
            a.CommitAction();
            return a;
        }
        return null;
    }

    public void StopTextCoroutine()
    {
        if (RevealTextCoroutine != null)
            StopCoroutine(RevealTextCoroutine);
    }

    private string StatsToText(StatsData stats)
    {
        return stats.HealthPoints + "\n" + stats.AttackSpeed;
    }

    public void PopulatePartyTab()
    {
        ClearTab(PartyPresenter);
        List<BattleParticipant> agilityOrder = BattleManager.agilityOrder;

        for (int i = 0; i < agilityOrder.Count; i++)
        {
            BattleParticipant turnTaker = agilityOrder[i];
            if (turnTaker.IsPartyMember)
            {
                PartyCharacterData partyMember = turnTaker.GetPartyMember();
                GameObject partyMemberPresenter = Instantiate(PartyMemberPresenterPrefab, PartyPresenter.transform);
                if (partyMember.stats.HealthPoints <= 0)
                {
                    partyMemberPresenter.transform.Find("Image").GetComponent<Image>().sprite = CrossImage;
                }
                else
                {
                    partyMemberPresenter.transform.Find("Image").GetComponent<Image>().sprite = partyMember.image;
                }
                partyMemberPresenter.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = partyMember.image.textureRect.size * 2;
                partyMemberPresenter.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = partyMember.name;
                partyMemberPresenter.transform.Find("Stats").GetComponent<TextMeshProUGUI>().text = StatsToText(partyMember.stats);
                partyMemberPresenter.name = i.ToString();
                if (i == BattleManager.currentTurn)
                {
                    partyMemberPresenter.GetComponent<Image>().color = partyMemberHighlightColor;
                }
            }
        }
    }

    public void SetPartyMemberColor(int member, Color color)
    {
        Debug.Log("setting member " + member + " color");
        PartyPresenter.transform.Find(member.ToString()).GetComponent<Image>().color = color;
    }

    public void ResetPartyMemberColor(int member)
    {
        SetPartyMemberColor(member, pmPresDefaultColor);
    }

    public void ClearTab(GameObject tab)
    {
        Transform tabTransform = tab.transform;
        int children = tabTransform.childCount;
        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(tabTransform.GetChild(i).gameObject);
        }
    }
}
