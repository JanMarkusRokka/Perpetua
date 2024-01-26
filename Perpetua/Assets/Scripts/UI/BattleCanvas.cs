using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject PartyPresenter;
    public GameObject ActionsPresenter;
    public GameObject PartyMemberPresenterPrefab;
    public GameObject ActionOptionPresenterPrefab;
    public GameObject AttackEffectsPresenter;
    public GameObject ThanksForPlaying;
    public List<Sprite> BattleEffects;
    public BattleManager BattleManager;
    public Sprite CrossImage;
    private Color pmPresDefaultColor;
    private Color partyMemberHighlightColor = Color.green;
    private TabsController _tc;

    void Start()
    {
        _tc = GetComponent<TabsController>();
        _tc.tabs = new List<GameObject>
        {
            OptionsPanel
        };
        partyMemberHighlightColor.a = 0.5f;
        pmPresDefaultColor = PartyMemberPresenterPrefab.GetComponent<Image>().color;
    }

    public void DisplayAttackEffect()
    {
        AttackEffectsPresenter.GetComponent<SpriteRenderer>().sprite = BattleEffects[Random.Range(0, BattleEffects.Count)];
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
        attackButton.GetComponent<Button>().onClick.AddListener( delegate { BattleManager.Instance.AddActionToQueue(BattleManager.Instance.currentTurn, "Attack", BattleManager.Instance.enemyOrderId); } );
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

    private string StatsToText(StatsData stats)
    {
        return stats.healthPoints + "\n" + stats.agility;
    }

    public void PopulatePartyTab()
    {
        ClearTab(PartyPresenter);
        List<ScriptableObject> agilityOrder = BattleManager.Instance.agilityOrder;

        for (int i = 0; i < agilityOrder.Count; i++)
        {
            ScriptableObject turnTaker = agilityOrder[i];
            if (turnTaker.GetType() == typeof(PartyCharacterData))
            {
                PartyCharacterData partyMember = (PartyCharacterData)turnTaker;
                GameObject partyMemberPresenter = Instantiate(PartyMemberPresenterPrefab, PartyPresenter.transform);
                if (partyMember.stats.healthPoints <= 0)
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
                if (i == BattleManager.Instance.currentTurn)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsPanel.activeInHierarchy)
                _tc.DisableTabs();
            else _tc.SetTab(OptionsPanel);
        }
    }
}
