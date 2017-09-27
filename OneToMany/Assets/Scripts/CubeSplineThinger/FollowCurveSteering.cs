using UnityEngine;
using System.Collections;

public class FollowCurveSteering : MonoBehaviour {
	public float minDist;
	public float maxDist;
	public Curve curveToFollow;
	public float speed;

	public float currentU = 0;
	private Vector3 offset;

	public AnimationCurve maxForceForDistance;
	public float maxSpeed = 1f;
	public float maxForce = 5f;

	// Use this for initialization
	void Start () {
		Vector3 start = curveToFollow.animationCurve.Evaluate(0);
		Vector3 startD = curveToFollow.CrappyDerivitiveDirection(0);
		offset = Quaternion.AngleAxis(Random.Range(0.0f,360.0f), startD) * Vector3.up * Random.Range(minDist, maxDist);
		transform.position = start + offset;
		
	}


	void FixedUpdate(){
		if(currentU <= 1){
			currentU += Time.deltaTime * speed;
			var targetPosition = curveToFollow.Get(currentU) + offset;

			var desiredVelocity = Vector3.ClampMagnitude(targetPosition - transform.position, maxSpeed);
			var desiredForce = Vector3.ClampMagnitude((desiredVelocity - GetComponent<Rigidbody>().velocity) / Time.deltaTime,maxForce);

			GetComponent<Rigidbody>().AddForce(desiredForce);
		}
		if(currentU > 1){
            transform.SendMessageUpwards("CubeRemoved", gameObject);
           	GetComponent<Animator>().Play("cube_out");
            Destroy(this);
        }
	}
}
