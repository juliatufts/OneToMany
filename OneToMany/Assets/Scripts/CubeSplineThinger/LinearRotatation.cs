using UnityEngine;
using System.Collections;

public class LinearRotatation : MonoBehaviour {

	Vector3 axis;
	public float rate;
	void Start () {
		axis = Random.onUnitSphere;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(axis,rate * Time.deltaTime);
	}
}
