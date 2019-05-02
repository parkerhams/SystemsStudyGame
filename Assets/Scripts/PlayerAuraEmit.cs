using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The aura trigger surrounding the player that calls for plants to grow.
/// This script will also lerp aura colors to visualize the aura emitting/not emitting 
/// which demonstrates if the player is within tree's trigger area for growth.
/// </summary>
public class PlayerAuraEmit : MonoBehaviour
{
    [Tooltip("The GameObject that represents the player's Aura area.")]
    [SerializeField]
    private GameObject auraSphere;

    [Tooltip("The float value of how quickly aura colors will change once player has entered/exited trigger area")]
    [SerializeField]
    private float auraColorLerpSpeed = 1f;

    [Tooltip("The Color of the aura when it is not inside growth trigger")]
    [SerializeField]
    Color notEmittingAuraColor;

    [Tooltip("The Color to show aura when it IS inside growth trigger")]
    [SerializeField]
    Color emittingAuraColor;

    [Tooltip("Lerp time is the interpolator for aura color transitions once player enters/exits")]
    [SerializeField]
    private float startAuraChangeTime;

    [SerializeField]
    private ParticleSystem auraParticles;

    // Start is called before the first frame update
    void Start()
    {
        startAuraChangeTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Flora"))
        {
            //if colliding with the Flora area, change mat opacity/color to show activated
            ActivateAuraColorLerp();

            auraParticles.Play();
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flora"))
        {
            //change mat opacity/color to show deactivated
            DeactivateAuraColorLerp();
            auraParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void ActivateAuraColorLerp()
    {
        float t = (Time.time - startAuraChangeTime) * auraColorLerpSpeed;
        GetComponent<Renderer>().material.color = Color.Lerp(notEmittingAuraColor, emittingAuraColor, t);
    }

    void DeactivateAuraColorLerp()
    {
        float t = (Time.time - startAuraChangeTime) * auraColorLerpSpeed;
        GetComponent<Renderer>().material.color = Color.Lerp(emittingAuraColor, notEmittingAuraColor, t);
    }
}
