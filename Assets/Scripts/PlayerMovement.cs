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

    [Tooltip("This FreeLook refers to the values of a Cinemachine camera, " +
        "in this case Free Look Cam")]
    [SerializeField]
    Camera freeLookCam;

    [Tooltip("The object we rotate to show which direction the player is aiming.")]
    [SerializeField]
    private Transform pivotPoint;

    [SerializeField]
    Vector3 aimDirection;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        aimDirection = new Vector3(freeLookCam.transform.position.x, 0, freeLookCam.transform.position.y);

        UpdateAimIndicator();
        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        if (Input.GetButton("Thrust"))
        {
            playerRigidbody.drag = movingDrag;            
            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
        }
        else if(Input.GetButton("Jump"))
        {
            SetPlayerRotation();
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
        }
    }

    void SetPlayerRotation()
    {
        transform.rotation = Quaternion.Euler(freeLookCam.transform.forward);
    }

   

/// <summary>
/// Rotate and scale the pivot point based on the aim direction.
/// Our aim indicator is a child of the pivot point game object,
/// so it will rotate and scale along with it's parent.
/// </summary>
    private void UpdateAimIndicator()
    {
        // Magnitude is the "length" of the vector.
        // If we're not pushing past our deadzone, ignore input.
        // If we are, rotate and scale the pivot point based on the input.
        // The rotation will indicate the direction we will putt in.
    
        pivotPoint.rotation = Quaternion.LookRotation(aimDirection, transform.up);
        pivotPoint.localScale = new Vector3(pivotPoint.localScale.x, pivotPoint.localScale.y, aimDirection.magnitude);
        
    }

}
