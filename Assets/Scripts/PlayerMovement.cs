using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    ///<summary> Meant to affect player movement, we are using the 
    ///thrust as a gas pedal, the rigidbody for our physics and force,
    ///and the camSpeed to affect our thrust further.
    #region Serialized Variables to control player movement
    //basically accelerate value, the gas pedal
    [SerializeField]
    float thrust;
    //how fast cam moves
    [SerializeField]
    float camSpeed = 180;
    [SerializeField]
    Rigidbody playerRigidbody;
    #endregion
    ///</summary>

    #region Reading Camera values to determine direction
    //actually calling vector3 position values of the main camera
    [SerializeField]
    Transform cam;
    //heading is calling
    //[SerializeField]
    //float heading = 0;
    //movement!
    [SerializeField]
    Vector3 inputVector;
    //cam location / position values with cam.forward and cam.right
    #endregion

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //set the input to match the forward value as we call each input axis
        inputVector = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        //normalize it so the value does not stutter, multiply by the thrust to get speed changes
        inputVector = inputVector.normalized * thrust;

        //CamRotation(); ---- Now in Camera Controller, could be reference later in controller

        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        //playerRigidbody.AddForce(inputVector * thrust);
        playerRigidbody.AddForce(cam.forward * thrust);

        //Basically, the simplest addforce of camera.forward is dependent on a camera follow script -
        //for some reason, not really working on play if that follow script is deactivated in the editor
    }
}
