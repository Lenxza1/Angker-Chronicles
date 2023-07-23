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
    public Transform player;
    public Transform head;

    [Header("Movement Speed")]
    [Tooltip("Player Base Speed")]
    public float baseSpeed;
    [SerializeField]
    private float speed;
    [Tooltip("Player Sprint Speed Multiplier")]
    public float sprintMultiplier;


    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        speed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -60f, 75f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        head.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        if(Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        } 
        if(Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed * sprintMultiplier;
        }
        else
        {
            speed = baseSpeed;
        }
    }
}
