using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Camera _playerCam; // Variabel private untuk menyimpan referensi Camera
    public Camera PlayerCam
    {
        get => _playerCam;
        set => _playerCam = value;
    }
    [SerializeField] private int initialPlayerFOV = 60;
    [SerializeField] private int maxPlayerFOV = 75;
    [SerializeField] private int fovChangeRate = 1;

    private int playerFOV;

    private EnemyFOV _enemyFOV;

    private void Start()
    {
        playerFOV = initialPlayerFOV;
        _enemyFOV = GetComponent<EnemyFOV>();
        StartCoroutine(ChangePlayerFOV());
    }

    private IEnumerator ChangePlayerFOV()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            if (_enemyFOV.playerSeen && playerFOV < maxPlayerFOV)
            {
                playerFOV = Mathf.Clamp(playerFOV + fovChangeRate, initialPlayerFOV, maxPlayerFOV);
                Debug.Log("fov incremented");
            }

            if (!_enemyFOV.playerSeen && playerFOV > initialPlayerFOV)
            {
                playerFOV = Mathf.Clamp(playerFOV - fovChangeRate, initialPlayerFOV, maxPlayerFOV);
                Debug.Log("fov decremented");
            }
        }
    }

    private void Update()
    {
        if (_enemyFOV.playerSeen)
        {
            // Set FOV kamera pemain jika pemain terlihat oleh musuh
            PlayerCam.fieldOfView = playerFOV;
        }
        else
        {
            // Atur FOV kamera pemain ke nilai awal jika pemain tidak terlihat oleh musuh
            PlayerCam.fieldOfView = initialPlayerFOV;
        }
    }
}
