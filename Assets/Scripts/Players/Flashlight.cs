using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [HideInInspector]public Light flashlight;

    Transform cameraPos;

    [HideInInspector]public bool isFlashlightEnabled = true;

    void Start()
    {
        flashlight = GetComponent<Light>();
        cameraPos = GameObject.Find("Camera Pos").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isFlashlightEnabled)
        {
            isFlashlightEnabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isFlashlightEnabled)
        {
            isFlashlightEnabled = true;
        }

        if (isFlashlightEnabled)
            flashlight.intensity = 2f;
        else
            flashlight.intensity = 0f;

        transform.rotation = cameraPos.rotation;
    }
}
