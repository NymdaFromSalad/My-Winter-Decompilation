using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
	public List<AxleInfo> axleInfos;

	public float maxMotorTorque;

	public float maxSteeringAngle;

	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount != 0)
		{
			Transform child = collider.transform.GetChild(0);
			Vector3 pos;
			Quaternion quat;
			collider.GetWorldPose(out pos, out quat);
			child.transform.position = pos;
			child.transform.rotation = quat;
		}
	}

	public void FixedUpdate()
	{
		float motorTorque = maxMotorTorque * Input.GetAxis("Vertical");
		float steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
		foreach (AxleInfo axleInfo in axleInfos)
		{
			if (axleInfo.steering)
			{
				axleInfo.leftWheel.steerAngle = steerAngle;
				axleInfo.rightWheel.steerAngle = steerAngle;
			}
			if (axleInfo.motor)
			{
				axleInfo.leftWheel.motorTorque = motorTorque;
				axleInfo.rightWheel.motorTorque = motorTorque;
			}
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}
	}
}
