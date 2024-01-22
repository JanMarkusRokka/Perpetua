using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldPartyMember : MonoBehaviour
{
    public PartyCharacterData CharacterData;
    public Transform Player;
    private NavMeshAgent agent;
    Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    public void SetupCharacter(PartyCharacterData _CharacterData, Transform _Player)
    {
        CharacterData = _CharacterData;
        Player = _Player;
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
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);
    }
}
