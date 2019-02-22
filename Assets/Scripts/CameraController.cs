using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera behavior variables
    //player transform values set in inspector
    [SerializeField]
    Transform target;
    //distance camera is away from player
    [Tooltip("This is going to be used for position of camera to player")]
    [SerializeField]
    Vector3 offset;
    //to be used later if we need to halt movement in any way
    [Tooltip("Temporarily to be used to check in order to change the target position")]
    [SerializeField]
    bool useOffsetValues;
    //how quickly camera rotates around target
    [Tooltip("used for camera position/rotation speed with mouse")]
    [SerializeField]
    float rotateSpeed;
    //to set a minimum value the camera cannot go below
    [Tooltip("camera has this z value relation to player")]
    [SerializeField]
    float minZHeight = .5f;
    //pivot for rotation and alignment - an empty
    [SerializeField]
    Transform pivot;
    #endregion

    void Start()
    {
        if(!useOffsetValues)
        {
            //splitting the difference between player position and camera position
            offset = target.position - transform.position;
        }

        //set this here in case we want to change where the pivot goes later
        pivot.transform.position = target.transform.position;
        //childed pivot can be send to the target at runtime
        pivot.transform.parent = target.transform;

        //get rid of mouse icon during play
        Cursor.lockState = CursorLockMode.Locked;
    }

    //with help from tutorial by gamesplusjames, learned LateUpdate is called after each frame of Update
    //useful for realigning the camera after input instead of during to avoid stutter
    void LateUpdate()
    {
        //get x position of mouse and rotate target
        float horizontalInput = Input.GetAxis("Mouse X") * rotateSpeed;
        //rotate the target based on the horizontal input of the mouse
        target.Rotate(0, horizontalInput, 0);

        //get y position of mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        //take rotation from pivot and apply to camera itself
        pivot.Rotate(-vertical, 0, 0);

        //move camera based on position of current target rotation and original offset
        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = target.eulerAngles.x;
        //create a rotation quaternion that will take our desired angle vlaues to be smoother
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        //move our camera to our taget smoothly while keeping mind its offset and current rotation
        transform.position = target.position - (rotation * offset);

        //send cam to the target if past a certain distance
        if(transform.position.y < target.position.y)
        {
            //make sure it's within the bounds we set
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z - minZHeight);
        }

        //focus on player - could be used for other things later, too, like plants or trees to grow 
        transform.LookAt(target);

    }
    //**Possibly Redundant, but another version of what's being called in update above here,
    //does not have the float value heading for cam offset 

    //followed along with tutorial from lmhpoly - but now calling this function causes bad camera glitches. 
    //Keeping for reference later
    //void CamRotation()
    //{
    //    //heading, in this case, is meant to take in the mouse X and set a movement speed for the camera
    //    heading += Input.GetAxis("Mouse X") * Time.deltaTime*camSpeed;
    //    //with a euler angle, calling that rotation to be set
    //    cam.rotation = Quaternion.Euler(0, heading, 0);

    //    //inputVector is basically referencing the direct input of player and setting it a new vector 3 that calls the input axis
    //    inputVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    //    //clamp magnitude being called so we can bind it to this value
    //    inputVector = Vector3.ClampMagnitude(inputVector, 1);

    //    //calling these values and normalizing them sets a limit on y movement to avoid unwated rotation, ideally
    //    cam.forward.y = 0;
    //    cam.right.y = 0;

    //    cam.right.normalized;
    //    cam.forward.normalized;

    //    //calling the forward transform of the player to be added based on the cam axes and input axes
    //    transform.forward += (transform.position += (cam.forward * inputVector.y + cam.right * inputVector.x) * Time.deltaTime * thrust);

    //    //using Vector3s to more directly set the cam/player positions together
    //    Vector3 relativePos = cam.position - transform.position;
    //    Quaternion playerCamRotation = Quaternion.LookRotation(relativePos, Vector3.up);
    //    transform.rotation = playerCamRotation;

    //}
}
