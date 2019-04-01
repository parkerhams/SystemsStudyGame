using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    #region Unity Editor private Serialized Variables
    [Tooltip("float values for drag - moving drag")]
    [SerializeField]
    private float movingDrag;

    [Tooltip("float values for drag - stopping drag")]
    [SerializeField]
    private float stoppingDrag;
    
    [Tooltip("accelaration")]
    [SerializeField]
    private float thrust;

    [SerializeField]
    private Rigidbody playerRigidbody;

    [Tooltip("Player's velocity never goes over this limit")]
    [SerializeField]
    private float maxSpeed = 10f;
    #endregion

    [Tooltip("Refers to the scene's Main Camera gameobject, instead")]
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
            Accelerate();
            AdjustForMaxSpeed();
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
            //set player's angular drag to stopping drag so that player stops rotating when not accelerating
            playerRigidbody.angularDrag = stoppingDrag;
        }
    }

    private void Accelerate()
    {
        transform.rotation = freeLookCam.transform.rotation;
        playerRigidbody.drag = movingDrag;

        playerRigidbody.transform.forward = freeLookCam.transform.forward;
        playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
    }

    private void AdjustForMaxSpeed()
    {
        Vector3 newVelocity = playerRigidbody.velocity;
        
        newVelocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxSpeed);
        playerRigidbody.velocity = newVelocity;
    }
}
