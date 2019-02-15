using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerper : MonoBehaviour
{
    //base size for any trees or foliage that will scale
    Vector3 minScale;

    //max scale meant to be controlled in editor - biggest size it can get 
    [SerializeField]
    Vector3 maxScale;
    //can it also shrink down? Meant for if player has not completely grown the area
    [SerializeField]
    bool repeatable;
    //foliage to add more details like grass or flowers around these larger scaled objects
    [SerializeField]
    List<GameObject> foliage = new List<GameObject>();
    //scalableObject is a reference to itself for purpose of tracking what's growing
    [SerializeField]
    GameObject scalableObject;

    //how quickly it grows
    public float speed = 2f;
    //how long to process of growing takes place
    public float duration = 5f;

    

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //scalableObject = transform.localPosition;

        //smallest scale is whatever it is set to in world space
        minScale = transform.localScale;
        //if it can grow, it will call other coroutine to begin scale lerp
        while(repeatable)
        {
            //lerp scale of model UP
            yield return RepeatLerp(minScale, maxScale, duration);
            //lerp scale down for fluctuation?
            yield return RepeatLerp(maxScale, minScale, duration);
            
        }
    }

    //takes in two vector 3s - start scale and max scale - and how quickly they lerp as time
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        //rate of growth
        float i = 0.0f;
        float rate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code! Not my while loop
        while(i < 1f)
        {
            //this is mine though hehe
            //yeet
            i += Time.deltaTime * rate;
            //changing the world scale of the object to whatever it is on those three conditions
            transform.localScale = Vector3.Lerp(a, b, i);
            //set it to not repeatable once the cycle is done
            repeatable = false;
            yield return null;
        }
    }

    
}
