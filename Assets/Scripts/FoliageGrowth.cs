using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGrowth : MonoBehaviour
{
    [Tooltip("base size for any trees or foliage that will scale")]
    Vector3 minScale;

    [Tooltip("biggest size it can get")]
    [SerializeField]
    Vector3 maxScale;

    [Tooltip("location of spawn areas for more thickness and brush")]
    [SerializeField]
    Transform foliageSpawn;

    [Tooltip("how quickly it grows")]
    [SerializeField]
    private float growthRate = 2f;

    [Tooltip("can it also shrink down - Meant for if player has not completely grown the area")]
    [SerializeField]
    bool canShrink = true;

    [SerializeField]
    bool isGrowingFoliage;

    [SerializeField]
    static float foliageGrowth = 0.0f;

    [Tooltip("foliage to add more details like grass or flowers around these larger scaled objects")]
    [SerializeField]
    List<GameObject> foliage = new List<GameObject>();

    List<GameObject> spawnedFoliage = new List<GameObject>();

    void Update()
    {
        TriggerGrowth();
    }

    public void TriggerGrowth()
    {
        // .. and increase the foliageGrowth interpolater
        foliageGrowth += 0.5f * Time.deltaTime;

        // now check if the interpolator has reached maxScale
        // grow the foliage by calling method
        if (foliageGrowth > maxScale.y)
        {
            isGrowingFoliage = true;
            if (isGrowingFoliage)
            {
                Vector3 temp9 = new Vector3(Mathf.Lerp(minScale.y, maxScale.y, foliageGrowth), 0);
                temp9 = foliageSpawn.transform.position;

                //FoliageSpawnLerp(foliageSpawn, maxScale, duration);
            }
        }

    }

    public IEnumerator FoliageSpawnLerp(Transform location, Vector3 size, float period)
    {
        //rate of growth
        float fgrowth = 0.0f;
        float frate = (1.0f / period) * growthRate;


        for (int i = 0; i < foliage.Count; i++)
        {
            var foliageInstance = Instantiate(foliage[i], location);
            spawnedFoliage.Add(foliageInstance);

            if (spawnedFoliage.Count >= foliage.Count)
            {
                while (fgrowth < 1f)
                {
                    fgrowth += Time.deltaTime * frate;
                    transform.localScale = Vector3.Lerp(location.position, size, period);
                    yield return null;
                }
            }
        }
    }
}
