using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	Animator animator;
	CameraController cameraController;

	public float walkSpeed = 2f;
	public float runSpeed = 6;

	public float turnSmoothTime = .2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = .1f;
	float speedSmoothVelocity;
	float currentSpeed;

	private void Start()
	{
		animator = GetComponent<Animator>();
		cameraController = GameObject.FindObjectOfType<CameraController>();
	}

	private void Update()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;

		if (inputDir != Vector2.zero)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraController.CameraAngle;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		bool running = Input.GetKey(KeyCode.LeftShift);
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

		transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

		float speedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
		animator.SetFloat("SpeedPercent", speedPercent, speedSmoothTime, Time.deltaTime);
	}
}
