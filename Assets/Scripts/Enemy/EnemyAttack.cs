using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttack : MonoBehaviour
{
    EnemyFOV _enemyFOV;

    public Camera playerCam;

    private float playerFOV = 60f;
    public float fovChangeSpeed = 1000f;
    void Start()
    {
        StartCoroutine(ChangePlayerFOV());
        _enemyFOV = GetComponent<EnemyFOV>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            Debug.Log("Kena");
        }
    }

    private IEnumerator ChangePlayerFOV()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            float targetFOV = _enemyFOV.playerSeen ? 75f : 60f;
            if(_enemyFOV.playerSeen )
            {
                playerFOV = Mathf.MoveTowards(playerFOV, targetFOV, fovChangeSpeed * Time.deltaTime);
            }
            else
            {
                playerFOV = Mathf.MoveTowards(playerFOV, targetFOV, fovChangeSpeed * Time.deltaTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerCam.fieldOfView = playerFOV;
    }
}
