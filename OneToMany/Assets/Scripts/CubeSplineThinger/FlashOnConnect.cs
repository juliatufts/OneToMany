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

	public void Flash(AnimationCurve flashCurve, Color color, float time){
		StopCoroutine("Flash_Coroutine");
		StartCoroutine(Flash_Coroutine(flashCurve, color, time));
	}

	IEnumerator Flash_Coroutine(AnimationCurve flashCurve, Color color, float time){
		var renderer = GetComponent<Renderer>();
		float startTime = Time.time;
		while(Time.time-startTime < time){
			float u = (Time.time-startTime)/time;
			renderer.material.SetColor("_EmissionColor",flashCurve.Evaluate(u) * color);
			yield return null;
		}
		renderer.material.SetColor("_EmissionColor",Color.black);
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer != LayerMask.NameToLayer(cubeLayer)){
			Flash(flashCurve, color, flashTime);
		}
	}
}
