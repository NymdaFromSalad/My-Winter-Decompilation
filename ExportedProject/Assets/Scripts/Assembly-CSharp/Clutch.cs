using UnityEngine;

public class Clutch
{
	public float maxTorque;

	public float speedDiff;

	private float clutch_position;

	private float impulseLimit;

	public float GetDragImpulse(float engine_speed, float drive_speed, float engineInertia, float totalRotationalInertia, float ratio, float wheelImpulse, float engineTorqueImpulse)
	{
		float num = totalRotationalInertia / (ratio * ratio);
		impulseLimit = clutch_position * maxTorque * Time.deltaTime;
		speedDiff = engine_speed - drive_speed;
		float num2 = engineInertia * num * speedDiff;
		float num3 = engineInertia * (wheelImpulse / ratio);
		float num4 = num * engineTorqueImpulse;
		float value = (num2 - num3 + num4) / (engineInertia + num);
		return Mathf.Clamp(value, 0f - impulseLimit, impulseLimit);
	}

	public void SetClutchPosition(float value)
	{
		clutch_position = value;
	}

	public float GetClutchPosition()
	{
		return clutch_position;
	}
}
