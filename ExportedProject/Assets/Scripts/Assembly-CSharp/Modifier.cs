using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
	public float minVelocity = 35f;

	public float overallStrength = 0.5f;

	public float COGHelperStrength = 1f;

	public float torqueHelperStrength = 1f;

	public float gripHelperStrength = 1f;

	protected CarDynamics carDynamics;

	protected Axles axles;

	protected float COGYShift;

	protected float torque;

	protected float gripSlip;

	protected float gripVelo;

	protected Rigidbody body;

	protected float lateralSlip;

	protected float absLateralSlip;

	protected float veloKmh;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		carDynamics = GetComponent<CarDynamics>();
		axles = GetComponent<Axles>();
		if (overallStrength < 0f)
		{
			overallStrength = 0f;
		}
		if (COGHelperStrength < 0f)
		{
			COGHelperStrength = 0f;
		}
		if (torqueHelperStrength < 0f)
		{
			torqueHelperStrength = 0f;
		}
		if (gripHelperStrength < 0f)
		{
			gripHelperStrength = 0f;
		}
	}

	private void OnDisable()
	{
		ResetParameters();
	}

	protected abstract void COGHelper(out float COGYShift, float absLateralSlip, float overallStrength);

	protected abstract void TorqueHelper(out float torque, float absLateralSlip, float lateralSlip, float overallStrength);

	protected abstract void GripHelper(out float gripSlip, out float gripVelo, float absLateralSlip, float overallStrength);

	protected abstract void ResetParameters();

	private void FixedUpdate()
	{
		overallStrength = Mathf.Clamp01(overallStrength);
		veloKmh = Mathf.Abs(carDynamics.velo * 3.6f);
		if (veloKmh > minVelocity)
		{
			lateralSlip = MaxLateralSlip();
			absLateralSlip = Mathf.Abs(lateralSlip);
			COGHelper(out COGYShift, absLateralSlip, overallStrength);
			TorqueHelper(out torque, absLateralSlip, lateralSlip, overallStrength);
			GripHelper(out gripSlip, out gripVelo, absLateralSlip, overallStrength);
		}
		else
		{
			ResetParameters();
		}
	}

	protected void SetCOGYPosition(float COGYShift)
	{
		if (carDynamics.centerOfMass != null)
		{
			carDynamics.centerOfMass.localPosition = new Vector3(carDynamics.xlocalPosition, carDynamics.ylocalPosition - COGYShift, carDynamics.zlocalPosition);
			body.centerOfMass = carDynamics.centerOfMass.localPosition;
		}
	}

	private float MaxLateralSlip()
	{
		float num = 0f;
		float num2 = 0f;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			num2 = Mathf.Abs(wheel.lateralSlip);
			if (num < num2)
			{
				num = wheel.lateralSlip;
			}
		}
		return num;
	}
}
