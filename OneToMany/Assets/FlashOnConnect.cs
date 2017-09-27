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

	IEnumerator Flash(AnimationCurve flashCurve, Color color){
		var renderer = GetComponent<Renderer>();
		float startTime = Time.time;
		while(Time.time-startTime < flashTime){
			float u = (Time.time-startTime)/flashTime;
			renderer.material.SetColor("_EmissionColor",flashCurve.Evaluate(u) * color);
			yield return null;
		}
		renderer.material.SetColor("_EmissionColor",Color.black);
	}

	void OnCollisionEnter(Collision collision) {
//		Debug.Log("Collision Detected");
		if(collision.gameObject.layer != LayerMask.NameToLayer(cubeLayer)){
//			Debug.Log("Starting Flash");
			StartCoroutine(Flash(flashCurve, color));
		}
	}
}
