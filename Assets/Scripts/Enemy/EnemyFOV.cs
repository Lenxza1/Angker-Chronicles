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
            _enemyAttack.PlayerCam = target.GetComponentInChildren<Camera>();
        }
        else
        {
            _enemyMovement.patrol();
            _enemyMovement.agent.autoBraking = true;
            _enemyAttack.PlayerCam.fieldOfView = 60;
            _enemyAttack.PlayerCam = null;
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
        // Mencari semua objek yang berada dalam radius FOV musuh dengan layer yang sesuai
        checks = Physics.OverlapSphere(transform.position, radius, targetMask);

        // Jika ada objek yang terdeteksi dalam radius FOV
        if (checks.Length != 0)
        {
            // Menemukan target terdekat berdasarkan jarak dari musuh
            Transform initialTarget = checks[0].transform;
            float bestTargetPos = Vector3.Distance(transform.position, initialTarget.position);
            Transform bestTarget = checks[0].transform;
            for (int i = 0; i < checks.Length; i++)
            {

                initialTarget = checks[i].transform;
                distanceToTarget = Vector3.Distance(transform.position, initialTarget.position);

                // Membandingkan jarak dengan target sebelumnya untuk menemukan target terdekat
                if (distanceToTarget <= bestTargetPos)
                {
                    bestTargetPos = distanceToTarget;
                    bestTarget = checks[i].transform;
                    target = bestTarget;
                }
            }

            // Menghitung arah ke target terdekat dari musuh
            Vector3 directionToTarget = (bestTarget.position - transform.position).normalized;

            // Memeriksa apakah arah ke target terdekat berada dalam sudut FOV yang diizinkan
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {

                // Memeriksa apakah arah ke target terdekat tidak dihalangi oleh objek dengan layer obstruction
                if (!Physics.Raycast(transform.position, directionToTarget, bestTargetPos, obstruction))
                {
                    // Jika tidak dihalangi, pemain terlihat oleh musuh
                    playerSeen = true;
                }
                else
                    // Jika tidak dihalangi, pemain terlihat oleh musuh
                    playerSeen = false;
            }
            else
                // Jika arah ke target terdekat berada di luar sudut FOV yang diizinkan, pemain tidak terlihat oleh musuh
                playerSeen = false;
        }
        else
            // Jika tidak ada objek yang terdeteksi dalam radius FOV, pemain tidak terlihat oleh musuh
            playerSeen = false;        
    }
}
