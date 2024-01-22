using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldEnemy : MonoBehaviour
{
    public float AggroTime;
    private NavMeshAgent agent;
    Vector3 velocity = Vector3.zero;
    private Vector3 IdleLocation;
    private Vector3 KnownPlayerLocation;
    private float stopFollowing;
    /*
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        IdleLocation = transform.position;
    }
    private void FixedUpdate()
    {
        if (stopFollowing > Time.time)
        FollowPlayer();
        else
    }

    private void OnTriggerEnter(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player)
        {
            KnownPlayerLocation = player.GetComponent<Transform>().position;
            stopFollowing = Time.time + AggroTime;
        }
    }

    private void GoToLocation()
    {

    }

    private void FollowPlayer()
    {
        agent.SetDestination(Player.position);
        Debug.DrawLine(transform.position, new Vector3(agent.destination.x, agent.destination.y + 1f, agent.destination.z), Color.red);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);
    }*/
}
