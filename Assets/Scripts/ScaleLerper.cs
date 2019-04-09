using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The scale lerper class is meant to scale a game object, in this case trees, upwards to a maxScale
/// while the player's game object is in the trigger area of the tree. It lerps between these scales.
/// The trees are referred to as a scalableObject, which grows to the maxScale variable.
/// The trees will grow at a rate of the speed variable over a duration of the duration variable. 
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
        yield return ScaleLerp(minScale, maxScale, duration);
        //lerp scale down for scaling down if not yet at maxScale
        yield return ScaleLerp(maxScale, minScale, duration);

    }

    /// <summary>
    /// ---SCALELERP SUMMARY---
    /// In the Detect coroutine, we return RepeatLerp once the player enters the trigger area.
    /// When Detect is called, we set the minScale to whatever the growable GameObject's local scale is
    /// in the world space. We then call the couroutine RepeatLerp, which takes in two vector 3's and a float variable.
    /// We pass in minScale and maxScale and the duration, all serialized variables at the top, into our ScaleLerp coroutine.
    /// The ScaleLerp uses two temporary variables - scaleValue, which is 0 and is assigned as our starting point, and 
    /// increaseRate, which is used to see when we tell the GameObject to stop scaling. Our increaseRate value tells
    /// the scaleValue value how much to increase by until it gets to 1 over same length of time that it takes our scaleableObject to 
    /// reach maxScale. In short, while our scaleValue is greater than 0 but less than 1, 
    /// then we tell the object to scale to our a, b, and time values - 
    /// which are actually our minScale, maxScale, and durtion values. The increaseRate value is how we increment
    /// scaleValue to 1 - and when it reaches 1, then we stop scaling. We scale the GameObject in the while loop to by setting the
    /// scaleableObject's localScale to a Vector 3 lerp and passing in the a(minScale), b(maxScale), and time (duration) values.
    /// </summary>
    public IEnumerator ScaleLerp(Vector3 a, Vector3 b, float time)
    {
        /// <summary>
        /// ----scaleValue and increaseRate summary----
        /// scaleValue is a temporary value used to say that when the scaleableObject is at its minScale, it is at 0, 
        /// and while the scaleableObject grows to its maxScale over the duration variable value, then we scale upwards 
        /// as long as scaleValue is less then 1 in a while loop. We increase scaleValue to 1 by adding Time.deltaTime multipled by
        /// the increaseRate value. increaseRate tells scaleValue to increase to 1 at the same rate that our scaleableObject 
        /// increases from minScale to maxScale - it accounts for the speed at which we've told the scaleable object to grow as well
        /// as the duration of growth through our temporary time variable, which takes in our serialized duration variable 
        /// when we call RepeatLerp() in our Detect() coroutine. While scaleRate is less than 1, scale the gameobject. 
        /// When scaleValue reaches 1 by taking into account the speed and time values, 
        /// then stop growing because that means we've reached our maxScale.
        ///</summary>
        float scaleValue = 0.0f;
        float increaseRate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code tutorial! Not my while loop
        while (scaleValue < 1f)
        {
            scaleValue += Time.deltaTime * increaseRate;

            //changing the world scale of the object to whatever it is on those three conditions
            scalableObject.transform.localScale = Vector3.Lerp(a, b, scaleValue);

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
