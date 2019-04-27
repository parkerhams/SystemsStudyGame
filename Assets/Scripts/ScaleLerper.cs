using System.Collections;
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
    //[Range(0,1)]
    [SerializeField]
    private float growthSpeed = 0.4f;

    [Tooltip("how quickly it shrinks")]
    //[Range(0, 1)]
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

    private const string shrinkCoroutine = nameof(Grow);
    private const string growCoroutine = nameof(Shrink);


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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has ENTERED!");
        if (other.gameObject.CompareTag("Player") && !IsAtMaxScale)
        {
            // Grow!
            //StopAllCoroutines();
            isShrinking = false;
            isGrowing = true;
            //if (!isGrowing)
            //    StartCoroutine(growCoroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has EXITED!");
        if (!IsAtMaxScale)
        {
            if (other.gameObject.CompareTag("Player") && !IsAtMinScale)
            {
                // Shrink!
                //StopAllCoroutines();
                //isGrowing = false;
                isShrinking = true;
                //if (!isShrinking)
                //    StartCoroutine(shrinkCoroutine);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsAtMaxScale)
        {
            if (isGrowing)
            {
                transform.localScale =
                    Vector3.MoveTowards(transform.localScale, Vector3.one * maxScale, growthSpeed * Time.deltaTime);

                float distanceFromMax = Mathf.Abs(maxScale - transform.localScale.x);
                bool isCloseEnough = distanceFromMax <= doneGrowingThreshold;

                if (isCloseEnough)
                {
                    transform.localScale = maxScale * Vector3.one;
                    isGrowing = false;
                    growthCompletedParticles.Play();
                    PlayNewAudioClip(completedGrowingAudioClip);
                }
            }

        }

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minScale = transform.localScale.magnitude;
    }

    private IEnumerator Grow()
    {
        isGrowing = true;
        PlayNewAudioClip(auraAudioClip);
        while (!IsAtMaxScale)
        {
             scalableObject.transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * maxScale, growthSpeed * Time.deltaTime);
            //scalableObject.transform.localScale = Vector3.Lerp(transform.localScale, maxScale * Vector3.one, growthSpeed * Time.deltaTime);
            float distanceFromMax = Mathf.Abs(maxScale - transform.localScale.x);
            bool isCloseEnough = distanceFromMax <= doneGrowingThreshold;

            if (isCloseEnough)
            {
                transform.localScale = maxScale * Vector3.one;               
            }
            yield return null;
        }

        isGrowing = false;

        growthCompletedParticles.Play();
        PlayNewAudioClip(completedGrowingAudioClip);
        yield return new WaitForSeconds(particleDuration);
        growthCompletedParticles.Stop();
    }

    private IEnumerator Shrink()
    {
        isShrinking = true;
        StopAudioClips(auraAudioClip);
        while(!IsAtMinScale)
        {
            scalableObject.transform.localScale = Vector3.Lerp(transform.localScale, minScale * Vector3.one, shrinkSpeed * Time.deltaTime);

            float distanceFromMin = Mathf.Abs(minScale - transform.localScale.magnitude);
            bool isCloseEnough = distanceFromMin <= doneGrowingThreshold;

            if (isCloseEnough)
            {
                transform.localScale = minScale * Vector3.one;
            }
            yield return null;
        }

        isShrinking = false;
    }
}
