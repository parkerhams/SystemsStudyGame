using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Scales objects up to a max scale if player enters trigger.
/// If player leaves trigger before reaching max scale, the object begins scaling back down to it's minimum / starting scale.
/// while the player's game object is in the trigger area of the tree.
/// Audio will play while the game object scales upwards, and it will play particle effects to show it's reached maxScale.
/// </summary>
public class ScaleLerper : MonoBehaviour
{
    

    [Tooltip("biggest size it can get")] 
    [SerializeField]
    private Vector3 maxScale;

    //[Tooltip("scalableObject is a reference to the location where the foliage should spawn")]
    //[SerializeField]
    //private GameObject scalableObject;

    [Tooltip("how quickly it grows")]
    [Range(0,1)]
    [SerializeField]
    private float growthSpeed = 0.4f;

    [Tooltip("how quickly it shrinks")]
    [Range(0, 1)]
    [SerializeField]
    private float shrinkSpeed = 0.2f;


    //[Tooltip("how long it takes to reach fully grown")]
    //[SerializeField]
    //private float duration = 5f;

    [SerializeField]
    private AudioSource auraAudio;

    [SerializeField]
    private AudioSource completedGrowingAudio;

    [SerializeField]
    private ParticleSystem growthCompletedParticles;

    private float particleDuration = 2f;

    private bool IsAtMaxScale => transform.localScale == maxScale;

    /// <summary>
    /// The smallest scale the object should be, as defined by it's starting scale set in the editor.
    /// </summary>
    private Vector3 minScale;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !IsAtMaxScale)
        {
            // Grow!
            StartCoroutine(Grow());
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        auraAudio.Stop();
    }

    private void Start()
    {
        minScale = transform.localScale;
    }

    private IEnumerator Grow()
    {
        auraAudio.Play();
        while (!IsAtMaxScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, maxScale, growthSpeed * Time.deltaTime);
            yield return null;
        }
        auraAudio.Stop();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Detect()
    {
        //lerp scale of model UP
        yield return null; 
        //lerp scale down for scaling down if not yet at maxScale
        //yield return ScaleLerp(maxScale, minScale, duration);

    }

    /// <summary>
    /// <para>In the Detect coroutine, we return RepeatLerp once the player enters the trigger area.
    /// When Detect is called, we set the minScale to whatever the growable GameObject's local scale is
    /// in the world space. We then call the couroutine RepeatLerp, which takes in two vector 3's and a float variable.
    /// We pass in minScale and maxScale and the duration, all serialized variables at the top, into our ScaleLerp coroutine.
    /// 
    /// <para>The ScaleLerp uses two temporary variables - scaleValue, which is 0 and is assigned as our starting point, and 
    /// increaseRate, which is used to see when we tell the GameObject to stop scaling. Our increaseRate value tells
    /// the scaleValue value how much to increase by until it gets to 1 over same length of time that it takes our scaleableObject to 
    /// reach maxScale. In short, while our scaleValue is greater than 0 but less than 1, 
    /// then we tell the object to scale to our a, b, and time values - 
    /// which are actually our minScale, maxScale, and durtion values. The increaseRate value is how we increment
    /// scaleValue to 1 - and when it reaches 1, then we stop scaling. We scale the GameObject in the while loop to by setting the
    /// scaleableObject's localScale to a Vector 3 lerp and passing in the a(minScale), b(maxScale), and time (duration) values.
    /// </summary>
    private IEnumerator ScaleLerp(Vector3 a, Vector3 b, float time)
    {
        //scaleValue is a temp value used to say that minScale is seen as a 0 value, and maxScale is a value of 1 
        //we will scale the scaleableObject upwards while we increase scaleRate from 0 to 1
        float scaleValue = 0.0f;
        //increaseRate tells scaleValue to increase to 1 at the same rate that our scaleableObject 
        //increases from minScale to maxScale. So, scaleRate will scale by a value of increaseRate to 
        //match the duration and speed the scaleableObject is using to get to maxScale. This is used
        //so we know we have another condition to tell it to stop scaling.
        //float increaseRate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code tutorial! Not my while loop
        while (scaleValue < 1f)
        {
            //scaleValue += Time.deltaTime * increaseRate;

            //changing the world scale of the object to whatever it is on those three conditions
            //scalableObject.transform.localScale = Vector3.Lerp(a, b, scaleValue);

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
