using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyFOV : MonoBehaviour
{
    [Header("Radius of The FOV")]
    [Tooltip("The radius in which the AI see")]
    public float radius;
    [Tooltip("The angle in which the AI see")]
    [Range(0, 360)]
    public float angle;

    public GameObject[] Player;

    [Header("The Mask Configuration")]
    [Tooltip("The target mask which the AI chase after")]
    public LayerMask targetMask;
    [Tooltip("The target mask which the AI can't see through")]
    public LayerMask obstruction;

    [HideInInspector]
    public bool playerSeen { get; private set; } = false;
    public Transform target { get; private set; }
    public Collider[] checks { get; private set; }

    private float distanceToTarget;

    EnemyMovement _enemyMovement;
    EnemyAttack _enemyAttack;

    private void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(FovRoutine());
       
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if(playerSeen)
        {
            _enemyMovement.chase();
            _enemyMovement.agent.autoBraking = false;
            _enemyAttack.playerCam = target.GetComponentInChildren<Camera>();
        }
        else
        {
            _enemyMovement.patrol();
            _enemyMovement.agent.autoBraking = true;
            _enemyAttack.playerCam = null;
        }
            
    }

    private IEnumerator FovRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.75f);
        
        while (true)
        {
            yield return wait;
            checkFOV();
        }
    }

    private void checkFOV()
    {
        checks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (checks.Length != 0)
        {
            Transform initialTarget = checks[0].transform;
            float bestTargetPos = Vector3.Distance(transform.position, initialTarget.position);
            Transform bestTarget = checks[0].transform;
            for (int i = 0; i < checks.Length; i++)
            {
                initialTarget = checks[i].transform;
                distanceToTarget = Vector3.Distance(transform.position, initialTarget.position);

                if(distanceToTarget <= bestTargetPos)
                {
                    bestTargetPos = distanceToTarget;
                    bestTarget = checks[i].transform;
                    target = bestTarget;
                }
            }
            Vector3 directionToTarget = (bestTarget.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {

                if (!Physics.Raycast(transform.position, directionToTarget, bestTargetPos, obstruction))
                {
                    playerSeen = true;
                }
                else
                    playerSeen = false;
            }
            else
                playerSeen = false;
        }
        else if (playerSeen)
             playerSeen = false;
    }
}
