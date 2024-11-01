using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldPartyMember : MonoBehaviour
{
    public PartyCharacterData CharacterData;
    public Transform Player;
    private NavMeshAgent agent;
    public Rigidbody _rb;
    Vector3 velocity = Vector3.zero;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //Events.OnBattleTriggered += OnBattleTriggered;
    }
    /*
    public void OnDestroy()
    {
        Events.OnBattleTriggered -= OnBattleTriggered;
    }
    */
    private void OnBattleTriggered(EnemyData enemyData)
    {
        enabled = false;
    }

    public void SetupCharacter(PartyCharacterData _CharacterData, Transform _Player)
    {
        CharacterData = _CharacterData;
        Player = _Player;
        animator.runtimeAnimatorController = _CharacterData.animatorController;
        GetComponent<SpriteRenderer>().sprite = CharacterData.image;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        agent.SetDestination(Player.position);
        Debug.DrawLine(transform.position, new Vector3(agent.destination.x, agent.destination.y + 1f, agent.destination.z), Color.red);
        //_rb.velocity = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);
    }
}
