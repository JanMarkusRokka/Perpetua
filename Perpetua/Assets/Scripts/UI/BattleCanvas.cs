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
    public GameObject ItemsPresenter;

    [Header("Right tabs")]
    public GameObject PartyPresenter;
    public GameObject PartyMemberPresenterPrefab;
    public GameObject ItemPresenterPrefab;

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
    private bool SelectPM;
    private BattleAction currentAction;

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
            SkillsPresenter,
            ItemsPresenter
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

    public void SetSelectPartyMembers(bool value)
    {
        SelectPM = value;
        SetSelectionButtons(value, PartyPresenter.transform);
    }

    public void SetSelectEnm(bool value)
    {
        SelectEnm = value;
        SetSelectionButtons(value, BattleManager.Enemies);
        SetEnemyHealthBars(value);
        //SetEnemyHoverHighlight(value);
    }

    public void SetSelectionButtons(bool value, Transform trans)
    {
        if (value)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                Vector3 pos = child.position;
                if (!trans.GetComponent<RectTransform>())
                    pos = Camera.main.WorldToScreenPoint(child.position);
                GameObject selectable = Instantiate(EnemySelectionPrefab, Selections.transform);
                selectable.transform.position = pos;
                selectable.transform.rotation = transform.rotation;
                selectable.GetComponent<Button>().onClick.AddListener(delegate { SelectTarget(child); });
                if (i == 0) selectable.GetComponent<Button>().Select();
            }
        }
        else
        {
            TabsController.ClearTab(Selections);
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
            TabsController.ClearTab(HealthBars);
        }
    }

    public void RefreshEnemyStatusEffects()
    {
        TabsController.ClearTab(StatusEffects);
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
                    SetStatusEffectTooltip(statusEffectPres, statusEffect);
                }
            }
        }
    }

    public void RefreshEnemyHealthBars()
    {
        TabsController.ClearTab(HealthBars);
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
    /*
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
    }*/



    public void SetTurn()
    {
        PopulatePartyTab();
        DisplayMainActionOptions();
    }

    public void DisplayMainActionOptions()
    {
        LeftTC.SetTab(ActionsPresenter);
        TabsController.ClearTab(ActionsPresenter);
        GameObject attackButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
        attackButton.GetComponent<Button>().onClick.AddListener( delegate { StartSelectEnemy(new Attack()); } );

        GameObject skillsButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        skillsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Skills";
        skillsButton.GetComponent<Button>().onClick.AddListener( delegate { LeftTC.SetTab(SkillsPresenter); } );

        GameObject guardButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        guardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Guard";
        guardButton.GetComponent<Button>().onClick.AddListener( delegate { BattleManager.AddActionToQueue(Guard.New(BattleManager.GetCurrentTurnTaker())); } );

        GameObject itemButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Item";
        itemButton.GetComponent<Button>().onClick.AddListener(delegate { LeftTC.SetTab(ItemsPresenter); });


        GameObject fleeButton = Instantiate(ActionOptionPresenterPrefab, ActionsPresenter.transform);
        fleeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Flee";
        fleeButton.GetComponent<Button>().onClick.AddListener( delegate { BattleManager.AddActionToQueue(Flee.New(BattleManager.GetCurrentTurnTaker())); } );

        attackButton.GetComponent<Button>().Select();
    }

    public void StartSelectEnemy(BattleAction battleAction)
    {
        //StopTextCoroutine();
        //RevealTextCoroutine = TextMethods.RevealText("Select your target", TextPresenter, 0.1f);
        //StartCoroutine(RevealTextCoroutine);
        currentAction = battleAction;
        SetSelectEnm(true);
        LeftTC.SetTab(SelectTargetLeftPanel);
        //SelectTargetLeftPanel.GetComponentInChildren<Button>().Select();
        //BattleManager.AddActionToQueue(actionType.New(BattleManager.GetCurrentTurnTaker(),));
    }

    public void StartSelectPartyMember(BattleAction battleAction)
    {
        currentAction = battleAction;
        SetSelectPartyMembers(true);
        LeftTC.SetTab(SelectTargetLeftPanel);
    }

    public void DisplayTurnOrder(List<BattleParticipant> participants)
    {
        TabsController.ClearTab(TurnOrderPresenter);
        for (int i = 0; i < participants.Count; i++)
        {
            BattleParticipant participant = participants[i];
            if (participant.GetStatsData().HealthPoints > 0)
            {
                Sprite image = null;
                Transform imagePresenter = null;
                if (participant.IsPartyMember)
                {
                    GameObject p = Instantiate(OrderCharPresenter, TurnOrderPresenter.transform);
                    p.name = i.ToString();
                    foreach (Transform child in p.transform)
                    {
                        imagePresenter = child;
                        image = participant.GetPartyMember().image;
                    }
                }
                else
                {
                    GameObject p = Instantiate(OrderCharPresenter, TurnOrderPresenter.transform);
                    p.name = i.ToString();
                    foreach (Transform child in p.transform)
                    {
                        imagePresenter = child;
                        image = participant.GetEnemy().image;
                    }
                }
                imagePresenter.GetComponent<Image>().sprite = image;
                if (image.textureRect.size.magnitude > new Vector2(32f, 32f).magnitude)
                {
                    imagePresenter.GetComponent<RectTransform>().sizeDelta = image.textureRect.size / 4;
                }
                else
                    imagePresenter.GetComponent<RectTransform>().sizeDelta = image.textureRect.size;
            }
        }
    }

    private void SelectTarget(Transform target)
    {
        SetSelectEnm(false);
        SetSelectPartyMembers(false);
        //StopTextCoroutine();
        TextPresenter.text = "";
        BattleAction action = currentAction.CreateFromUI(new List<BattleParticipant> { BattleManager.agilityOrder[BattleManager.currentTurn], BattleManager.agilityOrder[int.Parse(target.name)] });
        BattleManager.AddActionToQueue(action);
    }

    public void StopTextCoroutine()
    {
        if (RevealTextCoroutine != null)
            StopCoroutine(RevealTextCoroutine);
    }

    public void PopulatePartyTab()
    {
        TabsController.ClearTab(PartyPresenter);
        List<BattleParticipant> spawnedMembers = new();

        List<BattleParticipant> agilityOrder = BattleManager.agilityOrder;

        for (int i = 0; i < agilityOrder.Count; i++)
        {
            BattleParticipant turnTaker = agilityOrder[i];
            if (turnTaker.IsPartyMember)
            {
                if (!spawnedMembers.Contains(turnTaker))
                {
                    spawnedMembers.Add(turnTaker);
                    PartyCharacterData partyMember = turnTaker.GetPartyMember();
                    GameObject partyMemberPresenter = Instantiate(PartyMemberPresenterPrefab, PartyPresenter.transform);
                    turnTaker.transform = partyMemberPresenter.transform;
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
                    partyMemberPresenter.transform.Find("WillPowerBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)partyMember.stats.WillPower) / ((float)partyMember.stats.MaxWillPower);
                    partyMemberPresenter.transform.Find("WillPowerStats").GetComponent<TextMeshProUGUI>().text = partyMember.stats.WillPower + "/" + partyMember.stats.MaxWillPower;
                    Transform statusEffectsPresenter = partyMemberPresenter.transform.Find("StatusEffectsPresenter");
                    foreach (StatusEffect statusEffect in partyMember.statusEffects.statusEffects)
                    {
                        GameObject statusEffectPres = Instantiate(StatusEffectPresenterPrefab, statusEffectsPresenter.transform);
                        statusEffectPres.transform.GetChild(0).GetComponent<Image>().sprite = statusEffect.image;
                        SetStatusEffectTooltip(statusEffectPres, statusEffect);
                    }
                    Transform runesPresenter = partyMemberPresenter.transform.Find("RunesPresenter");
                    List<ItemData> runes = new List<ItemData> { partyMember.equipment.rune1, partyMember.equipment.rune2 };
                    foreach (ItemData rune in runes)
                    {
                        if (rune)
                        {
                            GameObject itemPres = Instantiate(ItemPresenterPrefab, runesPresenter.transform);
                            Image image = itemPres.transform.GetChild(0).GetComponent<Image>();
                            image.sprite = rune.image;
                            image.GetComponent<RectTransform>().sizeDelta = rune.image.textureRect.size;
                            TooltipTrigger tooltip = itemPres.GetComponent<TooltipTrigger>();
                            tooltip.header = rune.name;
                            tooltip.description = rune.GetDescription();
                        }
                    }
                    partyMemberPresenter.name = i.ToString();
                }
                if (i == BattleManager.currentTurn)
                {
                    turnTaker.transform.GetComponent<Image>().color = partyMemberHighlightColor;
                    turnTaker.transform.GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1f);

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

            if (partyMember.GetStatsWithAllEffects().HealthPoints <= 0)
            {
                memberPresenter.transform.Find("Image").GetComponent<Image>().sprite = CrossImage;
            }

            memberPresenter.transform.Find("HealthBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)partyMember.stats.HealthPoints) / ((float)partyMember.stats.MaxHealthPoints);
            memberPresenter.transform.Find("HealthStats").GetComponent<TextMeshProUGUI>().text = partyMember.stats.HealthPoints + "/" + partyMember.stats.MaxHealthPoints;
            memberPresenter.transform.Find("WillPowerBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)partyMember.stats.WillPower) / ((float)partyMember.stats.MaxWillPower);
            memberPresenter.transform.Find("WillPowerStats").GetComponent<TextMeshProUGUI>().text = partyMember.stats.WillPower + "/" + partyMember.stats.MaxWillPower;
            Transform statusEffectsPresenter = memberPresenter.transform.Find("StatusEffectsPresenter");
            TabsController.ClearTab(statusEffectsPresenter.gameObject);
            foreach (StatusEffect statusEffect in partyMember.statusEffects.statusEffects)
            {
                GameObject statusEffectPres = Instantiate(StatusEffectPresenterPrefab, statusEffectsPresenter.transform);
                statusEffectPres.transform.GetChild(0).GetComponent<Image>().sprite = statusEffect.image;
                SetStatusEffectTooltip(statusEffectPres, statusEffect);
            }
        }
    }

    private void SetStatusEffectTooltip(GameObject gameObject, StatusEffect statusEffect)
    {
        if (statusEffect.isAilment) gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
        TooltipTrigger tooltip = gameObject.GetComponent<TooltipTrigger>();
        tooltip.header = statusEffect.tooltip;
        tooltip.description = "";
    }

    public void SetPartyMemberColor(Transform member, Color color)
    {
        member.GetComponent<Image>().color = color;
    }

    public void ResetPartyMemberColor(Transform member)
    {
        SetPartyMemberColor(member, pmPresDefaultColor);
    }
}
