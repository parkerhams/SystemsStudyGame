using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The aura trigger surrounding the player that calls for plants to grow.
//This script will also lerp aura colors to visualize the aura emitting
public class PlayerAuraEmit : MonoBehaviour
{
    [Tooltip("THe GameObject that represents the player's Aura area.")]
    [SerializeField]
    private GameObject auraSphere;

    [Tooltip("The float value of how quickly colors will lerp once aura is activated")]
    [SerializeField]
    float lerpSpeed = 1f;

    [Tooltip("The Color of the deactivated aura")]
    [SerializeField]
    Color deactivatedAuraColor;

    [Tooltip("The Color to show aura has been activated")]
    [SerializeField]
    Color activatedAuraColor;

    [Tooltip("Lerp time value")]
    [SerializeField]
    float startLerpTime;

    [SerializeField]
    ParticleSystem auraParticles;

    // Start is called before the first frame update
    void Start()
    {
        startLerpTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Flora"))
        {
            //if colliding with the Flora area, change mat opacity/color to show activated
            ActivateAuraColorLerp();

            auraParticles.Play();

            Debug.Log("player is hitting flora!!!");
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flora"))
        {
            //change mat opacity/color to show deactivated
            DeactivateAuraColorLerp();
            auraParticles.Stop();
            Debug.Log("player is no longer touching flora area");
        }
    }

    void ActivateAuraColorLerp()
    {
        float t = (Time.time - startLerpTime) * lerpSpeed;
        GetComponent<Renderer>().material.color = Color.Lerp(deactivatedAuraColor, activatedAuraColor, t);
    }

    void DeactivateAuraColorLerp()
    {
        float t = (Time.time - startLerpTime) * lerpSpeed;
        GetComponent<Renderer>().material.color = Color.Lerp(activatedAuraColor, deactivatedAuraColor, t);
    }
}
