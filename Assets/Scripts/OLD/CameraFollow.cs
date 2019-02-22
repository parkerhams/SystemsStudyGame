using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform xFormCam;
    private Transform xFormParent;

    private Vector3 localRotation;
    private float maxCamHeight = 90f;
    private float camDistance = 10f;

    public Transform camTarget;

    public float smoothSpeed = 10f;
    public Vector3 offset;

    public float mouseSensitivity = 3f;
    public float orbitDampening = 10f;

    void Start()
    {
        this.xFormCam = this.transform;
        this.xFormParent = this.transform.parent;

    }

    void FixedUpdate()
    {
        //rotation based on cam coordiantes
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;

            localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            //clamp the y rotation to horizon so that it does not flip over the x/y
            localRotation.y = Mathf.Clamp(localRotation.y, 0f, maxCamHeight);
        }

        //Cam Rig transform position change using Euler angels
        //sets pitch and yaw of euler angles from y and x as angles - no rotation, so 0 on z
        Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        this.xFormCam.rotation = Quaternion.Lerp(this.xFormCam.rotation, qt, Time.deltaTime * orbitDampening);

        Vector3 desiredPosition = camTarget.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        //transform.LookAt(camTarget);
    }



}
