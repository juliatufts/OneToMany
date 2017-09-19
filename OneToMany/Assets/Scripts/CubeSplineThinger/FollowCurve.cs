using UnityEngine;
using System.Collections;

public class FollowCurve : MonoBehaviour {
	public float minDist;
	public float maxDist;
	public Curve curveToFollow;
	public float speed;

	public float currentU = 0;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		Vector3 start = curveToFollow.animationCurve.Evaluate(0);
		Vector3 startD = curveToFollow.CrappyDerivitiveDirection(0);
		offset = Quaternion.AngleAxis(Random.Range(0.0f,360.0f), startD) * Vector3.up * Random.Range(minDist, maxDist);
		transform.position = start + offset;
		
	}
	
	// Update is called once per frame
	void Update () {
		currentU += Time.deltaTime * speed;
		transform.position = curveToFollow.Get(currentU) + offset;

		if(currentU > 1){
            transform.SendMessageUpwards("CubeRemoved", gameObject);
            Destroy (gameObject);
		}
	}
}
