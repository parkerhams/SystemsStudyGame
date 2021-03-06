﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Scales objects up to a max scale if player enters trigger.
/// If player leaves trigger before reaching max scale, the object begins scaling back down to it's minimum / starting scale.
/// while the player's game object is in the trigger area of the tree.
/// Audio will play while the game object scales upwards, and it will play particle effects to show it's reached maxScale.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ScaleLerper : MonoBehaviour
{
    #region SerializedFields
    [Tooltip("biggest size it can get")] 
    [SerializeField]
    private float maxScale;

    [Tooltip("scalableObject is a reference to the location where the foliage should spawn")]
    [SerializeField]
    private GameObject scalableObject;

    [Tooltip("how quickly it grows")]
    [SerializeField]
    private float growthSpeed = 0.4f;

    [Tooltip("how quickly it shrinks")]
    [SerializeField]
    private float shrinkSpeed = 0.5f;

    [Tooltip("tell object to be set to maxScale if localScale is within this threshold")]
    [SerializeField]
    private float doneGrowingThreshold;

    [SerializeField]
    private AudioClip auraAudioClip;

    [SerializeField]
    private AudioClip completedGrowingAudioClip;

    [SerializeField]
    private ParticleSystem growthCompletedParticles;
    #endregion

    private float particleDuration = 2f;
    private AudioSource audioSource;

    private bool IsAtMaxScale => transform.localScale == maxScale * Vector3.one;

    private bool IsAtMinScale => transform.localScale == minScale * Vector3.one;

    private bool isShrinking, isGrowing;

    /// <summary>
    /// The smallest scale the object should be, as defined by it's starting scale set in the editor.
    /// </summary>
    private float minScale;

    private void PlayNewAudioClip(AudioClip clip)
    {
        audioSource.Stop();
        if (clip == auraAudioClip)
            audioSource.loop = true;
        else
            audioSource.loop = false;

        audioSource.clip = clip;

        audioSource.Play();
    }

    private void StopAudioClips(AudioClip clip)
    {
        audioSource.Stop();

        audioSource.clip = clip;

        audioSource.Stop();

    }

    //think about abstracting the trigger volumes to check one method on enter/exit
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !IsAtMaxScale)
        {
            // Grow!
            isShrinking = false;
            isGrowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
           if (other.gameObject.CompareTag("Player") && !IsAtMinScale)
           {
            // Shrink!
                isGrowing = false;
                isShrinking = true;
           }
    }

    /// <summary>
    /// Checking the isGrowing variables to then scale the gameobject's transform to the maxScale over the growthSpeed interpolator.
    /// isCloseEnough are temp variables to see if the object's scale is within the doneGrowingThreshhold, then set the scale to max.
    /// This is to avoid the object slowing down as it reaches the maxScale, or the player thinking it is as large as it can be only
    /// to have it scale downwards against expectations. If !isGrowing, scale the gameobject's transform to the minScale over the 
    /// growthSpeed interpolator.
    /// </summary>
    private void FixedUpdate()
    {
        if (!IsAtMaxScale)
        {
            if (isGrowing)
            {
                PlayNewAudioClip(auraAudioClip);
                transform.localScale =
                    Vector3.MoveTowards(transform.localScale, Vector3.one * maxScale, growthSpeed * Time.deltaTime);

                float distanceFromMax = Mathf.Abs(maxScale - transform.localScale.x);
                bool isCloseEnough = distanceFromMax <= doneGrowingThreshold;

                if (isCloseEnough)
                {
                    StopAudioClips(auraAudioClip);
                    transform.localScale = maxScale * Vector3.one;
                    isGrowing = false;
                    growthCompletedParticles.Play();
                    PlayNewAudioClip(completedGrowingAudioClip);
                }
                StopAudioClips(auraAudioClip);
            }

            else if (!isGrowing)
            {
                transform.localScale = 
                    Vector3.MoveTowards(transform.localScale, Vector3.one * minScale, shrinkSpeed * Time.deltaTime);
                float distanceFromMax = Mathf.Abs(maxScale - transform.localScale.x);
                bool isCloseEnough = distanceFromMax <= doneGrowingThreshold;

                if (isCloseEnough)
                {
                    transform.localScale = minScale * Vector3.one;
                    isShrinking = false;
                }
            }
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minScale = transform.localScale.magnitude;
    }
}
