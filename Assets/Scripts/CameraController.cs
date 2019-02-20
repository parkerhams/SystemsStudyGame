using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //player transform values set in inspector
    public Transform target;
    //distance camera is away from player
    public Vector3 offset;
    //to be used later if we need to halt movement in any way
    public bool useOffsetValues;
    //how quickly camera rotates around target
    public float rotateSpeed;
    //to set a minimum value the camera cannot go below
    public float minZHeight = .5f;
    //pivot for rotation and alignment - an empty
    public Transform pivot;

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


}
