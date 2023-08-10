using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using System.IO;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoint;
    private int destinationPoint;
    private int posFailSafe;

    [Header("Movement Speed")]
    public float baseSpeed = 12;
    public float chaseSpeedMultiplier = 1.5f;

    EnemyFOV _detectedPlayer;

    [HideInInspector]
    public NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
        destinationPoint = Random.Range(0, patrolPoint.Length);
        _detectedPlayer = GetComponent<EnemyFOV>();
    }

    public void Chase()
    {
        agent.speed = baseSpeed * chaseSpeedMultiplier;
    }

    public void Patrol()
    {
        if (patrolPoint.Length == 0)
            return;
        
        agent.SetDestination(patrolPoint[destinationPoint].position);
        posFailSafe = destinationPoint;
        agent.speed = baseSpeed;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (posFailSafe != destinationPoint && !_detectedPlayer.PlayerSeen)
            {
                destinationPoint = Random.Range(0, patrolPoint.Length);
                Patrol();
            }
            else
                destinationPoint = Random.Range(0, patrolPoint.Length);
        }

        if (_detectedPlayer.PlayerSeen)
        {
            agent.SetDestination(_detectedPlayer.Target.transform.position);
        }        
    }
}
