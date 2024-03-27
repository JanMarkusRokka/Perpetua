using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public List<EnemyData> EnemyData;
    public List<BattleParticipant> party;
    public List<BattleParticipant> agilityOrder;
    public int currentTurn;
    public BattleCanvas BattleCanvas;
    public GameObject EnemyPresenter;
    public BattleActionQueue<BattleAction> actionQueue;
    public GameObject MissText;
    public Transform EnemyPositions;
    public Transform Enemies;
    public GameObject EnemyPrefab;
    [NonSerialized]
    public ScenarioData returnScenario;
    public HashSet<BattleParticipant> GuardDuringTurn;
    public void Awake()
    {
        if (BattleManager.Instance)
        {
            Destroy(gameObject);
        }
        else Instance = this;
        Events.OnSetEnemy += OnSetEnemy;
    }

    void Start()
    {
        actionQueue = new();
        GuardDuringTurn = new();
        agilityOrder = CreateAgilityOrderList(PartyManager.Instance.party.PartyMembers, EnemyData);
        party = BattleParticipant.GetPartyMembers(agilityOrder);
        currentTurn = -1;
        SpawnEnemies();
        BattleCanvas.DisplayTurnOrder(agilityOrder);
        BattleCanvas.SetSelectEnm(false);
        TakeTurn();
    }

    private void SpawnEnemies()
    {
        int spawnLocationId = 0;

        for (int i = 0; i < agilityOrder.Count; i++)
        {
            if (!agilityOrder[i].IsPartyMember)
            {
                GameObject enemy = Instantiate(EnemyPrefab, Enemies);
                enemy.transform.position = EnemyPositions.Find("Enemy" + spawnLocationId).position;
                enemy.name = i.ToString();
                agilityOrder[i].transform = enemy.transform;
                spawnLocationId++;
            }
        }
    }

    private void UpdateEnemyNames()
    {
        for (int i = 0; i < agilityOrder.Count; i++)
        {
            BattleParticipant participant = agilityOrder[i];
            if (!participant.IsPartyMember)
            {
                participant.transform.gameObject.name = i.ToString();
            }
        }
    }

    private int GetOutOfActionPartyCount()
    {
        int count = 0;
        foreach(BattleParticipant member in party)
        {
            if (member.GetStatsData().HealthPoints <= 0)
            {
                count++;
            }
        }
        return count;
    }

    private int GetOutOfActionEnemyCount()
    {
        int count = 0;
        foreach(EnemyData enemy in EnemyData)
        {
            if (enemy.stats.HealthPoints <= 0)
            {
                count++;
            }
        }
        return count;
    }

    public void TakeTurn()
    {
        currentTurn += 1;
        
        if (GetOutOfActionPartyCount() == party.Count)
        {
            EndGame();
            return;
        }
        else if (GetOutOfActionEnemyCount() == EnemyData.Count)
        {
            foreach(EnemyData enemy in EnemyData)
            {
                PartyManager.Instance.party.objectives.Find(o => o.id == enemy.objectiveToAdvance).Advance();                
            }
            EndGame();
            return;
        }
        else if (currentTurn < agilityOrder.Count)
        {
            BattleParticipant participant = agilityOrder[currentTurn];
            if (participant.Health() <= 0)
            {
                TakeTurn();
            }
            else
            {
                if (participant.IsPartyMember)
                {
                    //Debug.Log(participant.GetPartyMember().name + "'s turn");
                    BattleCanvas.SetTurn();
                }
                else
                {
                    //Debug.Log(participant.GetEnemy().name + "'s turn");
                    BattleCanvas.PopulatePartyTab();
                    BattleCanvas.LeftTC.SetTab(BattleCanvas.ActionsPresenter);
                    BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);
                    AddEnemyTurnAction(participant);
                }
            }
        }
        else
        {
            currentTurn = -1;
            //BattleCanvas.SetTurn();
            if (agilityOrder[agilityOrder.Count - 1].IsPartyMember)
            {
                BattleCanvas.ResetPartyMemberColor(agilityOrder[agilityOrder.Count - 1].transform);
            }

            BattleCanvas.LeftTC.SetTab(BattleCanvas.ActionsPresenter);
            BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);

            ApplyStatusEffectsAndStartCommitingActions();
            return;
        }
    }

    private void ApplyStatusEffectsAndStartCommitingActions()
    {
        agilityOrder = SortAgilityOrderList(agilityOrder);
        BattleCanvas.DisplayTurnOrder(agilityOrder);
        UpdateEnemyNames(); // from agilityOrder

        StartCoroutine(WaitForLayout());
    }

    IEnumerator WaitForLayout()
    {
        yield return new WaitForEndOfFrame();
        foreach (BattleParticipant participant in agilityOrder)
        {
            List<StatusEffect> statusEffects = participant.GetStatusEffectsData().statusEffects;
            foreach (StatusEffect statusEffect in statusEffects)
            {
                statusEffect.InflictActiveStatusEffect(participant);
            }
            participant.GetStatusEffectsData().statusEffects.RemoveAll(sf => sf.GetTurnsLeft() <= 0);
        }

        BattleCanvas.RefreshEnemyStatusEffects();
        BattleCanvas.PopulatePartyTab();


        StartCoroutine(WaitBeforeNextAction(0.5f));
    }

    private IEnumerator WaitBeforeNextAction(float secs)
    {
        yield return new WaitForSeconds(secs);
        CommitNextAction();
    }

    public void AddEnemyTurnAction(BattleParticipant participant)
    {
        BattleAction action = participant.GetEnemy().SelectTurn(participant, true);
        if (action.GetType() == typeof(Guard)) AddActionToQueue(action);
        else
        {
            AddActionToQueue(EnemyTurn.New(participant));
        }
    }

    public BattleParticipant GetPartyMemberFromNumber(int memberId)
    {
        int count = 0;
        int id = 0;

        BattleParticipant member = null;

        while (true)
        {
            if (agilityOrder[id].IsPartyMember)
            {
                if (count == memberId)
                {
                    member = agilityOrder[id];
                    break;
                }
                count++;
            }
            id++;
        }

        return member;
    }

    public void AddActionToQueue(BattleAction action)
    {
        actionQueue.Enqueue(action);
        TakeTurn();
    }

    public void CommitNextAction()
    {
        if (actionQueue.Count() > 0)
        {
            BattleAction action = actionQueue.Dequeue();
            // Displays turn order advancing (character icons are removed based on action order)
            BattleCanvas.TurnOrderPresenter.transform.Find(agilityOrder.IndexOf(action.GetParticipant()).ToString()).GetComponent<TriggerAnimation>().TriggerAnim();
            StatsData stats = action.GetParticipant().GetStatsData();
            if (stats.HealthPoints > 0 && stats.AttackSpeed > 0)
            {
                action.GetParticipant().participant.stats.WillPower = Mathf.Max(0, action.GetParticipant().participant.stats.WillPower - action.GetWillPowerUsage());
                BattleCanvas.UpdatePartyTabStats();
                action.CommitAction();
            }
            else
            {
                CommitNextAction();
            }
        }
        else
        {
            GuardDuringTurn = new();
            agilityOrder = SortAgilityOrderList(agilityOrder);
            BattleCanvas.DisplayTurnOrder(agilityOrder);
            UpdateEnemyNames(); // from agilityOrder
            StartCoroutine(WaitBeforeTurn(0.5f));
        }
    }

    private IEnumerator WaitBeforeTurn(float secs)
    {
        yield return new WaitForSeconds(secs);
        TakeTurn();
    }

    public BattleParticipant GetCurrentTurnTaker()
    {
        return agilityOrder[currentTurn];
    }

    public void Flee()
    {
        Debug.Log("Flee");
        foreach(EnemyData enemy in EnemyData)
        {
            enemy.stunSeconds = 2f;
        }
        MenuPresenter.Instance.LoadSave(returnScenario);
    }

    private void EndGame()
    {
        MenuPresenter.Instance.LoadSave(returnScenario);
    }

    private List<BattleParticipant> CreateAgilityOrderList(List<PartyCharacterData> partyMembers, List<EnemyData> enemies)
    {
        List<BattleParticipant> battleParticipants = new List<BattleParticipant>();
        foreach (PartyCharacterData member in partyMembers)
        {
            battleParticipants.Add(BattleParticipant.New(member));
        }
        foreach (EnemyData enemy in enemies)
        {
            battleParticipants.Add(BattleParticipant.New(enemy));
        }
        return SortAgilityOrderList(battleParticipants);
    }

    private List<BattleParticipant> SortAgilityOrderList(List<BattleParticipant> agilityOrderList)
    {
        return agilityOrderList.OrderByDescending(x => x.GetStatsData().AttackSpeed).ToList();
    }

    private void OnDestroy()
    {
        Events.OnSetEnemy -= OnSetEnemy;
    }

    private void OnSetEnemy(List<EnemyData> enemyData)
    {
        EnemyData = enemyData;
    }
}
