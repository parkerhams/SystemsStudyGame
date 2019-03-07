using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("float values for drag - moving drag")]
    [SerializeField]
    private float movingDrag;

    [Tooltip("float values for drag - stopping drag")]
    [SerializeField]
    private float stoppingDrag;
    
    [Tooltip("basically accelerate value, the gas pedal")]
    [SerializeField]
    private float thrust;

    [SerializeField]
    private Rigidbody playerRigidbody;

    [Tooltip("Refers to the actual scene camera - this what we refer to" +
        "to determine direction we are facing")]
    [SerializeField]
    Camera freeLookCam;

    Vector3 aimDirection;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        aimDirection = new Vector3(freeLookCam.transform.position.x, 0, freeLookCam.transform.position.y);
        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        if (Input.GetButton("Thrust"))
        {
            playerRigidbody.drag = movingDrag;            
            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
        }
        else if(Input.GetButton("AimForward"))
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, freeLookCam.transform.rotation, (freeLookCam.transform.forward.z - playerRigidbody.transform.forward.z));
            transform.rotation = freeLookCam.transform.rotation;
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
        }
    }
}
