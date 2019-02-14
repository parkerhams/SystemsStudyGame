using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SongFadeIn : MonoBehaviour
{	
	public int fadeTime = 100;
	public float maxVolume = 1f;

	void FixedUpdate ()
	{
		if (GetComponent<AudioSource>().volume <= maxVolume)
		{
			GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume + (Time.deltaTime / (fadeTime));
		}
		//else
		//{
		//	Destroy (this);
		//}
	}
}