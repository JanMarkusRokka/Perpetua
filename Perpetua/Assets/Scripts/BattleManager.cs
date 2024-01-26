using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public EnemyData EnemyData;
    public int enemyOrderId;
    public List<PartyCharacterData> party;
    public List<ScriptableObject> agilityOrder;
    public int currentTurn;
    public BattleCanvas BattleCanvas;
    public GameObject Enemy;
    public Queue<Tuple<int, string, int>> actionQueue;
    public GameObject MissText;
    public Image HealthValue;
    private int deadPartyCount;

    public void Awake()
    {
        if (BattleManager.Instance)
        {
            Destroy(gameObject);
        }
        else Instance = this;
        Events.OnSetEnemy += OnSetEnemy;
        deadPartyCount = 0;
    }

    void Start()
    {
        party = PartyManager.Instance.party.PartyMembers;
        actionQueue = new Queue<Tuple<int, string, int>>();
        SortOrderList();
        currentTurn = -1;
        TakeTurn();
    }

    public void TakeTurn()
    {
        currentTurn += 1;
        if (deadPartyCount == party.Count)
        {
            EndGame();
        }
        else if (currentTurn <= agilityOrder.Count - 1)
        {
            if (agilityOrder[currentTurn].GetType() == typeof(PartyCharacterData))
            {
                PartyCharacterData member = (PartyCharacterData)agilityOrder[currentTurn];
                if (member.stats.healthPoints <= 0)
                {
                    TakeTurn();
                }
            }
        }
        else
        {
            currentTurn = -1;
            BattleCanvas.SetTurn();
            BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);
            CommitNextAction();
            return;
        }
        ScriptableObject turnTaker = agilityOrder[currentTurn];
        Debug.Log(turnTaker.name);
        if (turnTaker.GetType() == typeof(PartyCharacterData))
        {
            Debug.Log(((PartyCharacterData)turnTaker).name + "'s turn");
            BattleCanvas.SetTurn();
        } 
        else if (turnTaker.GetType() == typeof(EnemyData))
        {
            Debug.Log(((EnemyData)turnTaker).name + "'s turn");
            BattleCanvas.ClearTab(BattleCanvas.ActionsPresenter);
            EnemyTurn();
        }
    }

    public void EnemyTurn()
    {
        if (UnityEngine.Random.Range(0, 100) > 25)
        {
            PartyCharacterData recipient = party[UnityEngine.Random.Range(0, party.Count)];

            AddActionToQueue(enemyOrderId, "Attack", agilityOrder.IndexOf(recipient));
        }
        else
        {
            AddActionToQueue(enemyOrderId, "Guard", -1);
        }
    }

    public void AddActionToQueue(int turnTaker, string action, int receiver)
    {
        actionQueue.Enqueue(new Tuple<int, string, int>(turnTaker, action, receiver));
        TakeTurn();
    }

    public void CommitNextAction()
    {
        if (actionQueue.Count > 0)
        {
            Tuple<int, string, int> action = actionQueue.Dequeue();
            if (action.Item2 == "Attack")
            {
                if (agilityOrder[action.Item1].GetType() == typeof(PartyCharacterData))
                {
                    AttackEnemy(action.Item1);
                } else
                {
                    AttackPlayer(action.Item3);
                }
            }
            else if (action.Item2 == "Guard")
            {
                if (agilityOrder[action.Item1].GetType() == typeof(PartyCharacterData))
                {
                    GuardAsPlayer(action.Item1);
                }
                else
                {
                    GuardAsEnemy(action.Item1);
                }
            }
        }
        else
        {
            TakeTurn();
        }
    }

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
        Enemy.GetComponent<BattleEnemyAnimator>().PlayDeathAnimation();
        yield return new WaitForSeconds(1f);
        EndGame();
    }

    private void EndGame()
    {
        BattleCanvas.ThanksForPlaying.SetActive(true);
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
        Enemy.GetComponent<BattleEnemyAnimator>().PlayAttackAnimation();
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
    }

    private void SortOrderList()
    {
        List<ScriptableObject> sortedByAgility = new List<ScriptableObject>(party.OrderByDescending(x => x.stats.agility));
        bool enemyIsSlowest = true;
        for (int i = 0; i < sortedByAgility.Count; i++)
        {
            if (((PartyCharacterData)sortedByAgility[i]).stats.agility <= EnemyData.stats.agility)
            {
                sortedByAgility.Insert(i, EnemyData);
                enemyOrderId = i;
                enemyIsSlowest = false;
                break;
            }
        }
        if (enemyIsSlowest) sortedByAgility.Add(EnemyData);
        agilityOrder = sortedByAgility;
    }

    private void OnDestroy()
    {
        Events.OnSetEnemy -= OnSetEnemy;
    }

    private void OnSetEnemy(EnemyData enemyData)
    {
        EnemyData = enemyData;

    }

    public void Attack()
    {

    }
}
