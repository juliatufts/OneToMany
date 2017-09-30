using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlashOnConnect : MonoBehaviour {

	public string cubeLayer;
	public AnimationCurve flashCurve;
	public float flashTime;
	[ColorUsageAttribute(false,true,0,2,0.125f,3)]
	public Color color;
    public float flashMagnitude = 3.0f;
	public int flashCount = 1;

    IEnumerator Flash(AnimationCurve flashCurve, Color color){
        GetComponent<Animator>().enabled = false;
        var renderer = GetComponent<Renderer>();
		renderer.material = new Material(renderer.material);
		float startTime = Time.time;
		while(Time.time-startTime < flashTime){
			float u = (Time.time-startTime)/flashTime;
			renderer.material.SetColor("_EmissionColor",flashCurve.Evaluate(u) * color);
			yield return null;
		}
		renderer.material.SetColor("_EmissionColor",Color.black);
		GetComponent<Animator>().enabled = true;
	}

	void OnCollisionEnter(Collision collision) {
//		Debug.Log("Collision Detected");
		if(collision.gameObject.layer != LayerMask.NameToLayer(cubeLayer)
		|| collision.relativeVelocity.magnitude > flashMagnitude){
            Debug.LogError("Starting Flash");
            StopAllCoroutines();
            StartCoroutine(Flash(flashCurve, color));
		}
	}
}
