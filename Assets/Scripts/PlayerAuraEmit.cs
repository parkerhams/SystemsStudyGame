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
    [Tooltip("GameObject that represents the player's Aura area.")]
    [SerializeField]
    private GameObject auraSphere;

    [Tooltip("how quickly aura colors will change once player has entered/exited trigger area")]
    [SerializeField]
    private float auraColorLerpSpeed = 1f;

    [Tooltip("The auraMaterial color when player is not inside growth trigger")]
    [SerializeField]
    Color notEmittingAuraColor;

    [Tooltip("The auraMaterial's color when player is inside growth trigger")]
    [SerializeField]
    Color emittingAuraColor;

    [SerializeField]
    private ParticleSystem auraParticles;

    private Material auraMaterial;

    //startAuraChangeTime is the interpolator for aura color transitions once player enters/exits")]
    private float startAuraChangeTime;

    // Start is called before the first frame update
    void Start()
    {
        startAuraChangeTime = Time.time;
        auraMaterial = GetComponent<Renderer>().material; 
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

    void ActivateAuraColor()
    {
        float t = (Time.time - startAuraChangeTime) * auraColorLerpSpeed;
        auraMaterial.color = Color.Lerp(notEmittingAuraColor, emittingAuraColor, t);
    }

    void DeactivateAuraColor()
    {
        float t = (Time.time - startAuraChangeTime) * auraColorLerpSpeed;
        //actual aura won't just be a plain material color in final version - 
        //mat color change mostly just to test if it works/does not work/debug
        auraMaterial.color = Color.Lerp(emittingAuraColor, notEmittingAuraColor, t);
    }
}
