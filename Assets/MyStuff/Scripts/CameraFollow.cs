using UnityEngine;
using System.Collections;
using System;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float distanceCameraFromGround = 15;
	public float RotationSpeed = 1;
	public float MoveSpeed = 1;
	public float MaxDistanceFromTarget = 20;
	public float MinDistanceFromTarget = 19;
	public bool InvertCamRotation;

	private float camHeight = 100f;

	void Start()
	{
		if (InvertCamRotation)
		{
			RotationSpeed = -RotationSpeed;
		}
	}

	void FixedUpdate()
	{
		if (target)
		{
			SetCamHeight();

			Vector3 newPos = transform.position;
			var newTarget = new Vector3(target.position.x, camHeight, target.position.z);
			var distanceFromTarget = Vector3.Distance(transform.position, newTarget);
			if (distanceFromTarget > MaxDistanceFromTarget)
			{
				newPos = Vector3.MoveTowards(transform.position, newTarget, Time.fixedDeltaTime * (MoveSpeed * (float)Math.Truncate(distanceFromTarget - MaxDistanceFromTarget)));
			}
			else if(distanceFromTarget < MinDistanceFromTarget)
			{
				var newDirection = transform.position - newTarget;
				newDirection.Normalize();				
				newPos = transform.position + (newDirection * (Time.fixedDeltaTime * MoveSpeed)); Vector3.MoveTowards(transform.position, newTarget, Time.fixedDeltaTime * (MoveSpeed * (float)Math.Truncate(MinDistanceFromTarget - distanceFromTarget)));
			}
			transform.LookAt(target, Vector3.up);
			transform.position = new Vector3(newPos.x, camHeight, newPos.z);			

			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetAxis("Mouse ScrollWheel") < 0)
				{//rotate camera
					transform.RotateAround(target.position, Vector3.up, -RotationSpeed);
				}
				if (Input.GetAxis("Mouse ScrollWheel") > 0)
				{//rotate camera
					transform.RotateAround(target.position, Vector3.up, RotationSpeed);
				}
			}
			else
			{
				if (Input.GetAxis("Mouse ScrollWheel") < 0)
				{//zoom camera Out
					if (MaxDistanceFromTarget < 20)
					{
						distanceCameraFromGround++;
						MaxDistanceFromTarget++;
						MinDistanceFromTarget++;
					}
				}
				if (Input.GetAxis("Mouse ScrollWheel") > 0)
				{//zoom camera in
					if (distanceCameraFromGround > 5)
					{
						distanceCameraFromGround--;
						MaxDistanceFromTarget--;
						MinDistanceFromTarget--;
					}
				}
			}
		}

	}

	private void SetCamHeight()
	{
		Ray r = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, 1000, LayerMask.GetMask("Ground")))
		{
			camHeight = hit.point.y + distanceCameraFromGround;
		}
	}
	private float GetCamAngle()
	{
		float angle = 0;

		return angle;
	}
}
