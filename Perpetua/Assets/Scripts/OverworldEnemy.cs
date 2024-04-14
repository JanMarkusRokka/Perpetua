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
    public Vector3 RoamingDestination;
    private Vector3 KnownPlayerLocation;
    private float stopFollowing;
    private Transform playerTransform;
    public float FightTriggerDistance = 1;
    public EnemyData EnemyData;
    public Objective ObjectiveToAdvance;
    public bool Roaming;
    public float Speed;
    public float RoamingSpeed;
    private SpriteRenderer _sr;
    private SphereCollider _sc;
    public List<GameObject> EnemyGroup;

    private void Awake()
    {
        Debug.Log(EnemyData.GetInstanceID());
        IdleLocation = transform.position;
        RoamingDestination = IdleLocation;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.speed = Speed;
        EnemyData enemyData = EnemyData.Clone();
        _sr = GetComponent<SpriteRenderer>();
        _sc = GetComponent<SphereCollider>();
        SetupEnemy(enemyData);
    }

    public void SetupEnemy(EnemyData enemyData)
    {
        EnemyData = enemyData;
        EnemyData.objectiveToAdvance = ObjectiveToAdvance;
        if (enemyData.stats.HealthPoints <= 0)
        {
            if (EnemyData.isStaticDialogueEnemy)
            {
                _sr.sprite = EnemyData.gonerSprite;
                GetComponent<DialogueObject>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(true);
            }
            foreach(ItemData item in EnemyData.loot)
            {
                Events.ReceiveItem(item.Clone());
            }
            EnemyData.loot = new();

            agent.enabled = false;
            GetComponent<SpriteRenderer>().sprite = enemyData.gonerSprite;
            agent.transform.position = enemyData.stunLocation + new Vector3(0f, 1f, 0f);
            transform.position = enemyData.stunLocation + new Vector3(0f, 1f, 0f);
            enabled = false;
        } else if (enemyData.stunSeconds > 0)
        {
            float time = enemyData.stunSeconds;
            transform.position = enemyData.stunLocation + new Vector3(0f, 1f, 0f);
            agent.SetDestination(transform.position);
            agent.transform.position = enemyData.stunLocation + new Vector3(0f, 1f, 0f);
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
        if (agent.nextPosition.x > transform.position.x) _sr.flipX = true;
        else _sr.flipX = false;
        if (stopFollowing > Time.time) {
            agent.speed = Speed;
            if (Vector3.Distance(playerTransform.position, transform.position) < FightTriggerDistance)
            {
                TriggerFight();
            }
            FollowPlayer(); 
        }
        else if (Roaming) 
        {
            agent.speed = RoamingSpeed;
            Roam();
        } // Not roaming - stay at idle location
        else if (Vector3.Distance(transform.position, IdleLocation) > 1)
        {
            agent.speed = RoamingSpeed;
            GoToLocation(IdleLocation);
        }
    }

    private void Roam()
    {
        if (Vector3.Distance(transform.position, new Vector3(RoamingDestination.x, transform.position.y, RoamingDestination.z)) > 1)
        {
            GoToLocation(RoamingDestination);
        }
        else
        {   // at selected roaming destination, select new one
            RoamingDestination = IdleLocation + new Vector3(Random.Range(-_sc.radius, _sc.radius), 0, Random.Range(-_sc.radius, _sc.radius));

        }
    }

    public void TriggerFight()
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

    private void GoToLocation(Vector3 position)
    {
        agent.SetDestination(position);
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
