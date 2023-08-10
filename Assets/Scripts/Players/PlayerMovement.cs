using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    [Tooltip("Mouse x Sensitivity")]
    public float sensX;
    [Tooltip("Mouse y Sensitivity")]
    public float sensY;

    [Header("Object Reference")]
    private CharacterController characterController;
    public Transform head;

    [Header("Movement Speed")]
    [Tooltip("Player Base Speed")]
    public float baseSpeed;
    [Tooltip("Player Speed")]
    public float speed;
    [Tooltip("Player Sprint Speed Multiplier")]
    public float sprintMultiplier;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        speed = baseSpeed;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseLook();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.forward * moveVertical + transform.right * moveHorizontal;
        movementDirection.y = 0f;

        characterController.Move(speed * Time.deltaTime * movementDirection);

        HandleSprint();
#elif UNITY_ANDROID
    // handle Android Movement
#endif
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 75f);
        yRotation += mouseX;

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        head.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed * sprintMultiplier;
        }
        else
        {
            if(speed > baseSpeed)
                speed = baseSpeed;
        }
    }
}