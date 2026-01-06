using UnityEngine;

[RequireComponent(typeof(Axles))]
public abstract class CarController : MonoBehaviour
{
	private bool counterSteering;

	private float steerTimer;

	private Wheel[] allWheels;

	private Transform myTransform;

	public float brake;

	public float throttle;

	public float steering;

	private float oldSteering;

	private float deltaSteering;

	private float steeringVelo;

	private float maxSteer = 1f;

	private float maxThrottle = 1f;

	[HideInInspector]
	public bool brakeKey;

	[HideInInspector]
	public bool accelKey;

	[HideInInspector]
	public float steerInput;

	[HideInInspector]
	public float brakeInput;

	[HideInInspector]
	public float throttleInput;

	[HideInInspector]
	public float handbrakeInput;

	[HideInInspector]
	public float clutchInput;

	[HideInInspector]
	public bool startEngineInput;

	private Rigidbody body;

	protected Drivetrain drivetrain;

	private CarDynamics cardynamics;

	private Axles axles;

	private float velo;

	private float veloKmh;

	public bool smoothInput = true;

	public float throttleTime = 0.1f;

	public float throttleReleaseTime = 0.1f;

	public float maxThrottleInReverse = 1f;

	public float brakesTime = 0.1f;

	public float brakesReleaseTime = 0.1f;

	public float steerTime = 0.1f;

	public float steerReleaseTime = 0.1f;

	public float veloSteerTime = 0.05f;

	public float veloSteerReleaseTime = 0.05f;

	public float steerCorrectionFactor;

	public bool steerAssistance = true;

	public float SteerAssistanceMinVelocity = 20f;

	public bool TCS = true;

	[HideInInspector]
	public bool TCSTriggered;

	public float TCSAllowedSlip;

	public float TCSMinVelocity = 20f;

	[HideInInspector]
	public float externalTCSThreshold;

	public bool ABS = true;

	[HideInInspector]
	public bool ABSTriggered;

	public float ABSAllowedSlip;

	public float ABSMinVelocity = 20f;

	public bool ESP = true;

	[HideInInspector]
	public bool ESPTriggered;

	public float ESPStrength = 2f;

	public float ESPMinVelocity = 35f;

	[HideInInspector]
	public bool reverse;

	protected abstract void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out bool startEngineInput, out int targetGear);

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		cardynamics = GetComponent<CarDynamics>();
		drivetrain = GetComponent<Drivetrain>();
		axles = GetComponent<Axles>();
		allWheels = axles.allWheels;
		myTransform = base.transform;
	}

	private void Update()
	{
		int targetGear;
		GetInput(out throttleInput, out brakeInput, out steerInput, out handbrakeInput, out clutchInput, out startEngineInput, out targetGear);
		if (!drivetrain.changingGear && targetGear != drivetrain.gear)
		{
			drivetrain.Shift(targetGear);
		}
		if (drivetrain.automatic && drivetrain.autoReverse)
		{
			if (brakeInput > 0f && velo <= 0.5f)
			{
				reverse = true;
				if (drivetrain.gear != drivetrain.firstReverse)
				{
					drivetrain.Shift(drivetrain.firstReverse);
				}
			}
			if (throttleInput > 0f && velo <= 0.5f)
			{
				reverse = false;
				if (drivetrain.gear != drivetrain.first)
				{
					drivetrain.Shift(drivetrain.first);
				}
			}
			if (reverse)
			{
				float num = throttleInput;
				throttleInput = brakeInput;
				brakeInput = num;
			}
		}
		else
		{
			reverse = false;
		}
		brakeKey = brakeInput > 0f;
		accelKey = throttleInput > 0f;
	}

	private void FixedUpdate()
	{
		maxThrottle = 1f;
		oldSteering = steering;
		velo = cardynamics.velo;
		veloKmh = cardynamics.velo * 3.6f;
		bool flag = drivetrain.OnGround();
		float num = cardynamics.LateralSlipVeloRearWheels();
		num = ((!(Mathf.Abs(num) < 0.1f)) ? num : 0f);
		counterSteering = Mathf.Sign(num) == Mathf.Sign(steering) && num != 0f && steering != 0f;
		if (smoothInput)
		{
			SmoothSteer();
			smoothThrottle();
			smoothBrakes();
		}
		else
		{
			steering = steerInput;
			brake = brakeInput;
			throttle = throttleInput;
			if (drivetrain.changingGear && drivetrain.automatic)
			{
				throttle = 0f;
			}
		}
		if (steerAssistance && drivetrain.ratio > 0f && veloKmh > SteerAssistanceMinVelocity && (!counterSteering || steerInput == 0f))
		{
			SteerAssistance();
		}
		else
		{
			steerTimer = 0f;
			maxSteer = 1f;
		}
		TCSTriggered = false;
		if (TCS && drivetrain.ratio > 0f && drivetrain.clutch.GetClutchPosition() >= 0.9f && flag && throttle > drivetrain.idlethrottle && veloKmh > TCSMinVelocity)
		{
			DoTCS();
		}
		ESPTriggered = false;
		if (ESP && drivetrain.ratio > 0f && flag && veloKmh > ESPMinVelocity)
		{
			DoESP();
		}
		ABSTriggered = false;
		if (ABS && brake > 0f && veloKmh > ABSMinVelocity && flag)
		{
			DoABS();
		}
		float max = ((!(drivetrain.gearRatios[drivetrain.gear] > 0f)) ? maxThrottleInReverse : maxThrottle);
		if (drivetrain.revLimiterTriggered)
		{
			throttle = 0f;
		}
		else if (drivetrain.revLimiterReleased)
		{
			throttle = throttleInput;
		}
		else
		{
			throttle = Mathf.Clamp(throttle, drivetrain.idlethrottle, max);
		}
		brake = Mathf.Clamp01(brake);
		steering = Mathf.Clamp(steering, -1f, 1f);
		deltaSteering = steering - oldSteering;
		Wheel[] array = allWheels;
		foreach (Wheel wheel in array)
		{
			if (!ABS || !(veloKmh > ABSMinVelocity) || !(brakeInput > 0f))
			{
				wheel.brake = brake;
			}
			wheel.handbrake = handbrakeInput;
			wheel.steering = steering;
			wheel.deltaSteering = deltaSteering;
		}
		drivetrain.throttle = throttle;
		if (drivetrain.clutch != null && (clutchInput != 0f || !drivetrain.autoClutch))
		{
			drivetrain.clutch.SetClutchPosition(1f - clutchInput);
		}
		drivetrain.startEngine = startEngineInput;
	}

	private void SteerAssistance()
	{
		float num = 0f;
		Wheel[] array = allWheels;
		foreach (Wheel wheel in array)
		{
			num += wheel.lateralSlip;
		}
		num /= (float)allWheels.Length;
		maxSteer = Mathf.Clamp(1f - Mathf.Abs(num), -1f, 1f);
		if (steerTimer <= 1f)
		{
			steerTimer += Time.deltaTime;
			maxSteer = 1f - steerTimer + steerTimer * maxSteer;
		}
		oldSteering = steering;
		float num2 = Mathf.Sign(num);
		if (maxSteer > 0f)
		{
			steering = Mathf.Clamp(steering, 0f - maxSteer, maxSteer);
		}
		else
		{
			steering = Mathf.Clamp(steering, num2 * maxSteer / 2f, num2 * maxSteer / 2f);
		}
		steeringVelo = steering - oldSteering;
		float num3 = 40f * cardynamics.fixedTimeStepScalar;
		float num4 = steeringVelo * num3;
		float num5 = num4;
		steering -= num5 * Time.deltaTime;
	}

	private void SmoothSteer()
	{
		if (steerInput < steering)
		{
			float num = ((!(steering > 0f) || steerInput != 0f) ? (1f / (steerTime + veloSteerTime * velo)) : (1f / (steerReleaseTime + veloSteerReleaseTime * velo)));
			if (counterSteering)
			{
				num *= 1f + Mathf.Abs(Mathf.Abs(steering) - Mathf.Abs(steerInput)) * steerCorrectionFactor;
			}
			steering -= num * Time.deltaTime;
			if (steerInput > steering)
			{
				steering = steerInput;
			}
		}
		else if (steerInput > steering)
		{
			float num = ((!(steering < 0f) || steerInput != 0f) ? (1f / (steerTime + veloSteerTime * velo)) : (1f / (steerReleaseTime + veloSteerReleaseTime * velo)));
			if (counterSteering)
			{
				num *= 1f + Mathf.Abs(Mathf.Abs(steering) - Mathf.Abs(steerInput)) * steerCorrectionFactor;
			}
			steering += num * Time.deltaTime;
			if (steerInput < steering)
			{
				steering = steerInput;
			}
		}
	}

	private void smoothThrottle()
	{
		if (throttleInput > 0f && (!drivetrain.changingGear || !drivetrain.automatic))
		{
			if (throttleInput < throttle)
			{
				throttle -= Time.deltaTime / throttleReleaseTime;
				if (throttleInput > throttle)
				{
					throttle = throttleInput;
				}
			}
			else if (throttleInput > throttle)
			{
				throttle += Time.deltaTime / throttleTime;
				if (throttleInput < throttle)
				{
					throttle = throttleInput;
				}
			}
		}
		else
		{
			throttle -= Time.deltaTime / throttleReleaseTime;
		}
	}

	private void smoothBrakes()
	{
		if (brakeInput > 0f)
		{
			if (brakeInput < brake)
			{
				brake -= Time.deltaTime / brakesReleaseTime;
				if (brakeInput > brake)
				{
					brake = brakeInput;
				}
			}
			else if (brakeInput > brake)
			{
				brake += Time.deltaTime / brakesTime;
				if (brakeInput < brake)
				{
					brake = brakeInput;
				}
			}
		}
		else
		{
			brake -= Time.deltaTime / brakesReleaseTime;
		}
	}

	private void DoABS()
	{
		Wheel[] array = allWheels;
		foreach (Wheel wheel in array)
		{
			float num = (0f - wheel.longitudinalSlip) * (1f - ABSAllowedSlip);
			if (num >= 1f)
			{
				wheel.brake = 0f;
				ABSTriggered = true;
			}
			else
			{
				wheel.brake = brake;
				ABSTriggered = false;
			}
		}
	}

	private void DoTCS()
	{
		float num = 0f;
		float num2 = 0f;
		Wheel[] poweredWheels = drivetrain.poweredWheels;
		foreach (Wheel wheel in poweredWheels)
		{
			num2 = Mathf.Max(wheel.longitudinalSlip, Mathf.Abs(wheel.lateralSlip) * 1.5f);
			if (num2 > num)
			{
				num = num2;
			}
		}
		TCSTriggered = false;
		float num3 = num * (1f - TCSAllowedSlip) + externalTCSThreshold;
		if (num3 >= 1f)
		{
			maxThrottle = Mathf.Clamp(2f - num3, 0f, 1f);
			if (maxThrottle > 0.9f)
			{
				maxThrottle = 1f;
			}
			else
			{
				TCSTriggered = true;
			}
		}
	}

	private void DoESP()
	{
		Vector3 forward = myTransform.forward;
		Vector3 velocity = body.velocity;
		velocity -= myTransform.up * Vector3.Dot(velocity, myTransform.up);
		velocity.Normalize();
		float num = 0f;
		if (velo > 1f)
		{
			num = 0f - Mathf.Asin(Vector3.Dot(Vector3.Cross(forward, velocity), myTransform.up));
		}
		ESPTriggered = false;
		if (num > 0.1f)
		{
			if ((bool)axles.frontAxle.leftWheel)
			{
				axles.frontAxle.leftWheel.brake = Mathf.Clamp01(axles.frontAxle.leftWheel.brake + Mathf.Abs(num) * ESPStrength);
			}
			maxThrottle = Mathf.Max(maxThrottle - num * ESPStrength, drivetrain.idlethrottle);
			ESPTriggered = true;
		}
		else if (num < -0.1f)
		{
			if ((bool)axles.frontAxle.rightWheel)
			{
				axles.frontAxle.rightWheel.brake = Mathf.Clamp01(axles.frontAxle.rightWheel.brake + Mathf.Abs(num) * ESPStrength);
			}
			maxThrottle = Mathf.Max(maxThrottle + num * ESPStrength, drivetrain.idlethrottle);
			ESPTriggered = true;
		}
	}
}
