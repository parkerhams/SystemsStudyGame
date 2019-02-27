using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    CinemachineCameraOffset cam;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        if (Input.GetButton("Thrust"))
        {
            playerRigidbody.rotation = Quaternion.Euler(cam.m_Offset);

            playerRigidbody.drag = movingDrag;            
            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
        }
    }
}
