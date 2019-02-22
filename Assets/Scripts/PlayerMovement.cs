using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //float values for drag - moving drag and stopping drag
    [SerializeField]
    private float movingDrag;
    [SerializeField]
    private float stoppingDrag;


    ///<summary> Meant to affect player movement, we are using the 
    ///thrust as a gas pedal, the rigidbody for our physics and force,
    ///and the camSpeed to affect our thrust further.
    #region Serialized Variables to control player movement
    //basically accelerate value, the gas pedal
    [SerializeField]
    float thrust;
    [SerializeField]
    Rigidbody playerRigidbody;
    #endregion
    ///</summary>

    #region Reading Camera values to determine direction
    //actually calling vector3 position values of the main camera
    //heading is calling
    //[SerializeField]
    //float heading = 0;
    //movement!
    //cam location / position values with cam.forward and cam.right
    #endregion

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        //using AddForce, we multiply the input (vertical and horziontal) by the thrust
        //playerRigidbody.AddForce(inputVector * thrust);
        if (Input.GetButton("Thrust"))
        {
            playerRigidbody.drag = movingDrag;
            
            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
        }

        //Basically, the simplest addforce of camera.forward is dependent on a camera follow script -
        //for some reason, not really working on play if that follow script is deactivated in the editor
    }
}
