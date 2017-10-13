using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(Rigidbody))]
public class FlashOnConnect : MonoBehaviour {

	public string cubeLayer;
	public AnimationCurve flashCurve;
	public float flashTime;
	[ColorUsageAttribute(false,true,0,2,0.125f,3)]
	public Color[] colors;
    [HideInInspector]
    public int cubeIndex;
    [HideInInspector]
    public CubeSpline spline;
    public float flashSpeed;
    public string lotusLayer;
    public AK.Wwise.Event onGrab;
    public AK.Wwise.Event onRelease;
    public AK.Wwise.Event onCollide;
    public AK.Wwise.Event onEnterLotusHole;
    public GameObject particleEffect;


    void Start()
    {
        VRTK_InteractableObject vrtk = GetComponent<VRTK_InteractableObject>();
        vrtk.InteractableObjectGrabbed += Grabbed;
        vrtk.InteractableObjectUngrabbed += Ungrabbed;
    }

    void OnDestroy()
    {
        VRTK_InteractableObject vrtk = GetComponent<VRTK_InteractableObject>();
        vrtk.InteractableObjectGrabbed -= Grabbed;
        vrtk.InteractableObjectUngrabbed -= Ungrabbed;
    }

    void Grabbed(object sender, InteractableObjectEventArgs e)
    {
        onGrab.Post(gameObject);
    }

    void Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        onRelease.Post(gameObject);
    }

    public void Flash(AnimationCurve flashCurve, Color[] colors, float time,int count = 1){
		StopCoroutine("Flash_Coroutine");
		StartCoroutine(Flash_Coroutine(flashCurve, colors, time,count));
	}

	IEnumerator Flash_Coroutine(AnimationCurve flashCurve, Color[] colors, float time, int count){
		var renderer = GetComponent<Renderer>();
		float startTime = Time.time;
        //int i = 0;
		while(Time.time-startTime < time){
			float u = (Time.time-startTime)/time;
            int i = Mathf.FloorToInt((Time.time - startTime) / flashSpeed);
            int colorIndex = i < count ? i : (count-1);
            Color color = colors[colorIndex % colors.Length];
			renderer.material.SetColor("_EmissionColor",flashCurve.Evaluate(u) * color);
            //i++;
			yield return null;
		}
		renderer.material.SetColor("_EmissionColor",Color.black);
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer != LayerMask.NameToLayer(cubeLayer) ||
           collision.relativeVelocity.magnitude > 2.0f){
            spline.IncreaseFlashCount(cubeIndex);
			Flash(flashCurve, colors, flashTime,spline.GetFlashCount(cubeIndex));
            if (collision.rigidbody 
                && onCollide != null 
                && collision.relativeVelocity.magnitude > 2.0f
                && GetComponent<Rigidbody>().velocity.sqrMagnitude > collision.rigidbody.GetComponent<Rigidbody>().velocity.sqrMagnitude) { // if we're the faster of the cubes
                onCollide.Post(gameObject);
            }
            if (!collision.rigidbody
                && onCollide != null
                && collision.relativeVelocity.magnitude > 2.0f)
            {
                onCollide.Post(gameObject);
            }
        }
	}
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(lotusLayer))
        {
            GetComponent<FollowCurveSteering>().currentU = 1; // Force death of Cube
            if(onEnterLotusHole != null)
            {
                onEnterLotusHole.Post(gameObject);
                var go = Instantiate(particleEffect, collider.transform.position, Quaternion.LookRotation(-collider.transform.right));
                
            }
        }
    }
}
