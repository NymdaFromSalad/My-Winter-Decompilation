using System;
using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	public Transform target;

	public double distance = 50.0;

	private float scrollSpeed = 3f;

	private float panSpeed = 0.5f;

	private double xSpeed = 250.0;

	private double ySpeed = 120.0;

	private double yMinLimit = -89.0;

	private double yMaxLimit = 89.0;

	private double minZoom = 1.0;

	private double maxZoom = 100.0;

	private double x;

	private double y;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		float[] layerCullDistances = new float[32];
		base.gameObject.GetComponent<Camera>().layerCullDistances = layerCullDistances;
	}

	private void FixedUpdate()
	{
		Vector3 vector = target.position - base.transform.position;
		vector.y = 0f;
		vector.Normalize();
		Vector3 vector2 = new Vector3(vector.x * Mathf.Cos((float)Math.PI / 2f) - vector.z * Mathf.Sin((float)Math.PI / 2f), 0f, vector.x * Mathf.Sin((float)Math.PI / 2f) - vector.z * Mathf.Cos((float)Math.PI / 2f));
		int num = (int)Mathf.Max(1f, panSpeed * Mathf.Max(1f, (float)(distance / 10.0)));
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			target.position += vector * num;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			target.position += vector2 * num;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			target.position -= vector * num;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			target.position -= vector2 * num;
		}
	}

	private void LateUpdate()
	{
		if ((bool)target && Input.GetMouseButton(1))
		{
			x += (double)Input.GetAxis("Mouse X") * xSpeed * 0.02;
			y -= (double)Input.GetAxis("Mouse Y") * ySpeed * 0.02;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
		}
		distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
		if (distance < minZoom)
		{
			distance = minZoom;
		}
		else if (distance > maxZoom)
		{
			distance = maxZoom;
		}
		Quaternion quaternion = Quaternion.Euler((float)y, (float)x, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, (float)(0.0 - distance)) + target.position;
		base.transform.rotation = quaternion;
		base.transform.position = position;
	}

	private static double ClampAngle(double angle, double min, double max)
	{
		if (angle < -360.0)
		{
			angle += 360.0;
		}
		if (angle > 360.0)
		{
			angle -= 360.0;
		}
		return Mathf.Clamp((float)angle, (float)min, (float)max);
	}

	public void FocusCameraOnTransform(Transform transform, float panDuration)
	{
		StartCoroutine(CameraLerpCoroutine(transform.position, Time.timeSinceLevelLoad, panDuration));
	}

	private IEnumerator CameraLerpCoroutine(Vector3 endPosition, float startTime, float panDuration)
	{
		Vector3 startPosition = new Vector3(target.position.x, target.position.y, target.position.z);
		float percentComplete = 0f;
		while (true)
		{
			float num;
			percentComplete = (num = (Time.timeSinceLevelLoad - startTime) / panDuration);
			if (!(num < 1f))
			{
				break;
			}
			target.position = Vector3.Slerp(startPosition, endPosition, percentComplete);
			yield return null;
		}
		target.position = endPosition;
	}
}
