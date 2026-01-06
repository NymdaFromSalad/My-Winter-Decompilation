using System;
using UnityEngine;

[RequireComponent(typeof(Axles))]
[RequireComponent(typeof(Rigidbody))]
public class CarDynamics : MonoBehaviour
{
	public enum Controller
	{
		axis = 0,
		mouse = 1,
		mobile = 2,
		external = 3
	}

	public enum SurfaceType
	{
		track = 0,
		grass = 1,
		sand = 2,
		offroad = 3,
		oil = 4
	}

	public enum Tires
	{
		competition_front = 0,
		competition_rear = 1,
		supersport_front = 2,
		supersport_rear = 3,
		sport_front = 4,
		sport_rear = 5,
		touring_front = 6,
		touring_rear = 7,
		offroad_front = 8,
		offroad_rear = 9,
		truck_front = 10,
		truck_rear = 11,
		winter_front = 12,
		winter_rear = 13
	}

	private float frontnforce;

	private float backnforce;

	private float factor1;

	private float factor2;

	private float factor3;

	private float deltaFactor;

	[HideInInspector]
	public float factor;

	[HideInInspector]
	public float velo;

	public Controller controller;

	[HideInInspector]
	public CarController carController;

	private AxisCarController axisCarController;

	private MouseCarController mouseCarcontroller;

	private MobileCarController mobileCarController;

	private Drivetrain drivetrain;

	private BrakeLights brakeLights;

	private DashBoard dashBoard;

	private SteeringWheel steeringWheel;

	private SoundController soundController;

	private Axles axles;

	[HideInInspector]
	public float transitionDamperVelo = 0.3f;

	public Transform centerOfMass;

	private GameObject centerOfMassObject;

	private Vector3 deltaCenterOfMass;

	private Vector3 originalCenterOfMass;

	private Transform myTransform;

	public float dampAbsRoadVelo = 8f;

	public float inertiaFactor = 1f;

	public float frontRearWeightRepartition = 0.5f;

	public float frontRearBrakeBalance = 0.65f;

	public float frontRearHandBrakeBalance;

	public bool enableForceFeedback;

	[HideInInspector]
	public float forceFeedback;

	[HideInInspector]
	public bool tridimensionalTire;

	[HideInInspector]
	public bool tirePressureEnabled = true;

	[HideInInspector]
	public float airDensity = 1.2041f;

	public Skidmarks skidmarks;

	public MyPhysicMaterial[] physicMaterials = new MyPhysicMaterial[4];

	[HideInInspector]
	public float xlocalPosition;

	[HideInInspector]
	public float xlocalPosition_orig;

	[HideInInspector]
	public float zlocalPosition;

	[HideInInspector]
	public float zlocalPosition_orig;

	[HideInInspector]
	public float ylocalPosition;

	[HideInInspector]
	public float ylocalPosition_orig;

	private float normalForceR;

	private float normalForceF;

	private float antiRollBarForce;

	[HideInInspector]
	public float fixedTimeStepScalar;

	[HideInInspector]
	public float invFixedTimeStepScalar;

	private float invAllWheelsLength;

	private Rigidbody body;

	private bool tiresFound;

	private float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}

	public float GetCentrifugalAccel()
	{
		float num = 0f;
		if (axles != null)
		{
			Wheel[] allWheels = axles.allWheels;
			foreach (Wheel wheel in allWheels)
			{
				num += wheel.Fy;
			}
		}
		return num * invAllWheelsLength / GetComponent<Rigidbody>().mass;
	}

	public void FixPhysX()
	{
		body.collisionDetectionMode = CollisionDetectionMode.Continuous;
		body.collisionDetectionMode = CollisionDetectionMode.Discrete;
		body.centerOfMass -= deltaCenterOfMass;
	}

	private void Awake()
	{
		body = GetComponent<Rigidbody>();
		originalCenterOfMass = body.centerOfMass;
		myTransform = base.transform;
		drivetrain = GetComponent<Drivetrain>();
		axisCarController = GetComponent<AxisCarController>();
		mouseCarcontroller = GetComponent<MouseCarController>();
		mobileCarController = GetComponent<MobileCarController>();
		brakeLights = GetComponent<BrakeLights>();
		dashBoard = myTransform.GetComponentInChildren<DashBoard>();
		steeringWheel = base.transform.GetComponentInChildren<SteeringWheel>();
		soundController = GetComponent<SoundController>();
		SetController(controller.ToString());
		axles = GetComponent<Axles>();
		invAllWheelsLength = 1f / (float)axles.allWheels.Length;
		fixedTimeStepScalar = 0.02f / Time.fixedDeltaTime;
		invFixedTimeStepScalar = 1f / fixedTimeStepScalar;
	}

	private void Start()
	{
		body.inertiaTensor *= inertiaFactor;
		axles.frontAxle.oldCamber = axles.frontAxle.camber;
		axles.rearAxle.oldCamber = axles.rearAxle.camber;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			axle.oldCamber = axle.camber;
		}
		SetCenterOfMass();
		SetWheelsParams();
		SetBrakes();
		SetTiresType();
		xlocalPosition_orig = xlocalPosition;
		ylocalPosition_orig = ylocalPosition;
		zlocalPosition_orig = zlocalPosition;
	}

	public Vector3 BoundingSize(Collider[] colliders)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (colliders.Length != 0)
		{
			foreach (Collider collider in colliders)
			{
				if (collider.gameObject.layer != LayerMask.NameToLayer("Wheel") && collider.transform.GetComponent<FuelTank>() == null)
				{
					num += collider.bounds.size.x;
					num2 += collider.bounds.size.y;
					num3 += collider.bounds.size.z;
				}
			}
		}
		return new Vector3(num, num2, num3);
	}

	public void SetCenterOfMass(Vector3? COGPosition = null)
	{
		if (centerOfMass == null)
		{
			centerOfMassObject = new GameObject("COG");
			centerOfMassObject.transform.parent = base.transform;
			centerOfMass = centerOfMassObject.transform;
			if (COGPosition.HasValue)
			{
				Rigidbody component = GetComponent<Rigidbody>();
				Vector3 value = COGPosition.Value;
				centerOfMass.localPosition = value;
				component.centerOfMass = value;
			}
			else
			{
				centerOfMass.localPosition = GetComponent<Rigidbody>().centerOfMass;
			}
		}
		else
		{
			if (COGPosition.HasValue)
			{
				Rigidbody component2 = GetComponent<Rigidbody>();
				Vector3 value = COGPosition.Value;
				centerOfMass.localPosition = value;
				component2.centerOfMass = value;
			}
			else
			{
				GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
			}
			deltaCenterOfMass = originalCenterOfMass - centerOfMass.localPosition;
		}
		xlocalPosition = centerOfMass.localPosition.x;
		ylocalPosition = centerOfMass.localPosition.y;
		zlocalPosition = centerOfMass.localPosition.z;
	}

	public void SetBrakes()
	{
		frontRearBrakeBalance = Mathf.Clamp01(frontRearBrakeBalance);
		frontRearHandBrakeBalance = Mathf.Clamp01(frontRearHandBrakeBalance);
		Wheel[] wheels = axles.frontAxle.wheels;
		foreach (Wheel wheel in wheels)
		{
			wheel.brakeFrictionTorque = axles.frontAxle.brakeFrictionTorque * Mathf.Min(frontRearBrakeBalance, 0.5f) * 2f;
			wheel.handbrakeFrictionTorque = axles.frontAxle.handbrakeFrictionTorque * Mathf.Min(frontRearHandBrakeBalance, 0.5f) * 2f;
		}
		Wheel[] wheels2 = axles.rearAxle.wheels;
		foreach (Wheel wheel2 in wheels2)
		{
			wheel2.brakeFrictionTorque = axles.rearAxle.brakeFrictionTorque * Mathf.Min(1f - frontRearBrakeBalance, 0.5f) * 2f;
			wheel2.handbrakeFrictionTorque = axles.rearAxle.handbrakeFrictionTorque * Mathf.Min(1f - frontRearHandBrakeBalance, 0.5f) * 2f;
		}
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			Wheel[] wheels3 = axle.wheels;
			foreach (Wheel wheel3 in wheels3)
			{
				wheel3.brakeFrictionTorque = axle.brakeFrictionTorque;
				wheel3.handbrakeFrictionTorque = axle.handbrakeFrictionTorque;
			}
		}
	}

	public void SetWheelsParams()
	{
		Wheel[] wheels = axles.frontAxle.wheels;
		foreach (Wheel wheel in wheels)
		{
			if (wheel != null)
			{
				wheel.forwardGripFactor = axles.frontAxle.forwardGripFactor;
				wheel.sidewaysGripFactor = axles.frontAxle.sidewaysGripFactor;
				wheel.suspensionTravel = axles.frontAxle.suspensionTravel;
				wheel.suspensionRate = axles.frontAxle.suspensionRate;
				wheel.bumpRate = axles.frontAxle.bumpRate;
				wheel.reboundRate = axles.frontAxle.reboundRate;
				wheel.fastBumpFactor = axles.frontAxle.fastBumpFactor;
				wheel.fastReboundFactor = axles.frontAxle.fastReboundFactor;
				wheel.pressure = axles.frontAxle.tiresPressure;
				wheel.optimalPressure = axles.frontAxle.optimalTiresPressure;
				wheel.SetTireStiffness();
				wheel.maxSteeringAngle = axles.frontAxle.maxSteeringAngle;
				wheel.axleWheelsLength = axles.frontAxle.wheels.Length;
				wheel.axlesNumber = axles.otherAxles.Length + 2;
				wheel.caster = axles.frontAxle.caster;
				if (wheel.wheelPos == WheelPos.FRONT_RIGHT)
				{
					wheel.deltaCamber = 0f - axles.frontAxle.deltaCamber;
					wheel.camber = 0f - axles.frontAxle.camber;
				}
				else
				{
					wheel.deltaCamber = axles.frontAxle.deltaCamber;
					wheel.camber = axles.frontAxle.camber;
				}
			}
		}
		Wheel[] wheels2 = axles.rearAxle.wheels;
		foreach (Wheel wheel2 in wheels2)
		{
			if (wheel2 != null)
			{
				wheel2.forwardGripFactor = axles.rearAxle.forwardGripFactor;
				wheel2.sidewaysGripFactor = axles.rearAxle.sidewaysGripFactor;
				wheel2.suspensionTravel = axles.rearAxle.suspensionTravel;
				wheel2.suspensionRate = axles.rearAxle.suspensionRate;
				wheel2.bumpRate = axles.rearAxle.bumpRate;
				wheel2.reboundRate = axles.rearAxle.reboundRate;
				wheel2.fastBumpFactor = axles.rearAxle.fastBumpFactor;
				wheel2.fastReboundFactor = axles.rearAxle.fastReboundFactor;
				wheel2.pressure = axles.rearAxle.tiresPressure;
				wheel2.optimalPressure = axles.rearAxle.optimalTiresPressure;
				wheel2.SetTireStiffness();
				wheel2.maxSteeringAngle = 0f - axles.rearAxle.maxSteeringAngle;
				wheel2.axleWheelsLength = axles.rearAxle.wheels.Length;
				wheel2.axlesNumber = axles.otherAxles.Length + 2;
				wheel2.caster = axles.rearAxle.caster;
				if (wheel2.wheelPos == WheelPos.REAR_RIGHT)
				{
					wheel2.deltaCamber = 0f - axles.rearAxle.deltaCamber;
					wheel2.camber = 0f - axles.rearAxle.camber;
				}
				else
				{
					wheel2.deltaCamber = axles.rearAxle.deltaCamber;
					wheel2.camber = axles.rearAxle.camber;
				}
			}
		}
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			Wheel[] wheels3 = axle.wheels;
			foreach (Wheel wheel3 in wheels3)
			{
				if (wheel3 != null)
				{
					wheel3.forwardGripFactor = axle.forwardGripFactor;
					wheel3.sidewaysGripFactor = axle.sidewaysGripFactor;
					wheel3.suspensionTravel = axle.suspensionTravel;
					wheel3.suspensionRate = axle.suspensionRate;
					wheel3.bumpRate = axle.bumpRate;
					wheel3.reboundRate = axle.reboundRate;
					wheel3.fastBumpFactor = axle.fastBumpFactor;
					wheel3.fastReboundFactor = axle.fastReboundFactor;
					wheel3.pressure = axle.tiresPressure;
					wheel3.optimalPressure = axle.optimalTiresPressure;
					wheel3.SetTireStiffness();
					wheel3.maxSteeringAngle = axle.maxSteeringAngle;
					wheel3.axleWheelsLength = axle.wheels.Length;
					wheel3.axlesNumber = axles.otherAxles.Length + 2;
					wheel3.caster = axle.caster;
					if (wheel3 == axle.rightWheel)
					{
						wheel3.deltaCamber = 0f - axle.deltaCamber;
						wheel3.camber = 0f - axle.camber;
					}
					else
					{
						wheel3.deltaCamber = axle.deltaCamber;
						wheel3.camber = axle.camber;
					}
				}
			}
		}
	}

	public void SetTiresType()
	{
		Wheel[] wheels = axles.frontAxle.wheels;
		foreach (Wheel w in wheels)
		{
			LoadTiresData(w, axles.frontAxle.tires.ToString());
		}
		if (!tiresFound)
		{
			Debug.LogWarning("UnityCar: Tires \"" + axles.frontAxle.tires.ToString() + "\" not found. Using standard tires data on front axle (" + myTransform.name + ")");
		}
		else
		{
			Debug.Log("UnityCar: Tires \"" + axles.frontAxle.tires.ToString() + "\" loaded on front axle (" + myTransform.name + ")");
		}
		Wheel[] wheels2 = axles.rearAxle.wheels;
		foreach (Wheel w2 in wheels2)
		{
			LoadTiresData(w2, axles.rearAxle.tires.ToString());
		}
		if (!tiresFound)
		{
			Debug.LogWarning("UnityCar: Tires \"" + axles.rearAxle.tires.ToString() + "\" not found. Using standard tires data on rear axle (" + myTransform.name + ")");
		}
		else
		{
			Debug.Log("UnityCar: Tires \"" + axles.rearAxle.tires.ToString() + "\" loaded on rear axle (" + myTransform.name + ")");
		}
		int num = 1;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			Wheel[] wheels3 = axle.wheels;
			foreach (Wheel w3 in wheels3)
			{
				LoadTiresData(w3, axle.tires.ToString());
			}
			if (!tiresFound)
			{
				Debug.LogWarning("UnityCar: Tires  \"" + axle.tires.ToString() + "\" not found. Using standard tires data on other axle" + num + " (" + myTransform.name + ")");
			}
			else
			{
				Debug.Log("UnityCar: Tires \"" + axle.tires.ToString() + "\" loaded on other axle" + num + " (" + myTransform.name + ")");
			}
			num++;
		}
	}

	public void LoadTiresData(Wheel w, string tires)
	{
		tiresFound = true;
		switch (tires)
		{
		case "competition_front":
			Array.Copy(TireParameters.aCompetitionFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bCompetitionFront, w.b, w.b.Length);
			if (TireParameters.cCompetitionFront.Length != 0)
			{
				Array.Copy(TireParameters.cCompetitionFront, w.c, w.c.Length);
			}
			break;
		case "competition_rear":
			Array.Copy(TireParameters.aCompetitionRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bCompetitionRear, w.b, w.b.Length);
			if (TireParameters.cCompetitionRear.Length != 0)
			{
				Array.Copy(TireParameters.cCompetitionRear, w.c, w.c.Length);
			}
			break;
		case "supersport_front":
			Array.Copy(TireParameters.aSuperSportFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bSuperSportFront, w.b, w.b.Length);
			if (TireParameters.cSuperSportFront.Length != 0)
			{
				Array.Copy(TireParameters.cSuperSportFront, w.c, w.c.Length);
			}
			break;
		case "supersport_rear":
			Array.Copy(TireParameters.aSuperSportRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bSuperSportRear, w.b, w.b.Length);
			if (TireParameters.cSuperSportRear.Length != 0)
			{
				Array.Copy(TireParameters.cSuperSportRear, w.c, w.c.Length);
			}
			break;
		case "sport_front":
			Array.Copy(TireParameters.aSportFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bSportFront, w.b, w.b.Length);
			if (TireParameters.cSportFront.Length != 0)
			{
				Array.Copy(TireParameters.cSportFront, w.c, w.c.Length);
			}
			break;
		case "sport_rear":
			Array.Copy(TireParameters.aSportRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bSportRear, w.b, w.b.Length);
			if (TireParameters.cSportRear.Length != 0)
			{
				Array.Copy(TireParameters.cSportRear, w.c, w.c.Length);
			}
			break;
		case "touring_front":
			Array.Copy(TireParameters.aTouringFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bTouringFront, w.b, w.b.Length);
			if (TireParameters.cTouringFront.Length != 0)
			{
				Array.Copy(TireParameters.cTouringFront, w.c, w.c.Length);
			}
			break;
		case "touring_rear":
			Array.Copy(TireParameters.aTouringRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bTouringRear, w.b, w.b.Length);
			if (TireParameters.cTouringRear.Length != 0)
			{
				Array.Copy(TireParameters.cTouringRear, w.c, w.c.Length);
			}
			break;
		case "offroad_front":
			Array.Copy(TireParameters.aOffRoadFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bOffRoadFront, w.b, w.b.Length);
			if (TireParameters.cOffRoadFront.Length != 0)
			{
				Array.Copy(TireParameters.cOffRoadFront, w.c, w.c.Length);
			}
			break;
		case "offroad_rear":
			Array.Copy(TireParameters.aOffRoadRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bOffRoadRear, w.b, w.b.Length);
			if (TireParameters.cOffRoadRear.Length != 0)
			{
				Array.Copy(TireParameters.cOffRoadRear, w.c, w.c.Length);
			}
			break;
		case "truck_front":
			Array.Copy(TireParameters.aTruckFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bTruckFront, w.b, w.b.Length);
			if (TireParameters.cTruckFront.Length != 0)
			{
				Array.Copy(TireParameters.cTruckFront, w.c, w.c.Length);
			}
			break;
		case "truck_rear":
			Array.Copy(TireParameters.aTruckRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bTruckRear, w.b, w.b.Length);
			if (TireParameters.cTruckRear.Length != 0)
			{
				Array.Copy(TireParameters.cTruckRear, w.c, w.c.Length);
			}
			break;
		case "winter_front":
			Array.Copy(TireParameters.aTruckFront, w.a, w.a.Length);
			Array.Copy(TireParameters.bTruckFront, w.b, w.b.Length);
			if (TireParameters.cTruckFront.Length != 0)
			{
				Array.Copy(TireParameters.cTruckFront, w.c, w.c.Length);
			}
			break;
		case "winter_rear":
			Array.Copy(TireParameters.aWinterRear, w.a, w.a.Length);
			Array.Copy(TireParameters.bWinterRear, w.b, w.b.Length);
			if (TireParameters.cWinterRear.Length != 0)
			{
				Array.Copy(TireParameters.cWinterRear, w.c, w.c.Length);
			}
			break;
		default:
			tiresFound = false;
			break;
		}
		w.CalculateIdealSlipRatioIdealSlipAngle(20);
	}

	public void SetController(string strcontroller)
	{
		axisCarController = GetComponent<AxisCarController>();
		mouseCarcontroller = GetComponent<MouseCarController>();
		mobileCarController = GetComponent<MobileCarController>();
		if (strcontroller == Controller.axis.ToString())
		{
			if (axisCarController == null)
			{
				axisCarController = base.transform.gameObject.AddComponent<AxisCarController>();
			}
			axisCarController.enabled = true;
			carController = axisCarController;
			if (drivetrain != null)
			{
				drivetrain.carController = axisCarController;
			}
			if (brakeLights != null)
			{
				brakeLights.carController = axisCarController;
			}
			if (dashBoard != null)
			{
				dashBoard.carController = axisCarController;
			}
			if (steeringWheel != null)
			{
				steeringWheel.carController = axisCarController;
			}
			if (soundController != null)
			{
				soundController.carController = axisCarController;
			}
			if (mouseCarcontroller != null)
			{
				mouseCarcontroller.enabled = false;
			}
			if (mobileCarController != null)
			{
				mobileCarController.enabled = false;
			}
			controller = Controller.axis;
		}
		else if (strcontroller == Controller.mouse.ToString())
		{
			if (mouseCarcontroller == null)
			{
				mouseCarcontroller = base.transform.gameObject.AddComponent<MouseCarController>();
			}
			mouseCarcontroller.enabled = true;
			mouseCarcontroller.smoothInput = false;
			carController = mouseCarcontroller;
			if (drivetrain != null)
			{
				drivetrain.carController = mouseCarcontroller;
			}
			if (brakeLights != null)
			{
				brakeLights.carController = mouseCarcontroller;
			}
			if (dashBoard != null)
			{
				dashBoard.carController = mouseCarcontroller;
			}
			if (steeringWheel != null)
			{
				steeringWheel.carController = mouseCarcontroller;
			}
			if (soundController != null)
			{
				soundController.carController = mouseCarcontroller;
			}
			if (axisCarController != null)
			{
				axisCarController.enabled = false;
			}
			if (mobileCarController != null)
			{
				mobileCarController.enabled = false;
			}
			controller = Controller.mouse;
		}
		else if (strcontroller == Controller.mobile.ToString())
		{
			if (mobileCarController == null)
			{
				mobileCarController = base.transform.gameObject.AddComponent<MobileCarController>();
			}
			mobileCarController.enabled = true;
			carController = mobileCarController;
			if (drivetrain != null)
			{
				drivetrain.carController = mobileCarController;
			}
			if (brakeLights != null)
			{
				brakeLights.carController = mobileCarController;
			}
			if (dashBoard != null)
			{
				dashBoard.carController = mobileCarController;
			}
			if (steeringWheel != null)
			{
				steeringWheel.carController = mobileCarController;
			}
			if (soundController != null)
			{
				soundController.carController = mobileCarController;
			}
			if (axisCarController != null)
			{
				axisCarController.enabled = false;
			}
			if (mouseCarcontroller != null)
			{
				mouseCarcontroller.enabled = false;
			}
			controller = Controller.mobile;
		}
		else if (strcontroller == Controller.external.ToString())
		{
			if (mouseCarcontroller != null)
			{
				mouseCarcontroller.throttle = (drivetrain.throttle = 0f);
				mouseCarcontroller.enabled = false;
			}
			if (axisCarController != null)
			{
				axisCarController.throttle = (drivetrain.throttle = 0f);
				axisCarController.enabled = false;
			}
			if (mobileCarController != null)
			{
				mobileCarController.throttle = (drivetrain.throttle = 0f);
				mobileCarController.enabled = false;
			}
			controller = Controller.external;
		}
	}

	private void FixedUpdate()
	{
		body.centerOfMass = centerOfMass.localPosition;
		fixedTimeStepScalar = 0.02f / Time.fixedDeltaTime;
		invFixedTimeStepScalar = 1f / fixedTimeStepScalar;
		velo = Mathf.Abs(myTransform.InverseTransformDirection(body.velocity).z);
		if (Application.isEditor)
		{
			SetBrakes();
			SetController(controller.ToString());
			axles.frontAxle.deltaCamber = axles.frontAxle.camber - axles.frontAxle.oldCamber;
			if (axles.frontAxle.deltaCamber != 0f)
			{
				axles.frontAxle.oldCamber = axles.frontAxle.camber;
				SetWheelsParams();
			}
			axles.rearAxle.deltaCamber = axles.rearAxle.camber - axles.rearAxle.oldCamber;
			if (axles.frontAxle.deltaCamber != 0f)
			{
				axles.rearAxle.oldCamber = axles.rearAxle.camber;
				SetWheelsParams();
			}
			Axle[] otherAxles = axles.otherAxles;
			foreach (Axle axle in otherAxles)
			{
				axle.deltaCamber = axle.camber - axle.oldCamber;
				if (axle.deltaCamber != 0f)
				{
					axle.oldCamber = axle.camber;
					SetWheelsParams();
				}
			}
		}
		if (axles.frontAxle.leftWheel != null && axles.frontAxle.rightWheel != null)
		{
			antiRollBarForce = (axles.frontAxle.leftWheel.compression - axles.frontAxle.rightWheel.compression) * axles.frontAxle.antiRollBarRate;
			axles.frontAxle.leftWheel.antiRollBarForce = antiRollBarForce;
			axles.frontAxle.rightWheel.antiRollBarForce = 0f - antiRollBarForce;
		}
		if (axles.rearAxle.leftWheel != null && axles.rearAxle.rightWheel != null)
		{
			antiRollBarForce = (axles.rearAxle.leftWheel.compression - axles.rearAxle.rightWheel.compression) * axles.rearAxle.antiRollBarRate;
			axles.rearAxle.leftWheel.antiRollBarForce = antiRollBarForce;
			axles.rearAxle.rightWheel.antiRollBarForce = 0f - antiRollBarForce;
		}
		Axle[] otherAxles2 = axles.otherAxles;
		foreach (Axle axle2 in otherAxles2)
		{
			if (axle2.leftWheel != null && axle2.rightWheel != null)
			{
				antiRollBarForce = (axle2.leftWheel.compression - axle2.rightWheel.compression) * axle2.antiRollBarRate;
				axle2.leftWheel.antiRollBarForce = antiRollBarForce;
				axle2.rightWheel.antiRollBarForce = 0f - antiRollBarForce;
			}
		}
		normalForceF = (normalForceR = 0f);
		Wheel[] wheels = axles.frontAxle.wheels;
		foreach (Wheel wheel in wheels)
		{
			normalForceF += wheel.normalForce;
		}
		if (axles.frontAxle.wheels.Length != 0)
		{
			normalForceF /= axles.frontAxle.wheels.Length;
		}
		Wheel[] wheels2 = axles.rearAxle.wheels;
		foreach (Wheel wheel2 in wheels2)
		{
			normalForceR += wheel2.normalForce;
		}
		if (axles.rearAxle.wheels.Length != 0)
		{
			normalForceR /= axles.rearAxle.wheels.Length;
		}
		if (normalForceF + normalForceR != 0f)
		{
			frontRearWeightRepartition = RoundTo(normalForceF / (normalForceF + normalForceR), 2);
		}
		float num = body.mass * (0f - Physics.gravity.y) * 0.25f;
		if (zlocalPosition > 0f)
		{
			frontnforce = num * (1f + zlocalPosition);
			backnforce = num * (1f - zlocalPosition);
		}
		else
		{
			frontnforce = num * (1f + zlocalPosition);
			backnforce = num * (1f - zlocalPosition);
		}
		if (normalForceR != 0f)
		{
			factor1 = normalForceF / normalForceR;
		}
		if (backnforce != 0f)
		{
			factor2 = frontnforce / backnforce;
		}
		factor3 = factor2 + 0.25f * (1f - factor2);
		if (factor1 < factor3)
		{
			deltaFactor = factor3 - factor1;
		}
		else
		{
			deltaFactor = factor1 - factor3;
		}
		factor = factor3 - deltaFactor;
		factor = Mathf.Clamp(factor, 0.25f, 1f);
		forceFeedback = 0f;
		if (!enableForceFeedback)
		{
			return;
		}
		float num2 = 0f;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel3 in allWheels)
		{
			if (wheel3.maxSteeringAngle != 0f)
			{
				forceFeedback += wheel3.Mz;
				num2 += 1f;
			}
		}
		forceFeedback /= num2;
	}

	public float LateralSlipVeloRearWheels()
	{
		float num = 0f;
		Wheel[] wheels = axles.rearAxle.wheels;
		foreach (Wheel wheel in wheels)
		{
			num += wheel.lateralSlipVelo;
		}
		return num / (float)axles.rearAxle.wheels.Length;
	}

	public float SlipVelo()
	{
		float num = 0f;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			num += wheel.slipVelo;
		}
		return num * invAllWheelsLength;
	}

	public bool AllWheelsOnGround()
	{
		bool flag = false;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			flag = wheel.onGroundDown;
			if (flag)
			{
				break;
			}
		}
		return flag;
	}
}
