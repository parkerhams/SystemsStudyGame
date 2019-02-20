using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //basically accelerate value, the gas pedal
    public float thrust;
    //how fast cam moves
    public float camSpeed = 180;
    public Rigidbody rb;

    //actually calling vector3 position values of the main camera
    public Transform cam;

    float heading = 0;
    //movement!
    Vector3 inputVector;
    //cam location / position values
    Vector3 camForward;
    Vector3 camRight;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //right nw, isnce CameraRotation() is not called and causes so many issues, these are redundant - but here for revision
        camForward = cam.forward;
        camRight = cam.right;

    }

    void FixedUpdate()
    {
        //set the input to match the forward value as we call each input axis
        inputVector = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        //normalize it so the value does not stutter, multiply by the thrust to get speed changes
        inputVector = inputVector.normalized * thrust;

        //CamRotation();

        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        rb.AddForce(inputVector * thrust);
    }

    //followed along with tutorial from lmhpoly - but now calling this function causes bad camera glitches. 
    //Keeping for reference later
    void CamRotation()
    {
        //heading, in this case, is meant to take in the mouse X and set a movement speed for the camera
        heading += Input.GetAxis("Mouse X") * Time.deltaTime*camSpeed;
        //with a euler angle, calling that rotation to be set
        cam.rotation = Quaternion.Euler(0, heading, 0);

        //inputVector is basically referencing the direct input of player and setting it a new vector 3 that calls the input axis
        inputVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //clamp magnitude being called so we can bind it to this value
        inputVector = Vector3.ClampMagnitude(inputVector, 1);

        //calling these values and normalizing them sets a limit on y movement to avoid unwated rotation, ideally
        camForward.y = 0;
        camRight.y = 0;

        camRight = camRight.normalized;
        camForward = camForward.normalized;

        //calling the forward transform of the player to be added based on the cam axes and input axes
        transform.forward += (transform.position += (camForward * inputVector.y + camRight * inputVector.x) * Time.deltaTime * thrust);

        //using Vector3s to more directly set the cam/player positions together
        Vector3 relativePos = cam.position - transform.position;
        Quaternion playerCamRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = playerCamRotation;

    }
}
