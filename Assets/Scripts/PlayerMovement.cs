﻿using System.Collections;
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

    [Tooltip("float values for angular drag - rotation movement drag")]
    [SerializeField]
    private float rotationDrag;
    
    [Tooltip("accelaration")]
    [SerializeField]
    private float thrust;

    [SerializeField]
    private Rigidbody playerRigidbody;

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
            transform.rotation = freeLookCam.transform.rotation;
            playerRigidbody.drag = movingDrag;

            playerRigidbody.transform.forward = freeLookCam.transform.forward;
            playerRigidbody.angularDrag = rotationDrag;

            playerRigidbody.AddForce(playerRigidbody.transform.forward * thrust);           
        }
        else
        {
            playerRigidbody.drag = stoppingDrag;
            //set player's angular drag to stopping drag so that player stops rotating when not accelerating
            playerRigidbody.angularDrag = stoppingDrag;
        }
    }
}
