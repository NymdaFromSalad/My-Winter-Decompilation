using UnityEngine;

public class Arcader : Modifier
{
	protected override void COGHelper(out float COGYShift, float absLateralSlip, float strength)
	{
		COGYShift = Mathf.Clamp01(absLateralSlip) * 0.35f * strength * COGHelperStrength;
		SetCOGYPosition(COGYShift);
	}

	protected override void TorqueHelper(out float torque, float absLateralSlip, float lateralSlip, float strength)
	{
		torque = (absLateralSlip - 1f) * Mathf.Sign(lateralSlip) * 10000f * strength * torqueHelperStrength;
		if (absLateralSlip > 1f)
		{
			body.AddRelativeTorque(-Vector3.up * torque);
		}
	}

	protected override void GripHelper(out float gripSlip, out float gripVelo, float absLateralSlip, float strength)
	{
		gripSlip = (gripVelo = 0f);
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			gripSlip = Mathf.Clamp01(absLateralSlip) * strength;
			wheel.gripSlip = gripSlip;
			gripVelo = (veloKmh - minVelocity) * 0.0015f * strength * gripHelperStrength;
			wheel.gripVelo = gripVelo;
		}
	}

	protected override void ResetParameters()
	{
		if (carDynamics != null)
		{
			COGHelper(out COGYShift, 0f, 0f);
		}
		if (axles.allWheels != null)
		{
			GripHelper(out gripSlip, out gripVelo, 0f, 0f);
		}
	}
}
