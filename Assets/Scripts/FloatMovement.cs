using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMovement : MonoBehaviour
{
    [SerializeField]
    float horizontalSpeed;
    [SerializeField]
    float verticalSpeed;
    [SerializeField]
    float amplitude;

    [SerializeField]
    //postprocessing profile change

    private Vector3 tempPos;

	// Use this for initialization
	void Start ()
    {
        tempPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		//to be used to change post processing profile information - LERP between two different color settings
	}

    void FixedUpdate()
    {
        tempPos.x += horizontalSpeed;
        tempPos.y += Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        transform.position = tempPos;
    }
}
