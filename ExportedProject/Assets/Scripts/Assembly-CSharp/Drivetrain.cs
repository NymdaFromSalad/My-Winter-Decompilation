using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Axles))]
public class Drivetrain : MonoBehaviour
{
	public enum Transmissions
	{
		RWD = 0,
		FWD = 1,
		AWD = 2,
		XWD = 3
	}

	[HideInInspector]
	public bool engineTorqueFromFile;

	[HideInInspector]
	public int torqueRPMValuesLen;

	[HideInInspector]
	public float[,] torqueRPMValues = new float[0, 0];

	[HideInInspector]
	public Clutch clutch;

	[HideInInspector]
	public CarController carController;

	private Setup setup;

	private Axles axles;

	[HideInInspector]
	public FuelTank[] fuelTanks;

	private Rigidbody body;

	private Transform myTransform;

	[HideInInspector]
	public Wheel[] poweredWheels;

	public float maxPower = 210f;

	public float maxPowerRPM = 5000f;

	public float maxTorque = 360f;

	public float maxTorqueRPM = 2500f;

	[HideInInspector]
	public float originalMaxPower = 210f;

	[HideInInspector]
	public float maxNetPower;

	[HideInInspector]
	public float maxNetPowerRPM;

	[HideInInspector]
	public float maxNetTorque;

	[HideInInspector]
	public float maxNetTorqueRPM;

	[HideInInspector]
	public float torque;

	private float netTorque;

	private float netTorqueImpulse;

	[HideInInspector]
	public float wheelTireVelo;

	public float minRPM = 1000f;

	public float maxRPM = 6000f;

	public bool canStall;

	[HideInInspector]
	public bool startEngine;

	public bool revLimiter;

	public float revLimiterTime = 0.1f;

	[HideInInspector]
	public bool revLimiterTriggered;

	[HideInInspector]
	public bool revLimiterReleased;

	private float timer;

	public float engineInertia = 0.3f;

	public float drivetrainInertia = 0.02f;

	private float rotationalInertia;

	public float engineFrictionFactor = 0.25f;

	public Vector3 engineOrientation = Vector3.forward;

	public Transmissions transmission;

	public float[] gearRatios = new float[7] { -2.66f, 0f, 2.66f, 1.91f, 1.39f, 1f, 0.71f };

	[HideInInspector]
	public int neutral = 1;

	[HideInInspector]
	public int first;

	[HideInInspector]
	public int firstReverse;

	public float finalDriveRatio = 6.09f;

	public float differentialLockCoefficient = 1f;

	public bool shifter;

	public bool automatic = true;

	public bool autoReverse = true;

	public float shiftDownRPM = 2000f;

	public float shiftUpRPM;

	public float shiftTime = 0.5f;

	public float clutchMaxTorque;

	public bool autoClutch = true;

	public float engageRPM = 1500f;

	public float disengageRPM = 1000f;

	public float _fuelConsumptionAtCostantSpeed = 4.3f;

	public float _fuelConsumptionSpeed = 130f;

	public float currentConsumption;

	[HideInInspector]
	public float istantConsumption;

	[HideInInspector]
	public float RPMAtSpeedInLastGear;

	private float secondsToCover100Km;

	[HideInInspector]
	public float clutchTorqueMultiplier = 1.6f;

	public float clutchEngageSpeed;

	private float clutchPosition;

	[HideInInspector]
	public float throttle;

	[HideInInspector]
	public float idlethrottle;

	private float idlethrottleMinRPMDown;

	private float idleNetTorque;

	[HideInInspector]
	public float startTorque;

	private float startThrottle;

	private float nextStartImpulse;

	private float duration;

	[HideInInspector]
	public bool shiftTriggered;

	[HideInInspector]
	public bool soundPlayed;

	[HideInInspector]
	public float clutchDragImpulse;

	private float wheelImpulse;

	private float TransferredTorque;

	[HideInInspector]
	public float differentialSpeed;

	private float clutchSpeed;

	[HideInInspector]
	public bool engaging;

	private float shiftTimer;

	private float TimeToShiftAgain;

	[HideInInspector]
	public bool CanShiftAgain = true;

	private float ShiftDelay = -1f;

	private float lastShiftTime = -1f;

	public int gear = 1;

	public float rpm;

	private float slipRatio;

	private float idealSlipRatio;

	private float engineAngularVelo;

	[HideInInspector]
	public float angularVelo2RPM = 30f / (float)Math.PI;

	[HideInInspector]
	public float RPM2angularVelo = (float)Math.PI / 30f;

	[HideInInspector]
	public float KW2CV = 1.359f;

	[HideInInspector]
	public float CV2KW = 0.7358f;

	[HideInInspector]
	public float maxPowerDriveShaft;

	[HideInInspector]
	public float currentPower;

	private float maxPowerKW;

	private float maxPowerAngVel;

	private float maxPowerEngineTorque;

	private float P1;

	private float P2;

	private float P3;

	[HideInInspector]
	public float curveFactor;

	[HideInInspector]
	public float frictionTorque;

	[HideInInspector]
	public float powerMultiplier = 1f;

	[HideInInspector]
	public float externalMultiplier = 1f;

	[HideInInspector]
	public float ratio;

	[HideInInspector]
	public float lastGearRatio;

	[HideInInspector]
	public bool changingGear;

	private bool shiftImmediately;

	private int nextGear;

	private float lockingTorqueImpulse;

	private float max_power;

	[HideInInspector]
	public float drivetrainFraction;

	[HideInInspector]
	public float velo;

	[HideInInspector]
	private bool fuel = true;

	[HideInInspector]
	public float RPMAt130Kmh;

	public float fuelConsumptionAtCostantSpeed
	{
		get
		{
			return _fuelConsumptionAtCostantSpeed;
		}
		set
		{
			if (value < 0f)
			{
				_fuelConsumptionAtCostantSpeed = 0f;
			}
			else
			{
				_fuelConsumptionAtCostantSpeed = value;
			}
		}
	}

	public float fuelConsumptionSpeed
	{
		get
		{
			return _fuelConsumptionSpeed;
		}
		set
		{
			if (value < 1f)
			{
				_fuelConsumptionSpeed = 1f;
			}
			else
			{
				_fuelConsumptionSpeed = value;
			}
		}
	}

	private float Sqr(float x)
	{
		return x * x;
	}

	private void Awake()
	{
		engineTorqueFromFile = false;
		torqueRPMValuesLen = 0;
		body = GetComponent<Rigidbody>();
		myTransform = base.transform;
		clutch = new Clutch();
		carController = GetComponent<CarController>();
		setup = GetComponent<Setup>();
		axles = GetComponent<Axles>();
		fuelTanks = GetComponentsInChildren<FuelTank>();
		poweredWheels = axles.rearAxle.wheels;
	}

	private IEnumerator Start()
	{
		if (setup != null && setup.enabled)
		{
			while (setup.loadingSetup)
			{
				yield return new WaitForSeconds(0.02f);
			}
		}
		CalcValues(1f, engineTorqueFromFile);
		if (shiftUpRPM == 0f)
		{
			shiftUpRPM = maxPowerRPM;
		}
		bool found = false;
		for (int i = 0; i < gearRatios.Length; i++)
		{
			if (gearRatios[i] == 0f)
			{
				neutral = i;
				first = neutral + 1;
				firstReverse = neutral - 1;
				found = true;
			}
		}
		if (!found)
		{
			Debug.LogError("UnityCar: Neutral gear (a gear with value 0) is missing in gearRatios array. Neutral gear is mandatory (" + myTransform.name + ")");
		}
		SetTransmission(transmission);
		if (clutch.maxTorque == 0f)
		{
			CalcClutchTorque();
		}
		if (shiftTime == 0f)
		{
			shiftTime = 0.5f;
		}
		lastGearRatio = gearRatios[gearRatios.Length - 1] * finalDriveRatio;
		fuelConsumptionAtCostantSpeed = fuelConsumptionAtCostantSpeed;
		fuelConsumptionSpeed = fuelConsumptionSpeed;
		RPMAtSpeedInLastGear = CalcRPMAtSpeedInLastGear(fuelConsumptionSpeed);
		secondsToCover100Km = 100f / fuelConsumptionSpeed * 3600f;
		CalcIdleThrottle();
		DisengageClutch();
		StartEngine();
	}

	public float CalcRPMAtSpeedInLastGear(float speed)
	{
		if (speed > 0f)
		{
			return speed / (axles.frontAxle.leftWheel.radius * 2f * 0.1885f / (Mathf.Abs(gearRatios[gearRatios.Length - 1]) * finalDriveRatio));
		}
		return 0f;
	}

	public void CalcClutchTorque()
	{
		clutchMaxTorque = Mathf.Round(maxNetTorque * clutchTorqueMultiplier) * powerMultiplier;
		clutch.maxTorque = clutchMaxTorque;
	}

	public void SetTransmission(Transmissions transmission)
	{
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			wheel.lockingTorqueImpulse = 0f;
			wheel.drivetrainInertia = 0f;
			wheel.isPowered = false;
		}
		switch (transmission)
		{
		case Transmissions.FWD:
		{
			Wheel[] wheels5 = axles.frontAxle.wheels;
			foreach (Wheel wheel7 in wheels5)
			{
				wheel7.isPowered = true;
			}
			poweredWheels = axles.frontAxle.wheels;
			axles.frontAxle.powered = true;
			axles.rearAxle.powered = false;
			Axle[] otherAxles4 = axles.otherAxles;
			foreach (Axle axle4 in otherAxles4)
			{
				axle4.powered = false;
			}
			break;
		}
		case Transmissions.RWD:
		{
			Wheel[] wheels4 = axles.rearAxle.wheels;
			foreach (Wheel wheel6 in wheels4)
			{
				wheel6.isPowered = true;
			}
			poweredWheels = axles.rearAxle.wheels;
			axles.frontAxle.powered = false;
			axles.rearAxle.powered = true;
			Axle[] otherAxles3 = axles.otherAxles;
			foreach (Axle axle3 in otherAxles3)
			{
				axle3.powered = false;
			}
			break;
		}
		case Transmissions.XWD:
		{
			List<Wheel> list = new List<Wheel>();
			if (axles.frontAxle.powered)
			{
				Wheel[] wheels = axles.frontAxle.wheels;
				foreach (Wheel wheel3 in wheels)
				{
					wheel3.isPowered = true;
					list.Add(wheel3);
				}
			}
			if (axles.rearAxle.powered)
			{
				Wheel[] wheels2 = axles.rearAxle.wheels;
				foreach (Wheel wheel4 in wheels2)
				{
					wheel4.isPowered = true;
					list.Add(wheel4);
				}
			}
			Axle[] otherAxles2 = axles.otherAxles;
			foreach (Axle axle2 in otherAxles2)
			{
				if (axle2.powered)
				{
					Wheel[] wheels3 = axle2.wheels;
					foreach (Wheel wheel5 in wheels3)
					{
						wheel5.isPowered = true;
						list.Add(wheel5);
					}
				}
			}
			poweredWheels = list.ToArray();
			break;
		}
		case Transmissions.AWD:
		{
			Wheel[] allWheels2 = axles.allWheels;
			foreach (Wheel wheel2 in allWheels2)
			{
				wheel2.isPowered = true;
			}
			poweredWheels = axles.allWheels;
			axles.frontAxle.powered = true;
			axles.rearAxle.powered = true;
			Axle[] otherAxles = axles.otherAxles;
			foreach (Axle axle in otherAxles)
			{
				axle.powered = true;
			}
			break;
		}
		}
		drivetrainFraction = 1f / (float)poweredWheels.Length;
	}

	public float CalcEngineTorque(float factor, float rpm)
	{
		if (engineTorqueFromFile)
		{
			return CalcEngineTorqueExt(factor, rpm);
		}
		return CalcEngineTorqueInt(factor, rpm);
	}

	private float CalcEngineTorqueExt(float factor, float RPM)
	{
		if (torqueRPMValuesLen != 0)
		{
			int num = FindRightPoint(RPM);
			if (num == 0 || num == torqueRPMValuesLen)
			{
				return 0f;
			}
			float num2 = (RPM - torqueRPMValues[num, 0]) / (torqueRPMValues[num - 1, 0] - torqueRPMValues[num, 0]) * torqueRPMValues[num - 1, 1] - (RPM - torqueRPMValues[num - 1, 0]) / (torqueRPMValues[num - 1, 0] - torqueRPMValues[num, 0]) * torqueRPMValues[num, 1];
			return num2 * factor;
		}
		return 0f;
	}

	private int FindRightPoint(float RPM)
	{
		int i;
		for (i = 0; i <= torqueRPMValuesLen - 1 && !(torqueRPMValues[i, 0] > RPM); i++)
		{
		}
		return i;
	}

	private float CalcEngineTorqueInt(float factor, float rpm)
	{
		float num;
		if (rpm < maxTorqueRPM)
		{
			num = maxTorque * (0f - Sqr(rpm / maxTorqueRPM - 1f) + 1f);
		}
		else
		{
			float num2 = maxPower * CV2KW * 1000f / maxPowerAngVel;
			float num3 = (maxTorque - num2) / (2f * maxTorqueRPM * maxPowerRPM - Sqr(maxPowerRPM) - Sqr(maxTorqueRPM));
			float num4 = num3 * Sqr(rpm - maxTorqueRPM) + maxTorque;
			num = ((!(num4 > 0f)) ? 0f : num4);
		}
		if (rpm < 0f || num < 0f)
		{
			num = 0f;
		}
		return num * factor;
	}

	public float CalcEngineTorqueInt_reference(float factor, float RPM)
	{
		float num = RPM * RPM2angularVelo;
		float num2 = P1 + P2 * num + P3 * (num * num);
		if (RPM < maxTorqueRPM)
		{
			num2 *= 1f - Sqr(RPM / maxTorqueRPM - 1f);
		}
		return num2 * 1000f * factor;
	}

	public float CalcEngineFrictionTorque(float factor, float rpm)
	{
		float num = 0.1f;
		if (rpm < minRPM)
		{
			num = 1f - 0.9f * (rpm / minRPM);
		}
		return maxPowerEngineTorque * factor * engineFrictionFactor * (num + (1f - num) * rpm / maxRPM);
	}

	private float CalcEnginePower(float rpm, bool total, float factor)
	{
		if (total)
		{
			return (CalcEngineTorque(factor, rpm) - CalcEngineFrictionTorque(factor, rpm)) * rpm * RPM2angularVelo * 0.001f * KW2CV;
		}
		return CalcEngineTorque(factor, rpm) * rpm * RPM2angularVelo * 0.001f * KW2CV;
	}

	public void StartEngine()
	{
		engineAngularVelo = minRPM * RPM2angularVelo * 1.5f;
	}

	private void CalcEngineMaxPower(float powerMultiplier, bool setMaxPower)
	{
		for (float num = minRPM; num < maxRPM; num += 1f)
		{
			float num2 = CalcEnginePower(num, true, powerMultiplier);
			float num3 = CalcEnginePower(num + 1f, true, powerMultiplier);
			if (num3 > num2)
			{
				maxNetPowerRPM = num + 1f;
				maxNetPower = num3;
			}
			if (setMaxPower)
			{
				num2 = CalcEnginePower(num, false, powerMultiplier);
				num3 = CalcEnginePower(num + 1f, false, powerMultiplier);
				if (num3 > num2)
				{
					maxPowerRPM = num + 1f;
					maxPower = num3;
				}
			}
		}
	}

	private void CalcengineMaxTorque(float powerMultiplier, bool setMaxTorque)
	{
		for (float num = minRPM; num < maxRPM; num += 1f)
		{
			float num2 = CalcEngineTorque(powerMultiplier, num) - CalcEngineFrictionTorque(powerMultiplier, num);
			float num3 = CalcEngineTorque(powerMultiplier, num + 1f) - CalcEngineFrictionTorque(powerMultiplier, num + 1f);
			if (num3 > num2)
			{
				maxNetTorqueRPM = num + 1f;
				maxNetTorque = num3;
			}
			if (setMaxTorque)
			{
				num2 = CalcEngineTorque(powerMultiplier, num);
				num3 = CalcEngineTorque(powerMultiplier, num + 1f);
				if (num3 > num2)
				{
					maxTorqueRPM = num + 1f;
					maxTorque = num3;
				}
			}
		}
	}

	public void CalcIdleThrottle()
	{
		float num = CalcEngineFrictionTorque(powerMultiplier, minRPM);
		float num2 = CalcEngineTorque(powerMultiplier, minRPM);
		idleNetTorque = num2 - num;
		idlethrottle = 0f;
		while (idlethrottle < 1f && !(num2 * idlethrottle >= num))
		{
			idlethrottle += 0.0001f;
		}
		idlethrottleMinRPMDown = idlethrottle;
	}

	public void CalcValues(float externalFactor, bool setMaxPower)
	{
		maxPowerAngVel = maxPowerRPM * RPM2angularVelo;
		maxPowerKW = maxPower * CV2KW * externalFactor;
		maxPowerDriveShaft = maxPower * externalFactor;
		P1 = maxPowerKW / maxPowerAngVel;
		P2 = maxPowerKW / (maxPowerAngVel * maxPowerAngVel);
		P3 = (0f - maxPowerKW) / (maxPowerAngVel * maxPowerAngVel * maxPowerAngVel);
		maxPowerEngineTorque = CalcEngineTorque(1f, maxPowerRPM);
		CalcengineMaxTorque(1f, setMaxPower);
		CalcEngineMaxPower(1f, setMaxPower);
		originalMaxPower = maxPower;
		curveFactor = externalFactor;
	}

	private void FixedUpdate()
	{
		if (clutch == null)
		{
			clutch = new Clutch();
			CalcClutchTorque();
		}
		if (shifter)
		{
			automatic = false;
		}
		ratio = gearRatios[gear] * finalDriveRatio;
		if (rpm <= minRPM + maxRPM * 0.05f)
		{
			idlethrottle = idlethrottleMinRPMDown * ((minRPM + 500f - Mathf.Clamp(rpm, minRPM, rpm)) * 0.002f);
		}
		else
		{
			idlethrottle = 0f;
		}
		currentPower = CalcEnginePower(rpm, true, powerMultiplier);
		float num = engineInertia * powerMultiplier * externalMultiplier;
		float num2 = drivetrainInertia * powerMultiplier * externalMultiplier;
		velo = Mathf.Abs(myTransform.InverseTransformDirection(body.velocity).z);
		if ((rpm >= engageRPM || engaging) && autoClutch && carController.clutchInput == 0f && clutch.GetClutchPosition() != 1f && carController.handbrakeInput == 0f && ratio != 0f)
		{
			EngageClutch();
		}
		if (rpm <= disengageRPM && !engaging && autoClutch)
		{
			DisengageClutch();
		}
		if (changingGear)
		{
			DoGearShifting();
		}
		else
		{
			lastShiftTime = 0f;
		}
		if (automatic)
		{
			autoClutch = true;
			if (!CanShiftAgain)
			{
				TimeToShiftAgain = Mathf.Clamp01((Time.time - ShiftDelay) / (shiftTime + shiftTime / 2f));
				if (TimeToShiftAgain >= 1f)
				{
					CanShiftAgain = true;
				}
			}
			if (!changingGear)
			{
				if (rpm >= shiftUpRPM)
				{
					if (gear >= 0 && gear < gearRatios.Length - 1 && Mathf.Abs(slipRatio / idealSlipRatio) <= 1f && clutch.GetClutchPosition() != 0f && clutch.speedDiff < 50f && !engaging)
					{
						if (CanShiftAgain && OnGround())
						{
							if (gearRatios[gear] > 0f)
							{
								Shift(gear + 1);
							}
							else
							{
								Shift(gear - 1);
							}
						}
						CanShiftAgain = false;
						ShiftDelay = Time.time;
					}
				}
				else if (rpm <= shiftDownRPM && gear != first && gear != firstReverse && gear != neutral && gear > 0 && gear < gearRatios.Length && clutch.GetClutchPosition() != 0f && Mathf.Abs(clutch.speedDiff) < 50f && !engaging && CanShiftAgain && OnGround())
				{
					if (velo < 3f)
					{
						Shift(first);
					}
					else if (gearRatios[gear] > 0f)
					{
						Shift(gear - 1);
					}
					else
					{
						Shift(gear + 1);
					}
					CanShiftAgain = false;
					ShiftDelay = Time.time;
				}
			}
		}
		float num3 = 0f;
		wheelImpulse = 0f;
		rotationalInertia = 0f;
		wheelTireVelo = 0f;
		Wheel[] array = poweredWheels;
		foreach (Wheel wheel in array)
		{
			num3 += wheel.angularVelocity;
			wheelImpulse += wheel.wheelImpulse;
			rotationalInertia += wheel.rotationalInertia;
			wheelTireVelo += wheel.wheelTireVelo;
		}
		num3 *= drivetrainFraction;
		wheelTireVelo *= drivetrainFraction;
		float num4 = num2 + rotationalInertia;
		if (rpm < 20f && startTorque == 0f)
		{
			differentialSpeed = 0f;
			wheelImpulse = 0f;
		}
		clutchSpeed = differentialSpeed * ratio;
		fuel = true;
		if (fuelTanks.Length != 0)
		{
			float num5 = velo * 3.6f;
			float num6 = Mathf.Clamp(num5, 50f, num5) / fuelConsumptionSpeed;
			num6 *= num6;
			if (RPMAtSpeedInLastGear != 0f)
			{
				istantConsumption = rpm * throttle * fuelConsumptionAtCostantSpeed / (RPMAtSpeedInLastGear * secondsToCover100Km) * num6;
			}
			if (velo > 1f)
			{
				currentConsumption = istantConsumption / velo * 100000f;
			}
			else
			{
				currentConsumption = 0f;
			}
			fuel = false;
			FuelTank[] array2 = fuelTanks;
			foreach (FuelTank fuelTank in array2)
			{
				if (fuelTank.currentFuel != 0f)
				{
					fuel = true;
				}
			}
		}
		torque = CalcEngineTorque(powerMultiplier, rpm) * (throttle + startThrottle) * (float)(fuel ? 1 : 0);
		frictionTorque = CalcEngineFrictionTorque(powerMultiplier, rpm);
		startThrottle = 0f;
		startTorque = 0f;
		netTorque = torque - frictionTorque;
		if (rpm < 20f && startTorque == 0f)
		{
			netTorque = 0f;
		}
		if (rpm < minRPM)
		{
			startThrottle = 1f - rpm / minRPM;
		}
		if (startEngine && rpm < minRPM && Time.time > nextStartImpulse)
		{
			if (duration == 0f)
			{
				duration = Time.time + 0.1f;
			}
			if (Time.time > duration)
			{
				nextStartImpulse = Time.time + 0.2f;
			}
			startThrottle = 1f;
			startTorque = idleNetTorque;
		}
		else
		{
			duration = 0f;
		}
		netTorqueImpulse = (netTorque + startTorque) * Time.deltaTime;
		if (engineAngularVelo >= maxRPM * RPM2angularVelo)
		{
			if (revLimiterTime == 0f && revLimiter)
			{
				engineAngularVelo = maxRPM * RPM2angularVelo;
			}
			else
			{
				revLimiterTriggered = true;
			}
		}
		else if (engineAngularVelo <= minRPM * RPM2angularVelo && !canStall)
		{
			engineAngularVelo = minRPM * RPM2angularVelo;
		}
		else if (engineAngularVelo < 0f)
		{
			engineAngularVelo = 0f;
		}
		rpm = engineAngularVelo * angularVelo2RPM;
		if (ratio == 0f || clutch.GetClutchPosition() == 0f)
		{
			clutchDragImpulse = 0f;
			differentialSpeed = num3;
			if (autoClutch)
			{
				DisengageClutch();
			}
			body.AddTorque(-engineOrientation * Mathf.Min(Mathf.Abs(netTorque), 2000f) * Mathf.Sign(netTorque));
		}
		else
		{
			clutchDragImpulse = clutch.GetDragImpulse(engineAngularVelo, clutchSpeed, num, num4, ratio, wheelImpulse, netTorqueImpulse);
		}
		engineAngularVelo += (netTorqueImpulse - clutchDragImpulse) / num;
		differentialSpeed += (wheelImpulse + clutchDragImpulse * ratio) / num4;
		if (float.IsNaN(differentialSpeed))
		{
			differentialSpeed = 0f;
		}
		float num7 = differentialSpeed - num3;
		slipRatio = 0f;
		idealSlipRatio = 0f;
		float num8 = maxRPM / (Mathf.Abs(ratio) * angularVelo2RPM);
		Wheel[] array3 = poweredWheels;
		foreach (Wheel wheel2 in array3)
		{
			if (revLimiter && wheel2.angularVelocity > num8)
			{
				wheel2.angularVelocity = num8;
			}
			lockingTorqueImpulse = (num3 - wheel2.angularVelocity) * differentialLockCoefficient * Time.deltaTime;
			wheel2.drivetrainInertia = num2 * ratio * ratio * drivetrainFraction * clutch.GetClutchPosition();
			wheel2.angularVelocity += num7;
			wheel2.lockingTorqueImpulse = lockingTorqueImpulse;
			slipRatio += wheel2.slipRatio * drivetrainFraction;
			idealSlipRatio += wheel2.idealSlipRatio * drivetrainFraction;
		}
		if (revLimiter)
		{
			if (revLimiterTriggered)
			{
				revLimiterReleased = false;
				timer += Time.deltaTime;
				if (timer >= revLimiterTime)
				{
					timer = 0f;
					revLimiterTriggered = false;
					revLimiterReleased = true;
				}
			}
			else
			{
				revLimiterReleased = false;
			}
		}
		else
		{
			revLimiterTriggered = false;
			revLimiterReleased = false;
		}
	}

	private void DoGearShifting()
	{
		if (shiftImmediately)
		{
			gear = nextGear;
			if (nextGear != neutral)
			{
				shiftTriggered = true;
			}
			changingGear = false;
		}
		else
		{
			if (!(throttle <= idlethrottle * 1.1f) && automatic)
			{
				return;
			}
			if (lastShiftTime == 0f)
			{
				lastShiftTime = Time.time;
			}
			shiftTimer = (Time.time - lastShiftTime) / shiftTime;
			if (shiftTimer < 1f)
			{
				DisengageClutch();
			}
			if (shiftTimer >= 0.33f)
			{
				gear = neutral;
			}
			if (shiftTimer >= 0.66f)
			{
				gear = nextGear;
				if (!soundPlayed)
				{
					shiftTriggered = true;
				}
			}
			if (shiftTimer >= 1f && (rpm < engageRPM || carController.clutchInput != 0f))
			{
				changingGear = false;
			}
		}
	}

	private void EngageClutch()
	{
		engaging = true;
		int num = 1;
		if (rpm < maxPowerRPM / 2f)
		{
			clutchEngageSpeed = Mathf.Clamp(clutchDragImpulse / netTorqueImpulse, Time.fixedDeltaTime * 2f, 1f);
			num = ((clutchDragImpulse < netTorqueImpulse || netTorque < 1f) ? 1 : (-1));
		}
		else
		{
			clutchEngageSpeed = 0.1f;
		}
		if (clutchEngageSpeed != 0f)
		{
			clutchPosition += Time.deltaTime / clutchEngageSpeed * (float)num * ((!(throttle > idlethrottle)) ? 1f : throttle);
		}
		clutchPosition = Mathf.Clamp01(clutchPosition);
		clutch.SetClutchPosition(clutchPosition);
		if (clutchPosition == 1f)
		{
			engaging = false;
			changingGear = false;
		}
	}

	private void DisengageClutch()
	{
		clutchPosition = 0f;
		clutch.SetClutchPosition(clutchPosition);
	}

	public bool OnGround()
	{
		bool flag = false;
		if (poweredWheels != null)
		{
			Wheel[] array = poweredWheels;
			foreach (Wheel wheel in array)
			{
				flag = wheel.onGroundDown;
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	public void Shift(int m_gear)
	{
		if (m_gear <= gearRatios.Length - 1 && m_gear >= 0 && !changingGear && (autoClutch || clutch.GetClutchPosition() == 0f))
		{
			soundPlayed = false;
			changingGear = true;
			nextGear = m_gear;
			if (nextGear == neutral || (gear == neutral && nextGear == first) || (gear == neutral && nextGear == firstReverse) || shifter)
			{
				shiftImmediately = true;
			}
			else
			{
				shiftImmediately = false;
			}
		}
	}
}
