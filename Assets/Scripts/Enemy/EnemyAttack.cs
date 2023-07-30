using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttack : MonoBehaviour
{
    EnemyFOV _enemyFOV;


    public Camera playerCam;

    private int playerFOV = 60;
    void Start()
    {
        StartCoroutine(ChangePlayerFOV());
        _enemyFOV = GetComponent<EnemyFOV>();
    }

    private IEnumerator ChangePlayerFOV()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (_enemyFOV.playerSeen && playerFOV < 75)
            {
                playerFOV += 1;
                Debug.Log("fov incremented");
            }
            if (_enemyFOV.playerSeen == false && playerFOV > 60)
            {
                playerFOV -= 1;
                Debug.Log("fov decremented");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCam.fieldOfView = playerFOV;
    }
}
