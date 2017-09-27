using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Curve : MonoBehaviour {
	const int numSubDivisions = 250;
	const float derivativeDelta = 0.01f;
	public int numPoints;
	public Vector3AnimationCurve animationCurve = new Vector3AnimationCurve();

	public List<Transform> points = new List<Transform>();

	public Vector3 Get(float u){
		return animationCurve.Evaluate(u);
	}

	public Vector3 CrappyDerivitiveDirection(float u){
		Vector3 a = animationCurve.Evaluate(u-derivativeDelta*0.5f);
		Vector3 b = animationCurve.Evaluate(u+derivativeDelta*0.5f);
		return transform.TransformDirection((b-a).normalized);
	}

	public void RebuildCurve(){
		animationCurve.Clear();
		for(int i = 0; i < points.Count; i++){
			animationCurve.AddKey((float)i/(points.Count-1),points[i].position);
		}
	}

	void OnDrawGizmos(){
//		Gizmos.matrix = transform.localToWorldMatrix;
		Vector3 last =  animationCurve.Evaluate(0);
		for(int i = 0; i < numSubDivisions; i++){
			float u = (float)(i+1) /numSubDivisions;
			Vector3 next = animationCurve.Evaluate(u);
			Gizmos.DrawLine(last, next);
			last = next;
		}
	}

	void Update(){
		RebuildCurve();
    }
}

[System.Serializable]
public class Vector3AnimationCurve{
	public AnimationCurve x = new AnimationCurve();
	public AnimationCurve y = new AnimationCurve();
	public AnimationCurve z = new AnimationCurve();
	
	public Vector3 Evaluate(float u){
		return new Vector3(
			x.Evaluate(u),
			y.Evaluate(u),
			z.Evaluate(u)
			);
	}
	public void AddKey(float u, Vector3 value){
		x.AddKey(u,value.x);
		y.AddKey(u,value.y);
		z.AddKey(u,value.z);
	}
	public void Clear(){
		x = new AnimationCurve();
		y = new AnimationCurve();
		z = new AnimationCurve();
	}
}