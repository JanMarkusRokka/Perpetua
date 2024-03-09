using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldEnemy : MonoBehaviour
{
    public float AggroTime;
    private NavMeshAgent agent;
    Vector3 velocity = Vector3.zero;
    public Vector3 IdleLocation;
    private Vector3 KnownPlayerLocation;
    private float stopFollowing;
    private Transform playerTransform;
    public float FightTriggerDistance = 1;
    public EnemyData EnemyData;

    private void Awake()
    {
        IdleLocation = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        EnemyData enemyData = EnemyData.Clone(EnemyData);
        enemyData.stats = StatsData.Clone(EnemyData.stats);
        SetupEnemy(enemyData);
    }

    public void SetupEnemy(EnemyData enemyData)
    {
        EnemyData = enemyData;
        if (enemyData.stats.HealthPoints <= 0)
        {
            agent.enabled = false;
            GetComponent<SpriteRenderer>().sprite = enemyData.gonerSprite;
            enabled = false;
        } else if (enemyData.stunSeconds > 0)
        {
            float time = enemyData.stunSeconds;
            transform.position = enemyData.stunLocation;
            agent.SetDestination(transform.position);
            agent.transform.position = enemyData.stunLocation;
            enemyData.stunSeconds = 0f;
            GetComponent<DisableScriptForSeconds>().DisableNavAgent(time);
            GetComponent<DisableScriptForSeconds>().DisableOverworldEnemy(time);
        }
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (stopFollowing > Time.time) {
            if (Vector3.Distance(playerTransform.position, transform.position) < FightTriggerDistance)
            {
                TriggerFight();
            }
            FollowPlayer(); 
        }
        else if (Vector3.Distance(transform.position, IdleLocation) > 1) {
            GoToIdleLocation(); 
        }
    }

    private void TriggerFight()
    {
        EnemyData.stunLocation = transform.position;
        Events.TriggerBattle(gameObject, playerTransform.gameObject);
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player)
        {
            //Debug.Log("Player entered aggro area, following");
            playerTransform = player.transform;
            KnownPlayerLocation = playerTransform.position;
            stopFollowing = Time.time + 120;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player)
        {
            KnownPlayerLocation = player.transform.position;
            stopFollowing = Time.time + AggroTime;
        }
    }

    private void GoToIdleLocation()
    {
        agent.SetDestination(IdleLocation);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.001f);
    }

    private void FollowPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (playerTransform.position - transform.position), out hit, Vector3.Distance(transform.position, playerTransform.position) - 1f))
        {
            //Debug.Log(hit.collider.name);
            //Debug.Log("Moving to last known player location");
            agent.SetDestination(KnownPlayerLocation + new Vector3(0f, 1f, 0f));
        }
        else
        {
            //Debug.Log("Moving to player");
            agent.SetDestination(playerTransform.position + new Vector3(0f, 1f, 0f));
            KnownPlayerLocation = playerTransform.position;
        }
        Debug.DrawLine(transform.position, new Vector3(agent.destination.x, agent.destination.y, agent.destination.z), Color.red);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.001f);
        //transform.position = Vector3.MoveTowards(transform.position, agent.nextPosition, 10f);
    }
}
