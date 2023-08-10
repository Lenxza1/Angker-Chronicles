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
    [SerializeField] private int fovChangeRate = 5;

    private int playerFOV;

    EnemyFOV _enemyFOV;
    Flashlight _flashlight;

    private void Start()
    {
        playerFOV = initialPlayerFOV;
        _enemyFOV = GetComponent<EnemyFOV>();
        _flashlight = GameObject.Find("Flashlight").GetComponent<Flashlight>();
        StartCoroutine(ChangePlayerFOV());
        StartCoroutine(PlayerFlashlightFlicker());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Kena");
        }
    }

    private IEnumerator ChangePlayerFOV()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (_enemyFOV.PlayerSeen && playerFOV < maxPlayerFOV)
            {
                playerFOV = Mathf.Clamp(playerFOV + fovChangeRate, initialPlayerFOV, maxPlayerFOV);
            }

            if (!_enemyFOV.PlayerSeen && playerFOV > initialPlayerFOV)
            {
                playerFOV = Mathf.Clamp(playerFOV - fovChangeRate, initialPlayerFOV, maxPlayerFOV);
                if(playerFOV < initialPlayerFOV - 1)
                {
                    PlayerCam = null;
                }
            }
        }
    }

    private IEnumerator PlayerFlashlightFlicker()
    {
        if (_enemyFOV.Checks != null)
        {
            while (_enemyFOV.Checks.Length != 0)
            {
                _flashlight.flashlight.intensity = 0;

                yield return new WaitForSecondsRealtime(0.5f);

                _flashlight.flashlight.intensity = 2;
            }
        }
    }

    private void Update()
    {

        if(PlayerCam != null)
        {
            if (_enemyFOV.PlayerSeen)
            {
                // Set FOV kamera pemain jika pemain terlihat oleh musuh
                PlayerCam.fieldOfView = playerFOV;
            }
            else
            {
                // Atur FOV kamera pemain ke nilai awal jika pemain tidak terlihat oleh musuh
                PlayerCam.fieldOfView = playerFOV;
            }
        }
    }
}
