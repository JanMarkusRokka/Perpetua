using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public BattleManager BattleManager;
    [Header("Main tabs")]
    public GameObject OptionsPanel;

    [Header("Left tabs")]
    public GameObject ActionsPresenter;
    public GameObject ActionOptionPresenterPrefab;
    public GameObject SelectTargetLeftPanel;
    public GameObject SkillsPresenter;

    [Header("Right tabs")]
    public GameObject PartyPresenter;
    public GameObject PartyMemberPresenterPrefab;

    [Header("Turn order")]
    public GameObject TurnOrderPresenter;
    public GameObject OrderCharPresenter;

    [Header("Enemy stuff")]
    public GameObject Selections;
    public GameObject EnemySelectionPrefab;
    public GameObject HealthBars;
    public GameObject HealthBarPrefab;
    public GameObject StatusEffects;
    public GameObject StatusEffectsPrefab;
    public GameObject StatusEffectPresenterPrefab;

    [Header("Effects")]
    public GameObject ThanksForPlaying;
    public TextMeshProUGUI TextPresenter;
    public Sprite CrossImage;

    [NonSerialized]
    public BattleEffects battleEffects;

    private Color pmPresDefaultColor;
    private Color partyMemberHighlightColor = Color.green;
    [NonSerialized]
    public TabsController TC;
    [NonSerialized]
    public TabsController LeftTC;
    [NonSerialized]
    public TabsController RightTC;
    private IEnumerator RevealTextCoroutine = null;
    private bool SelectEnm;
    private string currentActionType;

    void Start()
    {
        setupTabControllers();
        partyMemberHighlightColor.a = 0.5f;
        pmPresDefaultColor = PartyMemberPresenterPrefab.GetComponent<Image>().color;
        TextPresenter.text = "";
        SelectEnm = false;
        battleEffects = GetComponent<BattleEffects>();
    }

    private void setupTabControllers()
    {
        TabsController[] tcArray = GetComponents<TabsController>();
        foreach (TabsController tc in tcArray)
        {
            if (tc.id == 0) TC = tc;
            else if (tc.id == 1) LeftTC = tc;
            else if (tc.id == 2) RightTC = tc;
        }
        TC.tabs = new List<GameObject>
        {
            OptionsPanel
        };

        LeftTC.tabs = new List<GameObject>
        {
            ActionsPresenter,
            SelectTargetLeftPanel,
            SkillsPresenter
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsPanel.activeInHierarchy)
                TC.DisableTabs();
            else TC.SetTab(OptionsPanel);
        }
        /*
        // Alternative mouse selection
        if (SelectEnm && Input.GetMouseButtonDown(0))
        {
            Transform selected = SelectRay(BattleManager.Enemies);
            if (selected != null)
            {
                SelectEnemy(selected);
            }
        }
        */
    }

    public void SetSelectEnm(bool value)
    {
        SelectEnm = value;
        SetEnemySelectionButtons(value);
        SetEnemyHealthBars(value);
        //SetEnemyHoverHighlight(value);
    }

    public void SetEnemySelectionButtons(bool value)
    {
        if (value)
        {
            Transform Enemies = BattleManager.Enemies;
            for (int i = 0; i < Enemies.childCount; i++)
            {
                Transform child = Enemies.GetChild(i);
                Vector3 pos = Camera.main.WorldToScreenPoint(child.position);
                GameObject selectable = Instantiate(EnemySelectionPrefab, Selections.transform);
                selectable.transform.position = pos;
                selectable.transform.rotation = transform.rotation;
                selectable.GetComponent<Button>().onClick.AddListener(delegate { SelectEnemy(child); });
                if (i == 0) selectable.GetComponent<Button>().Select();
            }
        }
        else
        {
            ClearTab(Selections);
        }
    }

    public void SetEnemyHealthBars(bool value)
    {
        if (value)
        {
            Transform Enemies = BattleManager.Enemies;
            for (int i = 0; i < Enemies.childCount; i++)
            {
                Transform child = Enemies.GetChild(i);
                Vector3 pos = Camera.main.WorldToScreenPoint(child.position);
                GameObject healthBar = Instantiate(HealthBarPrefab, HealthBars.transform);
                healthBar.transform.position = pos + new Vector3(0, 150f, 0);
                healthBar.transform.rotation = transform.rotation;
                StatsData stats = BattleManager.agilityOrder[int.Parse(child.name)].GetStatsData();
                healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = ((float)stats.HealthPoints) / ((float)stats.MaxHealthPoints);
            }
        }
        else
        {
            ClearTab(HealthBars);
        }
    }

    public void RefreshEnemyStatusEffects()
    {
        ClearTab(StatusEffects);
        Transform Enemies = BattleManager.Enemies;
        for (int i = 0; i < Enemies.childCount; i++)
        {
            Transform child = Enemies.GetChild(i);
            StatusEffectsData statusEfs = BattleManager.agilityOrder[int.Parse(child.name)].GetStatusEffectsData();
            if (statusEfs.statusEffects.Count > 0)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(child.position);
                GameObject statusEffectsBar = Instantiate(StatusEffectsPrefab, StatusEffects.transform);
                statusEffectsBar.transform.position = pos + new Vector3(0, 170f, 0);
                statusEffectsBar.transform.rotation = transform.rotation;
                foreach (StatusEffect statusEffect in statusEfs.statusEffects)
                {
                    GameObject statusEffectPres = Instantiate(StatusEffectPresenterPrefab, statusEffectsBar.transform);
                    statusEffectPres.transform.GetChild(0).GetComponent<Image>().sprite = statusEffect.image;
                }
            }
        }
    }

    public void RefreshEnemyHealthBars()
    {
        ClearTab(HealthBars);
        Transform Enemies = BattleManager.Enemies;
        for (int i = 0; i < Enemies.childCount; i++)
        {
            Transform child = Enemies.GetChild(i);
            Vector3 pos = Camera.main.WorldToScreenPoint(child.position);
            GameObject healthBar = Instantiate(HealthBarPrefab, HealthBars.transform);
            healthBar.transform.position = pos + new Vector3(0, 150f, 0);
            healthBar.transform.rotation = transform.rotation;
            StatsData stats = BattleManager.agilityOrder[int.Parse(child.name)].GetStatsData();
            healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = ((float)stats.HealthPoints) / ((float)stats.MaxHealthPoints);
        }
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

    // Finds transform from list of parent's children based on mouse position
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



    public void SetTurn()
    {
        PopulatePartyTab();
        DisplayMainActionOptions();
    }

    public void DisplayMainActionOptions()
    {
        LeftTC.SetTab(ActionsPresenter);
        ClearTab(ActionsPresenter);
        GameObject attackButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
        attackButton.GetComponent<Button>().onClick.AddListener( delegate { StartSelectEnemy("Atk"); } );

        GameObject skillsButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        skillsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skills";
        skillsButton.GetComponent<Button>().onClick.AddListener( delegate { LeftTC.SetTab(SkillsPresenter); } );

        GameObject guardButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        guardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Guard";
        guardButton.GetComponent<Button>().onClick.AddListener( delegate { BattleManager.AddActionToQueue(Guard.New(BattleManager.GetCurrentTurnTaker())); } );

        GameObject itemButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Item";

        GameObject fleeButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        fleeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Flee";
        fleeButton.GetComponent<Button>().onClick.AddListener( delegate { BattleManager.Flee(); } );

        attackButton.GetComponent<Button>().Select();
    }

    private void StartSelectEnemy(string actionType)
    {
        //StopTextCoroutine();
        //RevealTextCoroutine = TextMethods.RevealText("Select your target", TextPresenter, 0.1f);
        //StartCoroutine(RevealTextCoroutine);
        currentActionType = actionType;
        SetSelectEnm(true);
        LeftTC.SetTab(SelectTargetLeftPanel);
        //SelectTargetLeftPanel.GetComponentInChildren<Button>().Select();
        //BattleManager.AddActionToQueue(actionType.New(BattleManager.GetCurrentTurnTaker(),));
    }

    public void DisplayTurnOrder()
    {
        ClearTab(TurnOrderPresenter);
        List<BattleParticipant> participants = BattleManager.agilityOrder;
        foreach (BattleParticipant participant in participants)
        {
            Sprite image = null;
            Transform imagePresenter = null;
            if (participant.IsPartyMember)
            {
                GameObject p = Instantiate(OrderCharPresenter, TurnOrderPresenter.transform);
                foreach (Transform child in p.transform)
                {
                    imagePresenter = child;
                    image = participant.GetPartyMember().image;
                }
            } else
            {
                GameObject p = Instantiate(OrderCharPresenter, TurnOrderPresenter.transform);
                foreach (Transform child in p.transform)
                {
                    imagePresenter = child;
                    image = participant.GetEnemy().image;
                }
            }
            imagePresenter.GetComponent<Image>().sprite = image;
            imagePresenter.GetComponent<RectTransform>().sizeDelta = image.textureRect.size;
        }
    }

    private void SelectEnemy(Transform Enemy)
    {
        SetSelectEnm(false);
        //StopTextCoroutine();
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
            return a;
        }
        return null;
    }

    public void StopTextCoroutine()
    {
        if (RevealTextCoroutine != null)
            StopCoroutine(RevealTextCoroutine);
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
                partyMemberPresenter.transform.Find("HealthBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)partyMember.stats.HealthPoints) / ((float)partyMember.stats.MaxHealthPoints);
                partyMemberPresenter.transform.Find("HealthStats").GetComponent<TextMeshProUGUI>().text = partyMember.stats.HealthPoints + "/" + partyMember.stats.MaxHealthPoints;
                Transform statusEffectsPresenter = partyMemberPresenter.transform.Find("StatusEffectsPresenter");
                foreach(StatusEffect statusEffect in partyMember.statusEffects.statusEffects)
                {
                    GameObject statusEffectPres = Instantiate(StatusEffectPresenterPrefab, statusEffectsPresenter.transform);
                    statusEffectPres.transform.GetChild(0).GetComponent<Image>().sprite = statusEffect.image;
                }
                partyMemberPresenter.name = i.ToString();
                if (i == BattleManager.currentTurn)
                {
                    partyMemberPresenter.GetComponent<Image>().color = partyMemberHighlightColor;
                }
            }
        }
    }

    public void UpdatePartyTabStats()
    {
        for (int i = 0; i < PartyPresenter.transform.childCount; i++)
        {
            Transform memberPresenter = PartyPresenter.transform.GetChild(i);
            int orderId = int.Parse(memberPresenter.gameObject.name);
            PartyCharacterData partyMember = BattleManager.agilityOrder[orderId].GetPartyMember();

            if (partyMember.stats.HealthPoints <= 0)
            {
                memberPresenter.transform.Find("Image").GetComponent<Image>().sprite = CrossImage;
            }

            memberPresenter.transform.Find("HealthBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)partyMember.stats.HealthPoints) / ((float)partyMember.stats.MaxHealthPoints);
            memberPresenter.transform.Find("HealthStats").GetComponent<TextMeshProUGUI>().text = partyMember.stats.HealthPoints + "/" + partyMember.stats.MaxHealthPoints;
            Transform statusEffectsPresenter = memberPresenter.transform.Find("StatusEffectsPresenter");
            ClearTab(statusEffectsPresenter.gameObject);
            foreach (StatusEffect statusEffect in partyMember.statusEffects.statusEffects)
            {
                GameObject statusEffectPres = Instantiate(StatusEffectPresenterPrefab, statusEffectsPresenter.transform);
                statusEffectPres.transform.GetChild(0).GetComponent<Image>().sprite = statusEffect.image;
            }
        }
    }

    public void SetPartyMemberColor(int member, Color color)
    {
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
