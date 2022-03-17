using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Mouse Settings")]
	public bool lockCursor;
	[Range(1, 20)]
	public float mouseSensitivity = 3f;
	[Range(20, 50)]
	public float scrollAmount = 30;
	[Range(0, 1)]
	public float scrollSpeed = .25f;

	[Header("Camera Settings")]
	public Transform target;
	float dstFromTarget = 5;

	[Range(0, 2)]
	public float cameraHeight = 1.5f;

	public Vector2 pitchMinMax = new Vector2(-40, 85);
	public Vector2 dstFromTargetMinMax = new Vector2(2, 10);

	public float rotationSmoothTime = .12f;
	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float yaw;
	float pitch;

	public float CameraAngle { get { return cameraAngle; } set { } }
	float cameraAngle = 0;
	float prevCameraAngle = 0;

	bool isCameraLocked;

	private void Start()
	{
		if (lockCursor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		yaw = pitch = 0f;
		
		StartCoroutine(SmoothScroll());

		if (target == null) target = GameObject.FindObjectOfType<PlayerController>().transform;
	}

	private void Update()
	{
		if (!isCameraLocked) cameraAngle = Camera.main.transform.eulerAngles.y;

		if (Input.GetKeyDown(KeyCode.LeftAlt))
		{
			prevCameraAngle = this.transform.eulerAngles.y;
			cameraAngle = prevCameraAngle;
			isCameraLocked = true;
		}
		else if (Input.GetKeyUp(KeyCode.LeftAlt)) { cameraAngle = this.transform.eulerAngles.y; isCameraLocked = false; }
	}

	private void LateUpdate()
	{
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

		dstFromTarget = Mathf.Clamp(dstFromTarget, dstFromTargetMinMax.x, dstFromTargetMinMax.y);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = currentRotation;

		transform.position = target.position - transform.forward * dstFromTarget + Vector3.up * cameraHeight;
	}

	IEnumerator SmoothScroll()
	{
		float target = dstFromTarget;
		float lerpValue = 0f;

		while (true)
		{
			float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollAmount;

			if (scroll != 0)
			{
				target = dstFromTarget + scroll;
				lerpValue = 0f;
			}

			else
			{
				dstFromTarget = Mathf.Lerp(dstFromTarget, target, lerpValue);
				lerpValue += scrollSpeed * Time.deltaTime;
			}

			yield return null;
		}
	}
}
