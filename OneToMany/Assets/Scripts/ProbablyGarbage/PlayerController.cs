using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 6.0f;
	public float gravity = 20.0f;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update() 
	{
		// Movement
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		controller.Move(moveDirection * Time.deltaTime);

		if (Input.GetKey (KeyCode.C)) {
			controller.Move (Vector3.down * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.V)) {
			controller.Move (Vector3.up * speed * Time.deltaTime);
		}

		// Rotatation
		if (Input.GetKey (KeyCode.Z)) {
			transform.RotateAround (transform.position, Vector3.up, -speed);
		}
		if (Input.GetKey (KeyCode.X)) {
			transform.RotateAround (transform.position, Vector3.up, speed);
		}
	}
}