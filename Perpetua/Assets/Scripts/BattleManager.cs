using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public List<EnemyData> EnemyData;
    public List<PartyCharacterData> party;
    public List<BattleParticipant> agilityOrder;
    public int currentTurn;
    public BattleCanvas BattleCanvas;
    public GameObject EnemyPresenter;
    public Queue<BattleAction> actionQueue;
    public GameObject MissText;
    public Transform EnemyPositions;
    public Transform Enemies;
    public GameObject EnemyPrefab;
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
        party = PartyManager.Instance.party.PartyMembers;
        actionQueue = new Queue<BattleAction>();
        SortOrderList();
        currentTurn = -1;
        SpawnEnemies();
        BattleCanvas.DisplayTurnOrder();
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
                spawnLocationId++;
            }
        }
    }

    private int GetOutOfActionPartyCount()
    {
        int count = 0;
        foreach(PartyCharacterData member in party)
        {
            if (member.stats.HealthPoints <= 0)
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
                    Debug.Log(participant.GetPartyMember().name + "'s turn");
                    BattleCanvas.SetTurn();
                }
                else
                {
                    Debug.Log(participant.GetEnemy().name + "'s turn");
                    BattleCanvas.LeftTC.SetTab(BattleCanvas.ActionsPresenter);
                    BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);
                    EnemyTurn();
                }
            }
        }
        else
        {
            currentTurn = -1;
            //BattleCanvas.SetTurn();
            BattleCanvas.LeftTC.SetTab(BattleCanvas.ActionsPresenter);
            BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);
            CommitNextAction();
            return;
        }
    }

    public void EnemyTurn()
    {
        BattleParticipant participant = agilityOrder[currentTurn];
        if (UnityEngine.Random.Range(0, 100) > 25)
        {
            int recipientId = UnityEngine.Random.Range(0, party.Count);

            Attack attack = new Attack();
            attack.attacker = participant;
            attack.recipient = GetPartyMemberFromNumber(recipientId);
            AddActionToQueue(attack);
        }
        else
        {
            AddActionToQueue(Guard.New(participant));
        }
    }

    private BattleParticipant GetPartyMemberFromNumber(int memberId)
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
        if (actionQueue.Count > 0)
        {
            BattleAction action = actionQueue.Dequeue();
            action.CommitAction();

            //CommitNextAction();
        }
        else
        {
            TakeTurn();
        }
    }

    public BattleParticipant GetCurrentTurnTaker()
    {
        return agilityOrder[currentTurn];
    }

    private void EndGame()
    {
        BattleCanvas.ThanksForPlaying.SetActive(true);
    }
    /*
    private void GuardAsPlayer(int turnTakerId)
    {
        Debug.Log("Player guards (Not implemented)");
        CommitNextAction();
    }

    private void GuardAsEnemy(int turnTakerId)
    {
        Debug.Log("Enemy guards (Not implemented)");
        CommitNextAction();
    }

    private void AttackEnemy(int turnTakerId)
    {
        PartyCharacterData partyMember = (PartyCharacterData)agilityOrder[turnTakerId];
        // Currently static accuracy, changing further in the future (StatsData accuracy)
        if (((PartyCharacterData)agilityOrder[turnTakerId]).stats.healthPoints > 0)
        {
            Color color = Color.blue;
            color.a = 0.5f;
            Debug.Log(turnTakerId);
            BattleCanvas.SetPartyMemberColor(turnTakerId, color);
            if (UnityEngine.Random.Range(0, 100) > 25)
            {
                EnemyData.stats.healthPoints -= (partyMember.stats.baseAttack - EnemyData.stats.armor);
                StartCoroutine(DisplayAttackEffectCoroutine(turnTakerId));
                Debug.Log(EnemyData.stats.healthPoints);
            }
            else
            {
                StartCoroutine(DisplayMissCoroutine(turnTakerId));
            }
        }
        else
        {
            CommitNextAction();
        }
    }

    IEnumerator DisplayAttackEffectCoroutine(int turnTakerId)
    {
        HealthValue.fillAmount = EnemyData.stats.healthPoints / EnemyData.stats.maxHealth;
        BattleCanvas.DisplayAttackEffect();
        yield return new WaitForSeconds(1f);
        BattleCanvas.StopDisplayingAttackEffect();
        BattleCanvas.ResetPartyMemberColor(turnTakerId);
        if (EnemyData.stats.healthPoints <= 0)
        {
            StartCoroutine(DisplayEnemyDeathAnimationCoroutine());
            Debug.Log("Fight over, enemy is dead");
        } else
        {
            CommitNextAction();
        }
    }

    IEnumerator DisplayMissCoroutine(int turnTakerId)
    {
        MissText.SetActive(true);
        yield return new WaitForSeconds(1f);
        MissText.SetActive(false);
        BattleCanvas.ResetPartyMemberColor(turnTakerId);
        CommitNextAction();
    }

    IEnumerator DisplayEnemyDeathAnimationCoroutine()
    {
        EnemyPresenter.GetComponent<BattleEnemyAnimator>().PlayDeathAnimation();
        yield return new WaitForSeconds(1f);
        EndGame();
    }

    private void AttackPlayer(int recipient)
    {
        PartyCharacterData partyMember = (PartyCharacterData)agilityOrder[recipient];
        bool miss = true;
        float healthBefore = partyMember.stats.healthPoints;

        if (UnityEngine.Random.Range(0, 100) > 25)
        {
            partyMember.stats.healthPoints -= (EnemyData.stats.baseAttack - partyMember.stats.baseDefense);
            if (healthBefore > 0 && partyMember.stats.healthPoints <= 0)
            {
                deadPartyCount += 1;
            }
            BattleCanvas.PopulatePartyTab();
            miss = false;
        }
        StartCoroutine(DisplayEnemyAttackAnimationCoroutine(partyMember, recipient, miss));
    }

    IEnumerator DisplayEnemyAttackAnimationCoroutine(PartyCharacterData partyMember, int recipient, bool miss)
    {
        EnemyPresenter.GetComponent<BattleEnemyAnimator>().PlayAttackAnimation();
        yield return new WaitForSeconds(0.5f);
        Color color = Color.red;
        color.a = 0.5f;
        BattleCanvas.SetPartyMemberColor(recipient, color);
        if (miss)
        {
            MissText.SetActive(true);
        }
        yield return new WaitForSeconds(0.75f);
        BattleCanvas.ResetPartyMemberColor(recipient);
        if (miss)
        {
            yield return new WaitForSeconds(0.5f);
            MissText.SetActive(false);
        }
        if (partyMember.stats.healthPoints <= 0)
        {
            Debug.Log("Party member has died");
        }
        CommitNextAction();
    }*/

    private void SortOrderList()
    {
        List<BattleParticipant> battleParticipants = new List<BattleParticipant>();
        foreach(PartyCharacterData member in party)
        {
            battleParticipants.Add(BattleParticipant.New(member));
        }
        foreach(EnemyData enemy in EnemyData)
        {
            battleParticipants.Add(BattleParticipant.New(enemy));
        }

        battleParticipants = battleParticipants.OrderByDescending(x => x.Agility()).ToList();
        
        agilityOrder = battleParticipants;
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
