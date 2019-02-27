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

    //[Tooltip("Used for the new rotation direction as set by camera aim")]
    //[SerializeField]
    //private Vector3 direction;

    [SerializeField]
    private Rigidbody playerRigidbody;

    [Tooltip("This FreeLook refers to the values of a Cinemachine camera, " +
        "in this case Free Look Cam")]
    [SerializeField]
    CinemachineFreeLook freeLookCam;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        if (Input.GetButton("Thrust"))
        {
            //Vector3 targetDir = freeLookCam.transform.position - playerRigidbody.transform.position;

            //// The step size is equal to speed times frame time.
            //float step = 3f * Time.deltaTime;

            //Vector3 newDir = Vector3.RotateTowards(playerRigidbody.transform.forward, targetDir, step, 0.0f);

            //playerRigidbody.transform.rotation = Quaternion.LookRotation(newDir);

            //playerRigidbody.transform.rotation = Quaternion.Euler(step, freeLookCam.m_Heading.m_Bias, thrust);
            playerRigidbody.transform.rotation = Quaternion.Euler(freeLookCam.transform.position.x, freeLookCam.transform.position.y, freeLookCam.transform.position.z);

            playerRigidbody.drag = movingDrag;            
            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
        }
    }
}
