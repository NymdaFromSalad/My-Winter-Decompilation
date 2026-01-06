using UnityEngine;

public class CarDebug : MonoBehaviour
{
	private Drivetrain drivetrain;

	private Axles axles;

	private ForceFeedback forceFeedback;

	private void Start()
	{
		drivetrain = GetComponent<Drivetrain>();
		axles = GetComponent<Axles>();
		forceFeedback = GetComponent<ForceFeedback>();
	}

	public float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}

	private void OnGUI()
	{
		GUILayout.Label("clutch.GetClutchPosition: " + drivetrain.clutch.GetClutchPosition());
		GUILayout.Label("currentPower: " + Mathf.Round(drivetrain.currentPower));
		GUILayout.Label("engFricTorque: " + drivetrain.frictionTorque + " engineTorque: " + drivetrain.torque);
		GUILayout.Label("clutchDrag: " + drivetrain.clutchDragImpulse / Time.fixedDeltaTime + "  torque - frictionTorque: " + (drivetrain.torque - drivetrain.frictionTorque));
		GUILayout.Label("CanShiftAgain: " + drivetrain.CanShiftAgain + " changingGear: " + drivetrain.changingGear);
		GUILayout.Label("clutch.speedDiff: " + drivetrain.clutch.speedDiff);
		GUILayout.Label("gear: " + drivetrain.gear + " clutchEngageSpeed: " + drivetrain.clutchEngageSpeed);
		GUILayout.Label("maxPowerDriveShaft: " + drivetrain.maxPowerDriveShaft * drivetrain.powerMultiplier + " maxNetPower: " + drivetrain.maxNetPower * drivetrain.powerMultiplier + " maxTorque: " + drivetrain.maxTorque + " maxNetTorque: " + drivetrain.maxNetTorque);
		GUILayout.Label("startTorque: " + drivetrain.startTorque + " throttle:" + drivetrain.throttle);
		GUILayout.Label("% loss power: " + (drivetrain.maxPowerDriveShaft - drivetrain.maxNetPower) / drivetrain.maxPowerDriveShaft * 100f + "%");
		if (forceFeedback != null)
		{
			GUILayout.Label("force feedback: " + forceFeedback.force);
		}
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 280f, 600f, 200f), "hitDown.distance - radius : " + wheel.hitDown.distance);
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 300f, 600f, 200f), "hitDown.normal .x         .z " + wheel.hitDown.normal.x + " " + wheel.hitDown.normal.z);
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 320f, 600f, 200f), "groundNormal.x groundNormal.z " + wheel.groundNormal.x + " " + wheel.groundNormal.z);
			}
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 340f, 600f, 200f), "wheelRoadVeloLatFL: " + wheel.wheelRoadVeloLat);
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 360f, 600f, 200f), "wheelRoadVeloLatFR: " + wheel.wheelRoadVeloLat);
			}
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 380f, 600f, 200f), "absRoadVelo : " + Mathf.Abs(wheel.wheelRoadVelo));
			}
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 400f, 600f, 200f), "idealSlipRatioFL :" + wheel.idealSlipRatio + " idealSlipAngleFL " + wheel.idealSlipAngle);
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 420f, 600f, 200f), "idealSlipRatioFR :" + wheel.idealSlipRatio + " idealSlipAngleFR " + wheel.idealSlipAngle);
			}
			if (wheel.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 440f, 600f, 200f), "idealSlipRatioRL :" + wheel.idealSlipRatio + " idealSlipAngleRL " + wheel.idealSlipAngle);
			}
			if (wheel.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 460f, 600f, 200f), "idealSlipRatioRR :" + wheel.idealSlipRatio + " idealSlipAngleRR " + wheel.idealSlipAngle);
			}
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 480f, 600f, 200f), "slipRatioFL: " + wheel.slipRatio + "    Fx: " + wheel.Fx + " longitudinalSlip:" + RoundTo(wheel.longitudinalSlip, 3) + " normalForce:" + wheel.normalForce);
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 500f, 600f, 200f), "slipRatioFR: " + wheel.slipRatio + "    Fx: " + wheel.Fx + " longitudinalSlip:" + RoundTo(wheel.longitudinalSlip, 3) + " normalForce:" + wheel.normalForce);
			}
			if (wheel.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 520f, 600f, 200f), "slipRatioRL: " + wheel.slipRatio + "    Fx: " + wheel.Fx + " longitudinalSlip:" + RoundTo(wheel.longitudinalSlip, 3) + " normalForce:" + wheel.normalForce);
			}
			if (wheel.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 540f, 600f, 200f), "slipRatioRR: " + wheel.slipRatio + "    Fx: " + wheel.Fx + " longitudinalSlip:" + RoundTo(wheel.longitudinalSlip, 3) + " normalForce:" + wheel.normalForce);
			}
			if (wheel.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 560f, 600f, 200f), "slipAngleFL: " + wheel.slipAngle + "    Fy: " + wheel.Fy + " lateralSlip:" + RoundTo(wheel.lateralSlip, 3) + " rho:" + Mathf.Sqrt(wheel.lateralSlip * wheel.lateralSlip + wheel.longitudinalSlip * wheel.longitudinalSlip));
			}
			if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 580f, 600f, 200f), "slipAngleFR: " + wheel.slipAngle + "    Fy: " + wheel.Fy + " lateralSlip:" + RoundTo(wheel.lateralSlip, 3) + " rho:" + Mathf.Sqrt(wheel.lateralSlip * wheel.lateralSlip + wheel.longitudinalSlip * wheel.longitudinalSlip));
			}
			if (wheel.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 600f, 600f, 200f), "slipAngleRL: " + wheel.slipAngle + "    Fy: " + wheel.Fy + " lateralSlip:" + RoundTo(wheel.lateralSlip, 3) + " rho:" + Mathf.Sqrt(wheel.lateralSlip * wheel.lateralSlip + wheel.longitudinalSlip * wheel.longitudinalSlip));
			}
			if (wheel.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 620f, 600f, 200f), "slipAngleRR: " + wheel.slipAngle + "    Fy: " + wheel.Fy + " lateralSlip:" + RoundTo(wheel.lateralSlip, 3) + " rho:" + Mathf.Sqrt(wheel.lateralSlip * wheel.lateralSlip + wheel.longitudinalSlip * wheel.longitudinalSlip));
			}
		}
	}
}
