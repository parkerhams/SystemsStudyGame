using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The scale leraper class is meant to scale a game object, in this case trees, upwards to a maxScale
/// while the player's game object is in the trigger area of the tree.
/// The trees are referred to as a GameObject scalableObject, which grows to the maxScale variable.
/// The trees will grow ar a rate of the speed variable over a duration of the duration variable. 
/// Audio will play while the game object scales upwards, and it will play particle effects to show it's reached maxScale.
/// </summary>
public class ScaleLerper : MonoBehaviour
{
    [Tooltip("base size for any trees or foliage that will scale")]
    Vector3 minScale;

    [Tooltip("max scale meant to be controlled in editor - biggest size it can get")] 
    [SerializeField]
    Vector3 maxScale;

    [Tooltip("scalableObject is a reference to the location where the foliage should spawn")]
    [SerializeField]
    GameObject scalableObject;

    [Tooltip("how quickly it grows")]
    [SerializeField]
    private float speed = 2f;

    [Tooltip("how long it takes to reach fully grown")]
    [SerializeField]
    private float duration = 5f;

    [SerializeField]
    AudioSource auraAudio;

    [SerializeField]
    AudioSource completedGrowingAudio;

    [SerializeField]
    ParticleSystem growthCompletedParticles;

    [Tooltip("How long the doneGrowingParticles should play for")]
    private float particleDuration = 2f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("Detect");
            auraAudio.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        auraAudio.Stop();
    }

    public IEnumerator Detect()
    {
        //smallest scale is whatever it is set to in world space
        minScale = transform.localScale;
        //if it can grow, it will call other coroutine to begin scale lerp
        //lerp scale of model UP
        yield return RepeatLerp(minScale, maxScale, duration);
        //lerp scale down for fluctuation
        yield return RepeatLerp(maxScale, minScale, duration);

    }

    //takes in two vector 3s - start scale and max scale - and how quickly they lerp as time
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        /// <summary>
        /// This Enumerator is using scaleLerp to tell the gameObject where to start - and when we reach
        /// the iterateLerp variable, in this case 1 divided by the passed in time variable multipled by speed,
        /// we are telling the game objects to stop scaling. While we scale upwards, we scale while we are 
        /// between the values of scaleRate and iterateLerpRate. We start at a value of 0 here, and once the value reaches 
        /// the iterateLerpRate value, we stop scaling upwards.
        ///</summary>
        float scaleRate = 0.0f;
        float iterateLerpRate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code tutorial! Not my while loop
        while (scaleRate < 1f)
        {
            scaleRate += Time.deltaTime * iterateLerpRate;

            //changing the world scale of the object to whatever it is on those three conditions
            scalableObject.transform.localScale = Vector3.Lerp(a, b, scaleRate);

            //set it to not repeatable once the cycle is done         
            yield return null;
        }

        growthCompletedParticles.Play();
        yield return new WaitForSeconds(particleDuration);
        completedGrowingAudio.Play();
        growthCompletedParticles.Stop();
        auraAudio.Stop();
    }

}
