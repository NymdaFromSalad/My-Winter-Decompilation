using System;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	[HideInInspector]
	public WheelPos wheelPos;

	[HideInInspector]
	public PhysicMaterial physicMaterial;

	[HideInInspector]
	public CarDynamics.SurfaceType surfaceType;

	public GameObject model;

	public GameObject caliperModel;

	private LineRenderer suspensionLineRenderer;

	[HideInInspector]
	public bool showForces;

	[HideInInspector]
	public bool isPowered;

	private float relaxLong;

	private float relaxLat;

	private float tireDeflection;

	private float lateralTireDeflection;

	private float lateralTireStiffness;

	private float longitudinalTireStiffness;

	private float overTurningMoment;

	[HideInInspector]
	public float pressure = 200f;

	[HideInInspector]
	public float optimalPressure = 200f;

	public bool tirePuncture;

	private float pressureFactor;

	[HideInInspector]
	public bool rimScraping;

	private float verticalTireStiffness;

	private float tireDampingRate;

	private float cos;

	private float differentialSlipRatio;

	private float tanSlipAngle;

	private float deltaRatio1;

	private float deltaAngle1;

	private float localScale;

	[HideInInspector]
	public float lateralSlipVelo;

	[HideInInspector]
	public float longitunalSlipVelo;

	private float[] slipRatio_hat;

	private float[] slipAngle_hat;

	private Vector3 force;

	private Vector3 totalForce;

	[HideInInspector]
	public Vector3 pos;

	private Vector3 modelPosition;

	[HideInInspector]
	public float Fx;

	[HideInInspector]
	public float maxFx;

	[HideInInspector]
	public float Fy;

	[HideInInspector]
	public float maxFy;

	[HideInInspector]
	public float Mz;

	[HideInInspector]
	public float maxMz;

	private Vector3 latForce;

	private Vector3 longForce;

	private int layerMask;

	private int i;

	public bool onGroundDown;

	[HideInInspector]
	public RaycastHit hitDown;

	[HideInInspector]
	public float originalMass;

	public float mass = 50f;

	public float radius = 0.34f;

	public float rimRadius;

	private float sidewallHeight;

	public float width = 0.2f;

	[HideInInspector]
	public float suspensionTravel = 0.2f;

	[HideInInspector]
	public float suspensionRate = 20000f;

	[HideInInspector]
	public float bumpRate = 4000f;

	[HideInInspector]
	public float reboundRate = 4000f;

	[HideInInspector]
	public float fastBumpFactor = 0.3f;

	[HideInInspector]
	public float fastReboundFactor = 0.3f;

	[HideInInspector]
	public float rotationalInertia;

	[HideInInspector]
	public float totalRotationalInertia;

	public float brakeFrictionTorque = 1500f;

	[HideInInspector]
	public float handbrakeFrictionTorque;

	private float rollingResistanceTorque;

	[HideInInspector]
	public float rollingFrictionCoefficient = 0.018f;

	private float totalFrictionTorque;

	private float totalFrictionTorqueImpulse;

	private float frictionAngularDelta;

	private float staticFrictionCoefficient = 1f;

	private bool isDirty;

	[HideInInspector]
	public float gripMaterial = 1f;

	[HideInInspector]
	public float sidewaysGripFactor = 1f;

	[HideInInspector]
	public float forwardGripFactor = 1f;

	[HideInInspector]
	public float gripPressure = 1f;

	[HideInInspector]
	public float gripSlip;

	[HideInInspector]
	public float gripVelo;

	private float m_gripMaterial;

	[HideInInspector]
	public float maxSteeringAngle = 33f;

	[HideInInspector]
	public float[] a = new float[15]
	{
		1.5f, -40f, 1600f, 2600f, 8.7f, 0.014f, -0.24f, 1f, -0.03f, -0.0013f,
		-0.06f, -8.5f, -0.29f, 17.8f, -2.4f
	};

	[HideInInspector]
	public float[] b = new float[11]
	{
		1.5f, -80f, 1950f, 23.3f, 390f, 0.05f, 0f, 0.055f, -0.024f, 0.014f,
		0.26f
	};

	[HideInInspector]
	public float[] c = new float[18]
	{
		2.2f, -3.9f, -3.9f, -1.26f, -8.2f, 0.025f, 0f, 0.044f, -0.58f, 0.18f,
		0.043f, 0.048f, -0.0035f, -0.18f, 0.14f, -1.029f, 0.27f, -1.1f
	};

	private CarDynamics cardynamics;

	private Drivetrain drivetrain;

	private Axles axles;

	private Transform myTransform;

	public float wheelFrictionTorque;

	[HideInInspector]
	public float lockingTorqueImpulse;

	[HideInInspector]
	public float roadTorqueImpulse;

	[HideInInspector]
	public float drivetrainInertia;

	public float brake;

	public float handbrake;

	public float steering;

	[HideInInspector]
	public float deltaSteering;

	[HideInInspector]
	public float antiRollBarForce;

	[HideInInspector]
	public float wheelImpulse;

	[HideInInspector]
	public float angularVelocity;

	private float oldAngularVelocity;

	[HideInInspector]
	public float slipVelo;

	private float slipVeloSmoke;

	private float slipVeloThreshold = 4f;

	[HideInInspector]
	public float slipRatio;

	[HideInInspector]
	public float slipAngle;

	[HideInInspector]
	public float longitudinalSlip;

	[HideInInspector]
	public float lateralSlip;

	[HideInInspector]
	public float idealSlipRatio;

	[HideInInspector]
	public float idealSlipAngle;

	private float slipSkidAmount;

	private float slipSmokeAmount;

	public float compression;

	private float overTravel;

	[HideInInspector]
	public float wheelTireVelo;

	[HideInInspector]
	public float wheelRoadVelo;

	[HideInInspector]
	public float absRoadVelo;

	[HideInInspector]
	public float dampAbsRoadVelo;

	[HideInInspector]
	public float wheelRoadVeloLat;

	[HideInInspector]
	public Vector3 wheelVelo;

	[HideInInspector]
	public Vector3 groundNormal;

	private float rotation;

	public float normalForce;

	private float suspensionForce;

	private float tireForce;

	private float bumpStopForce;

	private float springForce;

	private float criticalDamping;

	private float radialDampingRatio;

	private float normalVelocity;

	private float oldNormalVelocity;

	private float deltaVelocity;

	private float accel;

	private float nextVelocity;

	private float nextCompression;

	private float deflectionVelocity;

	private float inclination;

	[HideInInspector]
	public float camber;

	[HideInInspector]
	public float deltaCamber;

	private float inclination_sin;

	private float inclination_rad;

	[HideInInspector]
	public float caster;

	private float roadDistance;

	private float springLength;

	private float radiusLoaded;

	private Vector3 roadForce;

	private Vector3 up;

	private Vector3 right;

	private Vector3 forwardNormal;

	private Vector3 rightNormal;

	private Quaternion localRotation = Quaternion.identity;

	private int lastSkid = -1;

	private Rigidbody body;

	private Transform trs;

	private Transform modelTransform;

	private Transform caliperModelTransform;

	private Skidmarks skidmarks;

	private ParticleEmitter skidSmoke;

	private bool isSkidSmoke = true;

	private bool isSkidMark = true;

	[HideInInspector]
	public int axleWheelsLength;

	[HideInInspector]
	public int axlesNumber;

	private float velo;

	private float B_;

	private float b_;

	private float PseudoAtan(float x)
	{
		float num = Mathf.Abs(x);
		return x * (1f + 1.1f * num) / (1f + 2f * (1.6f * num + 1.1f * x * x) / (float)Math.PI);
	}

	private float CalcLongitudinalForce(float Fz, float slipRatio)
	{
		slipRatio *= 100f;
		float num = Fz * Fz;
		float num2 = b[0];
		float num3 = (maxFy = (b[1] * num + b[2] * Fz) * m_gripMaterial * forwardGripFactor);
		float num4 = (b[3] * num + b[4] * Fz) * Mathf.Exp((0f - b[5]) * Fz);
		relaxLong = 0f;
		if (pressure != 0f && cardynamics.tirePressureEnabled && longitudinalTireStiffness > 0f)
		{
			relaxLong = num4 / longitudinalTireStiffness;
		}
		float num5 = num4 / (num2 * num3);
		float num6 = b[6] * num + b[7] * Fz + b[8];
		float num7 = 0f;
		float num8 = slipRatio + num7;
		float num9 = num5 * num8;
		return num3 * Mathf.Sin(num2 * PseudoAtan(num9 - num6 * (num9 - PseudoAtan(num9))));
	}

	private float CalcLateralForce(float Fz, float slipAngle, float inclination)
	{
		float num = Fz * Fz;
		float num2 = a[0];
		float num3 = (maxFx = (a[1] * num + a[2] * Fz) * m_gripMaterial * sidewaysGripFactor);
		float num4 = a[3] * Mathf.Sin(2f * PseudoAtan(Fz / a[4])) * (1f - a[5] * Mathf.Abs(inclination));
		relaxLat = 0f;
		if (pressure != 0f && cardynamics.tirePressureEnabled && lateralTireStiffness > 0f)
		{
			relaxLat = num4 / lateralTireStiffness;
		}
		float num5 = num4 / (num2 * num3);
		float num6 = a[6] * Fz + a[7];
		float num7 = 0f;
		float num8 = (0f - a[11]) * Fz * Mathf.Abs(inclination * Mathf.Clamp(absRoadVelo * 100f, 0f, 1f)) + a[12] * Fz + a[13];
		float num9 = slipAngle + num7;
		float num10 = num5 * num9;
		return num3 * Mathf.Sin(num2 * PseudoAtan(num10 - num6 * (num10 - PseudoAtan(num10)))) + num8;
	}

	private float CalcAligningForce(float Fz, float slipAngle, float inclination)
	{
		float num = Fz * Fz;
		float num2 = c[0];
		float num3 = (maxMz = (c[1] * num + c[2] * Fz) * m_gripMaterial * sidewaysGripFactor);
		float num4 = (c[3] * num + c[4] * Fz) * (1f - c[6] * Mathf.Abs(inclination)) * Mathf.Exp((0f - c[5]) * Fz);
		float num5 = num4 / (num2 * num3);
		float num6 = (c[7] * num + c[8] * Fz + c[9]) * (1f - c[10] * Mathf.Abs(inclination));
		float num7 = 0f;
		float num8 = (c[14] * num + c[15] * Fz) * inclination + c[16] * Fz + c[17];
		float num9 = slipAngle + num7;
		float num10 = num5 * num9;
		return num3 * Mathf.Sin(num2 * PseudoAtan(num10 + num6 * (PseudoAtan(num10) - num10))) + num8;
	}

	private Vector3 CalcForces(float Fz)
	{
		Fz *= 0.001f;
		float num = 1f;
		float num2 = 20f;
		if (Fz > num2)
		{
			num = Mathf.Clamp(Fz / num2, 1f, 2f);
			Fz = num2;
		}
		if (slipRatio_hat != null && slipAngle_hat != null)
		{
			LookupIdealSlipRatioIdealSlipAngle(Fz);
		}
		wheelTireVelo = angularVelocity * radiusLoaded;
		absRoadVelo = Mathf.Abs(wheelRoadVelo);
		B_ = relaxLong * 100f;
		b_ = relaxLat * 100f;
		if (B_ < 0.35f * cardynamics.invFixedTimeStepScalar)
		{
			B_ = 0.35f * cardynamics.invFixedTimeStepScalar;
		}
		if (b_ < 0.5f * cardynamics.invFixedTimeStepScalar)
		{
			b_ = 0.5f * cardynamics.invFixedTimeStepScalar;
		}
		float num3 = velo * 0.02f;
		if (num3 < 1f)
		{
			num3 = 1f;
		}
		B_ *= num3;
		b_ *= num3;
		float num4 = Mathf.Max(absRoadVelo, dampAbsRoadVelo);
		deltaRatio1 = wheelTireVelo - wheelRoadVelo - num4 * differentialSlipRatio;
		deltaRatio1 /= B_;
		differentialSlipRatio += deltaRatio1 * Time.deltaTime;
		slipRatio = differentialSlipRatio;
		slipRatio = Mathf.Clamp(slipRatio, -1.5f, 1.5f);
		float num5 = 1f;
		float num6 = Mathf.Abs(slipAngle);
		float num7 = idealSlipAngle * 0.5f;
		if (num6 < num7 && (wheelPos == WheelPos.REAR_RIGHT || wheelPos == WheelPos.REAR_LEFT))
		{
			num5 = (num6 - cardynamics.factor * num6 + num7 * cardynamics.factor) / num7;
		}
		deltaAngle1 = wheelRoadVeloLat - num4 * tanSlipAngle;
		deltaAngle1 /= b_;
		tanSlipAngle += deltaAngle1 * Time.deltaTime;
		slipAngle = (0f - PseudoAtan(tanSlipAngle)) * 57.29578f / num5;
		slipAngle = Mathf.Clamp(slipAngle, -45f, 45f);
		longitudinalSlip = slipRatio / idealSlipRatio;
		lateralSlip = slipAngle / idealSlipAngle;
		m_gripMaterial = (gripMaterial + gripSlip + gripVelo) * gripPressure * num;
		float num8 = Mathf.Max(Mathf.Sqrt(longitudinalSlip * longitudinalSlip + lateralSlip * lateralSlip), 0.0001f);
		Fx = longitudinalSlip / num8 * CalcLongitudinalForce(Fz, num8 * idealSlipRatio);
		Fy = lateralSlip / num8 * CalcLateralForce(Fz, num8 * idealSlipAngle, inclination);
		Mz = 0f;
		if (cardynamics.enableForceFeedback && maxSteeringAngle != 0f)
		{
			Mz = CalcAligningForce(Fz, slipAngle, inclination) - Fy * radiusLoaded * Mathf.Sin(caster);
		}
		if (float.IsInfinity(Fx) || float.IsNaN(Fx))
		{
			Fx = 0f;
		}
		if (float.IsInfinity(Fy) || float.IsNaN(Fy))
		{
			Fy = 0f;
		}
		return new Vector3(Fx, Fy, Mz);
	}

	private void LookupIdealSlipRatioIdealSlipAngle(float load)
	{
		int length = slipRatio_hat.GetLength(0);
		float num = 0.5f;
		if (load < num)
		{
			idealSlipRatio = slipRatio_hat[0];
			idealSlipAngle = slipAngle_hat[0];
			return;
		}
		if (load >= num * (float)length)
		{
			idealSlipRatio = slipRatio_hat[length - 1];
			idealSlipAngle = slipAngle_hat[length - 1];
			return;
		}
		int num2 = (int)(load / num);
		num2--;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 >= slipRatio_hat.GetLength(0))
		{
			num2 = slipRatio_hat.GetLength(0) - 1;
		}
		float num3 = (load - num * (float)(num2 + 1)) / num;
		idealSlipRatio = slipRatio_hat[num2] * (1f - num3) + slipRatio_hat[num2 + 1] * num3;
		idealSlipAngle = slipAngle_hat[num2] * (1f - num3) + slipAngle_hat[num2 + 1] * num3;
	}

	public void CalculateIdealSlipRatioIdealSlipAngle(int tablesize)
	{
		float num = 0.5f;
		Array.Resize(ref slipRatio_hat, tablesize);
		Array.Resize(ref slipAngle_hat, tablesize);
		for (int i = 0; i < tablesize; i++)
		{
			FindIdealSlipRatioIdealSlipAngle((float)(i + 1) * num, i, 400);
		}
	}

	private void FindIdealSlipRatioIdealSlipAngle(float load, int i, int iterations)
	{
		float num = 0f;
		float num2 = 0f;
		for (float num3 = -2f; num3 < 2f; num3 += 4f / (float)iterations)
		{
			num2 = CalcLongitudinalForce(load, num3);
			if (num2 > num)
			{
				slipRatio_hat[i] = num3;
				num = num2;
			}
		}
		num = 0f;
		for (float num3 = -20f; num3 < 20f; num3 += 40f / (float)iterations)
		{
			num2 = CalcLateralForce(load, num3, 0f);
			if (num2 > num)
			{
				slipAngle_hat[i] = num3;
				num = num2;
			}
		}
	}

	private void Awake()
	{
		m_gripMaterial = gripMaterial;
		myTransform = base.transform;
		trs = myTransform.parent;
		while (trs != null && trs.GetComponent<Rigidbody>() == null)
		{
			trs = trs.parent;
		}
		if (trs != null)
		{
			body = trs.GetComponent<Rigidbody>();
		}
		trs = myTransform.parent;
		while (trs.GetComponent<CarDynamics>() == null)
		{
			trs = trs.parent;
		}
		cardynamics = trs.GetComponent<CarDynamics>();
		drivetrain = trs.GetComponent<Drivetrain>();
		axles = trs.GetComponent<Axles>();
	}

	private void Start()
	{
		localScale = 1f / (trs.localScale.y * myTransform.localScale.y);
		layerMask = (1 << trs.gameObject.layer) | (1 << myTransform.gameObject.layer);
		layerMask = ~layerMask;
		radiusLoaded = radius;
		if (rimRadius == 0f)
		{
			rimRadius = radius * 0.735f;
		}
		sidewallHeight = radius - rimRadius;
		if (mass < 50f * cardynamics.invFixedTimeStepScalar)
		{
			mass = 50f * cardynamics.invFixedTimeStepScalar;
		}
		if (rotationalInertia == 0f || rotationalInertia < mass / 2f * radius * radius)
		{
			rotationalInertia = mass / 2f * radius * radius;
		}
		originalMass = mass;
		if (model == null)
		{
			model = new GameObject("temp_model");
			model.transform.parent = base.transform;
			model.transform.localPosition = new Vector3(0f, 0f, 0f);
			model.transform.localRotation = Quaternion.identity;
			Debug.LogWarning(string.Concat("UnityCar: wheel model in ", wheelPos, " is missing. Using empty object (", trs.name, ")"));
		}
		modelTransform = model.transform;
		if (caliperModel != null)
		{
			caliperModelTransform = caliperModel.transform;
		}
		skidmarks = cardynamics.skidmarks;
		if ((bool)skidmarks)
		{
			skidSmoke = skidmarks.GetComponentInChildren(typeof(ParticleEmitter)) as ParticleEmitter;
		}
		camber *= 1f;
		SetTireStiffness();
	}

	public void SetTireStiffness()
	{
		sidewallHeight = radius - rimRadius;
		verticalTireStiffness = radius / rimRadius * 0.25f * width * pressure * 1000f / sidewallHeight;
		radialDampingRatio = 0.6f;
		criticalDamping = 2f * Mathf.Sqrt(mass * verticalTireStiffness);
		tireDampingRate = criticalDamping * radialDampingRatio;
		if (pressure == 0f)
		{
			lateralTireStiffness = 0f;
		}
		else
		{
			lateralTireStiffness = (pressure + 0.24f * optimalPressure) * (1f - sidewallHeight / width) * (radius / width) * 1000f;
		}
		if (lateralTireStiffness != 0f && lateralTireStiffness < 20000f)
		{
			lateralTireStiffness = 20000f;
		}
		longitudinalTireStiffness = 2f * lateralTireStiffness;
	}

	private Vector3 RoadForce(float normalForce, Vector3 groundNormal)
	{
		if (cardynamics.physicMaterials.Length > 0 && !isDirty)
		{
			int num = 0;
			physicMaterial = hitDown.collider.sharedMaterial;
			if (physicMaterial != null)
			{
				for (i = 0; i < cardynamics.physicMaterials.Length; i++)
				{
					if (cardynamics.physicMaterials[i].physicMaterial == physicMaterial)
					{
						num = i;
						break;
					}
				}
				gripMaterial = cardynamics.physicMaterials[num].grip;
				rollingFrictionCoefficient = cardynamics.physicMaterials[num].rollingFriction;
				staticFrictionCoefficient = cardynamics.physicMaterials[num].staticFriction;
				isSkidSmoke = cardynamics.physicMaterials[num].isSkidSmoke;
				isSkidMark = cardynamics.physicMaterials[num].isSkidMark;
				isDirty = cardynamics.physicMaterials[num].isDirty;
				surfaceType = cardynamics.physicMaterials[num].surfaceType;
			}
		}
		float y = maxSteeringAngle * steering;
		localRotation = Quaternion.Euler(0f, y, 0f);
		right = myTransform.TransformDirection(localRotation * Vector3.right);
		inclination_sin = Vector3.Dot(right, groundNormal);
		rightNormal = right - groundNormal * inclination_sin;
		forwardNormal = Vector3.Cross(rightNormal, groundNormal);
		inclination_rad = Mathf.Asin(inclination_sin);
		inclination = 0f - (inclination_rad * 57.29578f + camber);
		inclination = Mathf.Clamp(inclination, -15f, 15f);
		wheelRoadVelo = Vector3.Dot(wheelVelo, forwardNormal);
		wheelRoadVeloLat = Vector3.Dot(wheelVelo, rightNormal);
		Vector3 vector = CalcForces(normalForce);
		longForce = forwardNormal * vector.x;
		latForce = rightNormal * vector.y;
		totalForce = longForce + latForce;
		roadTorqueImpulse = vector.x * radiusLoaded * Time.deltaTime;
		wheelImpulse = 0f;
		if (drivetrain != null && drivetrain.clutch != null && drivetrain.ratio != 0f && drivetrain.clutch.GetClutchPosition() != 0f)
		{
			wheelImpulse = CalcWheelImpulse(totalFrictionTorqueImpulse, roadTorqueImpulse, totalRotationalInertia, angularVelocity);
		}
		return totalForce;
	}

	private float CalcWheelImpulse(float totalFrictionTorqueImpulse, float roadTorqueImpulse, float totalRotationalInertia, float angularVelocity)
	{
		float num = Mathf.Clamp(angularVelocity * totalRotationalInertia, 0f - totalFrictionTorqueImpulse, totalFrictionTorqueImpulse);
		return 0f - (num + roadTorqueImpulse);
	}

	private void CalcWheelMovement()
	{
		if (model != null)
		{
			rotation += angularVelocity * Time.deltaTime;
			if (float.IsInfinity(rotation) || float.IsNaN(rotation))
			{
				rotation = 0f;
			}
			modelTransform.localPosition = Vector3.up * (compression - suspensionTravel) * localScale;
			modelTransform.localRotation = Quaternion.Euler(0f, maxSteeringAngle * steering, camber + caster * steering) * Quaternion.AngleAxis(57.29578f * rotation, Vector3.right);
			if (caliperModel != null)
			{
				caliperModelTransform.localPosition = modelTransform.localPosition;
				caliperModelTransform.localRotation = Quaternion.Euler(0f, maxSteeringAngle * steering, camber + caster * steering);
			}
		}
	}

	private void Update()
	{
		CalcWheelMovement();
		if ((bool)skidSmoke)
		{
			CalcSkidSmoke();
		}
		Debug.DrawRay(hitDown.point, up * 100f, Color.red);
		Debug.DrawRay(hitDown.point, groundNormal * 100f, Color.green);
		Debug.DrawRay(hitDown.point, longForce / 1000f, Color.red);
		Debug.DrawRay(hitDown.point, latForce / 1000f, Color.blue);
	}

	private void FixedUpdate()
	{
		velo = cardynamics.velo * 3.6f;
		pos = myTransform.position;
		up = myTransform.up;
		modelPosition = modelTransform.position;
		wheelVelo = body.GetPointVelocity(pos);
		dampAbsRoadVelo = cardynamics.dampAbsRoadVelo;
		oldNormalVelocity = normalVelocity;
		rollingResistanceTorque = normalForce * rollingFrictionCoefficient * radiusLoaded;
		totalRotationalInertia = rotationalInertia + drivetrainInertia;
		totalFrictionTorque = brakeFrictionTorque * 2f * brake + handbrakeFrictionTorque * 2f * handbrake + rollingResistanceTorque + wheelFrictionTorque;
		totalFrictionTorqueImpulse = totalFrictionTorque * Time.deltaTime;
		frictionAngularDelta = totalFrictionTorqueImpulse / totalRotationalInertia;
		onGroundDown = Physics.Raycast(pos, -up, out hitDown, suspensionTravel + radiusLoaded, layerMask);
		if (onGroundDown)
		{
			cos = Vector3.Dot(hitDown.normal, up);
			groundNormal = hitDown.normal;
			float num = 0f;
			roadDistance = hitDown.distance + num;
			if (cos >= 0.99f)
			{
				normalVelocity = (0f - Vector3.Dot(wheelVelo, groundNormal)) * cos;
			}
			else
			{
				normalVelocity = 0f - Vector3.Dot(wheelVelo, up);
			}
			deltaVelocity = normalVelocity - oldNormalVelocity;
			accel = deltaVelocity / Time.deltaTime;
			nextVelocity = normalVelocity + accel * Time.deltaTime;
			gripPressure = 1f;
			pressureFactor = 1f;
			if (tirePuncture)
			{
				rimScraping = true;
				radiusLoaded = radius - sidewallHeight;
				gripPressure = 0.1f;
			}
			else
			{
				rimScraping = false;
				if (pressure != 0f && cardynamics.tirePressureEnabled)
				{
					pressureFactor = pressure / optimalPressure;
					if (pressureFactor < 0.5f)
					{
						pressureFactor = 0.5f;
					}
					gripPressure = Mathf.Sqrt(pressureFactor);
					if (gripPressure > 1f)
					{
						gripPressure = 1f;
					}
				}
			}
			if (pressure == 0f || tirePuncture || !cardynamics.tirePressureEnabled)
			{
				springLength = roadDistance - radiusLoaded;
			}
			compression = suspensionTravel - springLength;
			nextCompression = compression + nextVelocity * Time.deltaTime;
			overTravel = compression - suspensionTravel;
			compression = Mathf.Clamp(compression, 0f, suspensionTravel);
			if (pressure != 0f && cardynamics.tirePressureEnabled && !tirePuncture)
			{
				tireForce = TireForce();
			}
			else
			{
				tireDeflection = 0f;
				deflectionVelocity = 0f;
				tireForce = 0f;
				radiusLoaded = radius;
				verticalTireStiffness = (lateralTireStiffness = (longitudinalTireStiffness = 0f));
			}
			suspensionForce = SuspensionForce(compression);
			bumpStopForce = 0f;
			if (overTravel >= 0f || nextCompression >= suspensionTravel)
			{
				bumpStopForce = BumpStopForce(overTravel) - suspensionForce;
				if (bumpStopForce < 0f)
				{
					bumpStopForce = 0f;
				}
				body.AddForceAtPosition(bumpStopForce * up, pos);
			}
			normalForce = suspensionForce + bumpStopForce;
			if (suspensionLineRenderer != null)
			{
				if (showForces)
				{
					suspensionLineRenderer.enabled = true;
					suspensionLineRenderer.SetPosition(1, new Vector3(0f, 0.0005f * suspensionForce, 0f));
				}
				else
				{
					suspensionLineRenderer.enabled = false;
				}
			}
			roadForce = RoadForce(normalForce, groundNormal);
			oldAngularVelocity = angularVelocity;
			angularVelocity += (lockingTorqueImpulse - roadTorqueImpulse) / totalRotationalInertia;
			if (Mathf.Abs(angularVelocity) > frictionAngularDelta)
			{
				angularVelocity -= frictionAngularDelta * Mathf.Sign(angularVelocity);
			}
			else
			{
				angularVelocity = 0f;
			}
			float num2 = angularVelocity - oldAngularVelocity;
			float num3 = num2 * 0.85f * Mathf.Clamp01(cardynamics.invFixedTimeStepScalar);
			angularVelocity -= num3;
			lateralTireDeflection = 0f;
			if (pressure != 0f && cardynamics.tirePressureEnabled && lateralTireStiffness != 0f)
			{
				lateralTireDeflection = Fy / lateralTireStiffness;
				if (Mathf.Abs(lateralTireDeflection) <= 0.0001f)
				{
					lateralTireDeflection = 0f;
				}
				overTurningMoment = 0f;
				if (lateralTireDeflection != 0f)
				{
					overTurningMoment = (0f - normalForce) * lateralTireDeflection;
					body.AddTorque(myTransform.forward * overTurningMoment);
					Mz += Fx * lateralTireDeflection;
				}
			}
			body.AddForceAtPosition(roadForce + suspensionForce * groundNormal, modelPosition);
			float sqrMagnitude = body.velocity.sqrMagnitude;
			if (sqrMagnitude <= 4f)
			{
				float num4 = CalculateFractionalMass() * (0f - Physics.gravity.y);
				Vector3 vector = Vector3.Cross(myTransform.forward, groundNormal);
				Vector3 vector2 = rightNormal;
				if (sqrMagnitude < 1f && (brake != 0f || handbrake != 0f))
				{
					vector2 = right * (1f - Mathf.Max(brake, handbrake)) - vector * Mathf.Max(brake, handbrake);
				}
				float num5 = Vector3.Dot(-vector2, Vector3.up);
				float num6 = num4 * num5 * cos;
				float num7 = staticFrictionCoefficient * m_gripMaterial * sidewaysGripFactor * num4;
				float num8 = num6;
				if (num7 < Mathf.Abs(num6))
				{
					num8 = num7 * Mathf.Sign(num6);
				}
				body.AddForceAtPosition(num8 * -vector2, modelPosition);
				Debug.DrawRay(modelPosition, num8 * -vector2 / 1000f, Color.white);
				if (sqrMagnitude < 1f && (brake != 0f || handbrake != 0f))
				{
					Vector3 vector3 = Vector3.Cross(myTransform.right, groundNormal);
					float num9 = Vector3.Dot(vector3, Vector3.up);
					float num10 = 1f;
					float num11 = 1f;
					if (wheelPos == WheelPos.FRONT_LEFT || wheelPos == WheelPos.FRONT_RIGHT)
					{
						if (cardynamics.frontRearBrakeBalance >= 0.9f || cardynamics.frontRearBrakeBalance <= 0.1f)
						{
							num10 = cardynamics.frontRearBrakeBalance * 2f * brake;
						}
						num11 = cardynamics.frontRearHandBrakeBalance * 2f * handbrake;
					}
					else
					{
						if (cardynamics.frontRearBrakeBalance >= 0.9f || cardynamics.frontRearBrakeBalance <= 0.1f)
						{
							num10 = (1f - cardynamics.frontRearBrakeBalance) * 2f * brake;
						}
						num11 = (1f - cardynamics.frontRearHandBrakeBalance) * 2f * handbrake;
					}
					if (num10 < 1f)
					{
						num10 = 1f;
					}
					if (num11 < 1f)
					{
						num11 = 1f;
					}
					float num12 = num4 * num9 * m_gripMaterial * forwardGripFactor * Mathf.Max(num10, num11);
					float num13 = totalFrictionTorque;
					float num14 = num12;
					if (num13 < Mathf.Abs(num12))
					{
						num14 = num13 * Mathf.Sign(num12);
					}
					body.AddForceAtPosition(num14 * vector3 * Mathf.Max(brake, handbrake), modelPosition);
					Debug.DrawRay(modelPosition, num14 * vector3 * Mathf.Max(brake, handbrake) / 1000f, Color.yellow);
				}
			}
			longitunalSlipVelo = Mathf.Abs(wheelTireVelo - wheelRoadVelo);
			lateralSlipVelo = wheelRoadVeloLat;
			float num15 = longitunalSlipVelo * longitunalSlipVelo;
			float num16 = lateralSlipVelo * lateralSlipVelo;
			slipVelo = Mathf.Sqrt(num15 + num16);
			slipVeloSmoke = Mathf.Sqrt(num15 + Mathf.Abs(lateralSlipVelo) * 0.001f);
			if (skidmarks != null)
			{
				CalcSkidmarks();
			}
		}
		else
		{
			compression = 0f;
			normalForce = 0f;
			roadTorqueImpulse = 0f;
			wheelImpulse = 0f;
			wheelTireVelo = (wheelRoadVelo = (absRoadVelo = (wheelRoadVeloLat = 0f)));
			wheelVelo = Vector3.zero;
			relaxLong = (relaxLat = (tireDeflection = (lateralTireDeflection = 0f)));
			suspensionForce = 0f;
			roadDistance = 0f;
			radiusLoaded = radius;
			springLength = suspensionTravel;
			roadForce = Vector3.zero;
			Fx = (Fy = 0f);
			slipAngle = (slipRatio = 0f);
			idealSlipRatio = (idealSlipAngle = 0f);
			if (suspensionLineRenderer != null)
			{
				suspensionLineRenderer.enabled = false;
			}
			lastSkid = -1;
			slipRatio = 0f;
			slipVelo = 0f;
			longitudinalSlip = 0f;
			lateralSlip = 0f;
			slipVeloSmoke = 0f;
			rimScraping = false;
			angularVelocity += (lockingTorqueImpulse - roadTorqueImpulse) / totalRotationalInertia;
			if (Mathf.Abs(angularVelocity) > frictionAngularDelta)
			{
				angularVelocity -= frictionAngularDelta * Mathf.Sign(angularVelocity);
			}
			else
			{
				angularVelocity = 0f;
			}
		}
	}

	private float BumpStopForce(float overtravel)
	{
		float num = body.mass / (float)axlesNumber / (float)axleWheelsLength;
		float num2 = num * (0f - Physics.gravity.y) * 32f * Mathf.Clamp01(cardynamics.fixedTimeStepScalar * 2f) * (float)axleWheelsLength;
		float num3 = num2 * (overtravel + 0.01f);
		float num4 = 2f * Mathf.Sqrt(num * num2);
		float num5 = ((!(cos >= 0.8f)) ? (0f - Vector3.Dot(wheelVelo, up)) : ((0f - Vector3.Dot(wheelVelo, groundNormal)) * cos));
		float num6 = num4 * cardynamics.fixedTimeStepScalar * num5;
		return num3 + num6;
	}

	private float CalculateFractionalMass()
	{
		float num = cardynamics.frontRearWeightRepartition;
		if (num == 0f || num == 1f)
		{
			num = 0.5f;
		}
		float num2 = ((wheelPos != WheelPos.FRONT_LEFT && wheelPos != WheelPos.FRONT_RIGHT) ? ((1f - num) / (float)axles.rearAxle.wheels.Length) : (num / (float)axles.frontAxle.wheels.Length));
		return num2 * body.mass;
	}

	private void CalcSkidmarks()
	{
		if (slipVelo > slipVeloThreshold && isSkidMark)
		{
			slipSkidAmount = (slipVelo - slipVeloThreshold) / 15f;
			skidmarks.markWidth = width;
			lastSkid = skidmarks.AddSkidMark(hitDown.point, hitDown.normal, slipSkidAmount, lastSkid);
		}
		else
		{
			lastSkid = -1;
		}
	}

	private void CalcSkidSmoke()
	{
		if (slipVeloSmoke > slipVeloThreshold && isSkidSmoke)
		{
			slipSmokeAmount = (slipVeloSmoke - slipVeloThreshold) / 15f;
			Vector3 vector = myTransform.TransformDirection(skidSmoke.localVelocity) + skidSmoke.worldVelocity;
			skidSmoke.Emit(hitDown.point + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f)), vector + wheelVelo * 0.05f, UnityEngine.Random.Range(skidSmoke.minSize, skidSmoke.maxSize) * Mathf.Clamp(slipSmokeAmount * 0.1f, 0.5f, 1f), UnityEngine.Random.Range(skidSmoke.minEnergy, skidSmoke.maxEnergy), Color.white);
		}
	}

	private float SuspensionForce(float compression)
	{
		springForce = suspensionRate * compression;
		float num = springForce;
		normalVelocity -= deflectionVelocity;
		float transitionDamperVelo = cardynamics.transitionDamperVelo;
		float num2 = ((normalVelocity > 0f) ? ((!(normalVelocity < transitionDamperVelo)) ? ((normalVelocity - transitionDamperVelo) * bumpRate * fastBumpFactor + transitionDamperVelo * bumpRate) : (normalVelocity * bumpRate)) : ((!(normalVelocity > 0f - transitionDamperVelo)) ? ((normalVelocity + transitionDamperVelo) * reboundRate * fastReboundFactor - transitionDamperVelo * reboundRate) : (normalVelocity * reboundRate)));
		num += num2;
		num += antiRollBarForce;
		suspensionForce = num;
		if (suspensionForce < 0f)
		{
			suspensionForce = 0f;
		}
		float num3 = 0f;
		if (pressure != 0f && cardynamics.tirePressureEnabled && !tirePuncture)
		{
			float num4 = 150f * cardynamics.invFixedTimeStepScalar;
			float num5 = ((!(mass < num4)) ? mass : num4);
			float num6 = (suspensionForce - tireForce) / num5;
			deflectionVelocity += num6 * Time.deltaTime;
			springLength += (deflectionVelocity - normalVelocity) * Time.deltaTime;
			if (springLength <= 0f)
			{
				springLength = 0f;
				num3 = tireForce - suspensionForce + num2;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
			}
			else if (springLength > suspensionTravel)
			{
				springLength = suspensionTravel;
			}
			if (cardynamics.invFixedTimeStepScalar >= 1f)
			{
				deflectionVelocity = 0f;
			}
		}
		suspensionForce += num3;
		return suspensionForce;
	}

	private float TireForce()
	{
		radiusLoaded = roadDistance - springLength;
		radiusLoaded = Mathf.Clamp(radiusLoaded, radius - sidewallHeight, radius);
		tireDeflection = radius - radiusLoaded;
		tireDeflection = Mathf.Clamp(tireDeflection, 0f, sidewallHeight);
		float num = verticalTireStiffness * tireDeflection;
		float num2 = deflectionVelocity * tireDampingRate;
		num += num2;
		tireForce = num;
		if (tireForce < 0f)
		{
			tireForce = 0f;
		}
		return tireForce;
	}
}
