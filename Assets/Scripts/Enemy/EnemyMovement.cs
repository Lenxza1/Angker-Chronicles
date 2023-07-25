using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using System.IO;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoint;
    private int destinationPoint = 0;
    private int posFailSafe;

    EnemyFOV detectedPlayer;

    [HideInInspector]
    public NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void chase()
    {
        detectedPlayer = GetComponent<EnemyFOV>();
        agent.speed = agent.speed * 1.5f;
    }

    public void patrol()
    {
        if (patrolPoint.Length == 0)
            return;
        
        agent.SetDestination(patrolPoint[destinationPoint].position);
        posFailSafe = destinationPoint;
        agent.speed = 12f;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (posFailSafe != destinationPoint && !detectedPlayer.playerSeen)
            {
                destinationPoint = Random.Range(0, patrolPoint.Length);
                patrol();
            }
            else
                destinationPoint = Random.Range(0, patrolPoint.Length);
        }

        if (detectedPlayer.playerSeen)
        {
            agent.SetDestination(detectedPlayer.target.transform.position);
        }        
    }
}
