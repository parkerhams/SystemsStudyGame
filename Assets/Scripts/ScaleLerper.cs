using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    ParticleSystem doneGrowingParticles;

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
        //rate of growth
        float i = 0.0f;
        float rate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code! Not my while loop
        while (i < 1f)
        {
            //this is mine though hehe
            i += Time.deltaTime * rate;
            //changing the world scale of the object to whatever it is on those three conditions
            //transform.localScale = Vector3.Lerp(a, b, i);

            scalableObject.transform.localScale = Vector3.Lerp(a, b, i);

            //set it to not repeatable once the cycle is done         
            yield return null;
        }

        doneGrowingParticles.Play();
        yield return new WaitForSeconds(3f);
        doneGrowingParticles.Stop();
        auraAudio.Stop();
    }

}
