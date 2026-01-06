using UnityEngine;

[ExecuteInEditMode]
public static class UnityCar
{
	private static Drivetrain drivetrain;

	private static AxisCarController axisCarController;

	private static CarDynamics carDynamics;

	private static SoundController soundController;

	private static Rigidbody mrigidbody;

	private static Axles axles;

	private static Wheel wheel;

	private static GameObject wheelFL;

	private static GameObject wheelFR;

	private static GameObject wheelRL;

	private static GameObject wheelRR;

	private static GameObject model;

	private static BoxCollider boxColliderDown;

	private static BoxCollider boxColliderUp;

	private static GameObject bodyDown;

	private static GameObject bodyUp;

	private static GameObject mcollider;

	private static GameObject dashBoard;

	private static GameObject centerOfMassObject;

	public static GameObject CreateNewCar()
	{
		GameObject gameObject = new GameObject("UnityCar");
		if (gameObject.GetComponent<Rigidbody>() == null)
		{
			mrigidbody = gameObject.AddComponent<Rigidbody>();
			mrigidbody.mass = 1000f;
			mrigidbody.angularDrag = 0f;
			mrigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}
		else
		{
			mrigidbody = gameObject.GetComponent<Rigidbody>();
		}
		if (gameObject.GetComponent<CarDynamics>() == null)
		{
			carDynamics = gameObject.AddComponent<CarDynamics>();
		}
		else
		{
			carDynamics = gameObject.GetComponent<CarDynamics>();
		}
		centerOfMassObject = new GameObject("COG");
		centerOfMassObject.transform.parent = gameObject.transform;
		centerOfMassObject.transform.localPosition = Vector3.zero;
		centerOfMassObject.transform.localRotation = Quaternion.identity;
		carDynamics.centerOfMass = centerOfMassObject.transform;
		if (gameObject.GetComponent<Drivetrain>() == null)
		{
			drivetrain = gameObject.AddComponent<Drivetrain>();
		}
		else
		{
			drivetrain = gameObject.GetComponent<Drivetrain>();
		}
		if (gameObject.GetComponent<AxisCarController>() == null)
		{
			axisCarController = gameObject.AddComponent<AxisCarController>();
		}
		else
		{
			axisCarController = gameObject.GetComponent<AxisCarController>();
		}
		if (gameObject.GetComponent<Axles>() == null)
		{
			axles = gameObject.AddComponent<Axles>();
		}
		else
		{
			axles = gameObject.GetComponent<Axles>();
		}
		gameObject.layer = 1;
		if (gameObject.transform.Find("Body") == null)
		{
			bodyDown = GameObject.CreatePrimitive(PrimitiveType.Cube);
			bodyDown.name = "Body";
			Object.DestroyImmediate(bodyDown.GetComponent<Collider>());
			bodyDown.transform.parent = gameObject.transform;
			bodyDown.transform.localPosition = Vector3.zero;
			bodyDown.transform.localRotation = Quaternion.identity;
			bodyDown.transform.localScale = new Vector3(1.5f, 0.5f, 4f);
			bodyUp = GameObject.CreatePrimitive(PrimitiveType.Cube);
			bodyUp.name = "Body";
			Object.DestroyImmediate(bodyUp.GetComponent<Collider>());
			bodyUp.transform.parent = gameObject.transform;
			bodyUp.transform.localRotation = Quaternion.identity;
			bodyUp.transform.localScale = new Vector3(bodyDown.transform.localScale.x, bodyDown.transform.localScale.y * 0.666f, bodyDown.transform.localScale.z / 2f);
			bodyUp.transform.localPosition = new Vector3(0f, bodyDown.transform.localScale.y - (bodyDown.transform.localScale.y - bodyUp.transform.localScale.y) / 2f, 0f);
		}
		else
		{
			bodyDown = gameObject.transform.Find("Body").gameObject;
		}
		if (gameObject.transform.Find("Collider") == null)
		{
			mcollider = new GameObject("ColliderDown");
			mcollider.transform.parent = gameObject.transform;
			mcollider.transform.localPosition = Vector3.zero;
			mcollider.transform.localRotation = Quaternion.identity;
			boxColliderDown = mcollider.gameObject.AddComponent<BoxCollider>();
			boxColliderDown.transform.localScale = new Vector3(1.5f, 0.5f, 4f);
			mcollider = new GameObject("ColliderUp");
			mcollider.transform.parent = gameObject.transform;
			mcollider.transform.localRotation = Quaternion.identity;
			boxColliderUp = mcollider.gameObject.AddComponent<BoxCollider>();
			boxColliderUp.transform.localScale = new Vector3(boxColliderDown.transform.localScale.x, boxColliderDown.transform.localScale.y * 0.666f, boxColliderDown.transform.localScale.z / 2f);
			boxColliderUp.transform.localPosition = new Vector3(0f, boxColliderDown.transform.localScale.y - (boxColliderDown.transform.localScale.y - boxColliderUp.transform.localScale.y) / 2f, 0f);
		}
		else
		{
			mcollider = gameObject.transform.Find("Collider").gameObject;
			boxColliderDown = mcollider.gameObject.GetComponent<BoxCollider>();
		}
		if (gameObject.transform.Find("wheelFL") == null)
		{
			wheelFL = new GameObject("wheelFL");
			wheelFL.transform.parent = gameObject.transform;
			wheel = wheelFL.gameObject.AddComponent<Wheel>();
			wheelFL.transform.localPosition = new Vector3((0f - boxColliderDown.transform.localScale.x) / 2f + wheel.width / 2f, -0.1f, boxColliderDown.transform.localScale.z / 2f - boxColliderDown.transform.localScale.z / 8f);
			wheelFL.transform.localRotation = Quaternion.identity;
			wheel.showForces = false;
			wheel.wheelPos = WheelPos.FRONT_LEFT;
			axles.frontAxle.leftWheel = wheel;
			model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			model.name = "modelFL";
			Object.DestroyImmediate(model.GetComponent<Collider>());
			model.transform.parent = wheelFL.transform;
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = new Vector3(wheel.width, wheel.radius * 2f, wheel.radius * 2f);
			wheel.model = model;
		}
		else
		{
			wheelFL = gameObject.transform.Find("wheelFL").gameObject;
		}
		if (LayerMask.NameToLayer("Wheel") != -1)
		{
			wheelFL.gameObject.layer = LayerMask.NameToLayer("Wheel");
		}
		if (gameObject.transform.Find("wheelFR") == null)
		{
			wheelFR = new GameObject("wheelFR");
			wheelFR.transform.parent = gameObject.transform;
			wheel = wheelFR.gameObject.AddComponent<Wheel>();
			wheelFR.transform.localPosition = new Vector3(boxColliderDown.transform.localScale.x / 2f - wheel.width / 2f, -0.1f, boxColliderDown.transform.localScale.z / 2f - boxColliderDown.transform.localScale.z / 8f);
			wheelFR.transform.localRotation = Quaternion.identity;
			wheel.showForces = false;
			wheel.wheelPos = WheelPos.FRONT_RIGHT;
			axles.frontAxle.rightWheel = wheel;
			model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			model.name = "modelFR";
			Object.DestroyImmediate(model.GetComponent<Collider>());
			model.transform.parent = wheelFR.transform;
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = new Vector3(wheel.width, wheel.radius * 2f, wheel.radius * 2f);
			wheel.model = model;
		}
		else
		{
			wheelFR = gameObject.transform.Find("wheelFR").gameObject;
		}
		if (LayerMask.NameToLayer("Wheel") != -1)
		{
			wheelFR.gameObject.layer = LayerMask.NameToLayer("Wheel");
		}
		if (gameObject.transform.Find("wheelRL") == null)
		{
			wheelRL = new GameObject("wheelRL");
			wheelRL.transform.parent = gameObject.transform;
			wheel = wheelRL.gameObject.AddComponent<Wheel>();
			wheelRL.transform.localPosition = new Vector3((0f - boxColliderDown.transform.localScale.x) / 2f + wheel.width / 2f, -0.1f, 0f - (boxColliderDown.transform.localScale.z / 2f - boxColliderDown.transform.localScale.z / 8f));
			wheelRL.transform.localRotation = Quaternion.identity;
			wheel.showForces = false;
			wheel.wheelPos = WheelPos.REAR_LEFT;
			axles.rearAxle.leftWheel = wheel;
			model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			model.name = "modelRL";
			Object.DestroyImmediate(model.GetComponent<Collider>());
			model.transform.parent = wheelRL.transform;
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = new Vector3(wheel.width, wheel.radius * 2f, wheel.radius * 2f);
			wheel.model = model;
		}
		else
		{
			wheelRL = gameObject.transform.Find("wheelRL").gameObject;
		}
		if (LayerMask.NameToLayer("Wheel") != -1)
		{
			wheelRL.gameObject.layer = LayerMask.NameToLayer("Wheel");
		}
		if (gameObject.transform.Find("wheelRR") == null)
		{
			wheelRR = new GameObject("wheelRR");
			wheelRR.transform.parent = gameObject.transform;
			wheel = wheelRR.gameObject.AddComponent<Wheel>();
			wheelRR.transform.localPosition = new Vector3(boxColliderDown.transform.localScale.x / 2f - wheel.width / 2f, -0.1f, 0f - (boxColliderDown.transform.localScale.z / 2f - boxColliderDown.transform.localScale.z / 8f));
			wheelRR.transform.localRotation = Quaternion.identity;
			wheel.showForces = false;
			wheel.wheelPos = WheelPos.REAR_RIGHT;
			axles.rearAxle.rightWheel = wheel;
			model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			model.name = "modelRR";
			Object.DestroyImmediate(model.GetComponent<Collider>());
			model.transform.parent = wheelRR.transform;
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = new Vector3(wheel.width, wheel.radius * 2f, wheel.radius * 2f);
			wheel.model = model;
		}
		else
		{
			wheelRR = gameObject.transform.Find("wheelRR").gameObject;
		}
		if (LayerMask.NameToLayer("Wheel") != -1)
		{
			wheelRR.gameObject.layer = LayerMask.NameToLayer("Wheel");
		}
		axles.frontAxle.maxSteeringAngle = 33f;
		axles.frontAxle.handbrakeFrictionTorque = 0f;
		axles.rearAxle.maxSteeringAngle = 0f;
		axles.rearAxle.handbrakeFrictionTorque = 1000f;
		axles.frontAxle.tires = CarDynamics.Tires.competition_front;
		axles.rearAxle.tires = CarDynamics.Tires.competition_rear;
		axles.SetWheels();
		drivetrain.enabled = true;
		axisCarController.enabled = true;
		return gameObject;
	}
}
