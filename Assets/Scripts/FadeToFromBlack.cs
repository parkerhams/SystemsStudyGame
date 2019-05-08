using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToFromBlack : MonoBehaviour
{
    [SerializeField]
    private Image img;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private float fadeTime = 3f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    //public void FadeTo(string scene)
    //{
    //    StartCoroutine(FadeOut(scene));
    //}

    IEnumerator FadeIn()
    {
        while (fadeTime > 0f)
        {
            fadeTime -= Time.deltaTime;
            float a = curve.Evaluate(fadeTime);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }
}
