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
        StartCoroutine(Shrink());
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

        if(IsAtMaxScale)
        {
            growthCompletedParticles.Play();
            auraAudio.Stop();
            yield return new WaitForSeconds(particleDuration);
            completedGrowingAudio.Play();
            growthCompletedParticles.Stop();
        }
    }

    private IEnumerator Shrink()
    {
        auraAudio.Stop();
        while(!IsAtMaxScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, minScale, shrinkSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
