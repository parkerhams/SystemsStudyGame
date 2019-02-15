using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerper : MonoBehaviour
{
    Vector3 minScale;

    [SerializeField]
    Vector3 maxScale;
    [SerializeField]
    bool repeatable;

    public float speed = 2f;
    public float duration = 5f;



    // Start is called before the first frame update
    IEnumerator Start()
    {
        minScale = transform.localScale;
        while(repeatable)
        {
            //lerp scale of model UP
            yield return RepeatLerp(minScale, maxScale, duration);
            //lerp scale down for fluctuation?
            yield return RepeatLerp(maxScale, minScale, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;

        //this while loop was made while following along with Resistance Code
        while(i < 1f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    
}
