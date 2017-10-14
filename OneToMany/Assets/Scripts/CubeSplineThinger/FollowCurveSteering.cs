using UnityEngine;
using System.Collections;

public class FollowCurveSteering : MonoBehaviour {
	public float minDist;
	public float maxDist;
	public Curve curveToFollow;
	public float speed;
    public bool neverDie = false;

	public float currentU = 0;
	private Vector3 offset;

	public AnimationCurve maxForceForDistance;
	public float maxSpeed = 1f;
	public float maxForce = 5f;

	public AnimationCurve flashInCurve;
	public AnimationCurve flashOutCurve;
    bool movingForward = true;

	// Use this for initialization
	void Start () {
		Vector3 start = curveToFollow.Get(currentU);
		Vector3 startD = curveToFollow.CrappyDerivitiveDirection(currentU);
		offset = Quaternion.AngleAxis(Random.Range(0.0f,360.0f), startD) * Vector3.up * Random.Range(minDist, maxDist);
		transform.position = start + offset;
		//GetComponent<FlashOnConnect>().Flash(flashInCurve,new Color[] { Color.white },1.5f);
	}


	void FixedUpdate(){
        if (currentU < 0){
            movingForward = true;
            currentU += Time.deltaTime * speed;
        }
        else if (currentU <= 1){
            currentU = (movingForward) ? (currentU + Time.deltaTime * speed) : (currentU - Time.deltaTime * speed);
            var targetPosition = curveToFollow.Get(currentU) + offset;
			var desiredVelocity = Vector3.ClampMagnitude(targetPosition - transform.position, maxSpeed);
			var desiredForce = Vector3.ClampMagnitude((desiredVelocity - GetComponent<Rigidbody>().velocity) / Time.deltaTime,maxForce);

			GetComponent<Rigidbody>().AddForce(desiredForce);
		}
		if(currentU > 1){
            if (neverDie){
                movingForward = false;
                currentU -= Time.deltaTime * speed;
            }
            else{
                GetComponentInParent<CubeSpline>().CubeRemoved(gameObject, GetComponent<FlashOnConnect>().cubeIndex);
                GetComponent<Animator>().Play("cube_out");
                GetComponent<FlashOnConnect>().Flash(flashInCurve, new Color[] { Color.white }, 2.0f);
                Destroy(this);
            }
        }
	}
}
