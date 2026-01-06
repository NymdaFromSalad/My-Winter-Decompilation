using UnityEngine;

[RequireComponent(typeof(CarCamerasController))]
public class CarCameras : MonoBehaviour
{
	public enum Cameras
	{
		SmoothLookAt = 0,
		MouseOrbit = 1,
		FixedTo = 2,
		Map = 3
	}

	public Cameras mycamera;

	public Transform target;

	public float distance = 10f;

	public float height = 2.5f;

	public float yawAngle;

	public float pitchAngle = -2.5f;

	public float rotationDamping = 3f;

	public float heightDamping = 2f;

	[HideInInspector]
	public bool dampFixedCamera;

	[HideInInspector]
	public bool mouseOrbitFixedCamera;

	[HideInInspector]
	public bool driverView;

	private float xSpeed = 10f;

	private float ySpeed = 10f;

	private float yMinLimit = -20f;

	private float yMaxLimit = 80f;

	[HideInInspector]
	public float distanceMin = 4f;

	[HideInInspector]
	public float distanceMax = 30f;

	private float mouseDamping = 10f;

	[HideInInspector]
	public float x;

	[HideInInspector]
	public float y;

	private float currentYawRotationAngle;

	private float wantedYawRotationAngle;

	private float currentHeight;

	private float wantedHeight;

	private float deltaPitchAngle;

	private Vector3 deltaMovement;

	private Vector3 oldVelocity;

	private Vector3 deltaVelocity;

	private Vector3 velocity;

	private Vector3 accel;

	private float centrifugalAccel;

	private CarDynamics cardynamics;

	[HideInInspector]
	public Transform myTransform;

	[HideInInspector]
	public Transform mtarget;

	private Quaternion rotation;

	private void Start()
	{
		myTransform = base.transform;
		mtarget = target;
		if ((bool)mtarget)
		{
			cardynamics = mtarget.GetComponent<CarDynamics>();
		}
	}

	private void LateUpdate()
	{
		if (!target)
		{
			return;
		}
		if (mycamera == Cameras.MouseOrbit)
		{
			x += Input.GetAxis("Mouse X") * xSpeed;
			y -= Input.GetAxis("Mouse Y") * ySpeed;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.Euler(y, x, 0f), Time.deltaTime * mouseDamping);
			distance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
			distance = Mathf.Clamp(distance, distanceMin, distanceMax);
			Vector3 vector = new Vector3(0f, 0f, 0f - distance);
			Vector3 position = rotation * vector + target.position;
			myTransform.rotation = rotation;
			myTransform.position = position;
		}
		else if (mycamera == Cameras.Map)
		{
			myTransform.position = new Vector3(target.position.x, myTransform.position.y, target.position.z);
			myTransform.eulerAngles = new Vector3(myTransform.eulerAngles.x, target.eulerAngles.y, myTransform.eulerAngles.z);
		}
		else if (mycamera == Cameras.SmoothLookAt)
		{
			wantedYawRotationAngle = target.eulerAngles.y + yawAngle;
			currentYawRotationAngle = myTransform.eulerAngles.y;
			wantedHeight = target.position.y + height;
			currentHeight = myTransform.position.y;
			currentYawRotationAngle = Mathf.LerpAngle(currentYawRotationAngle, wantedYawRotationAngle, rotationDamping * Time.deltaTime);
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, currentYawRotationAngle, 0f);
			myTransform.position = target.position;
			myTransform.position -= quaternion * Vector3.forward * distance;
			myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);
			myTransform.LookAt(new Vector3(target.position.x, target.position.y + height + pitchAngle, target.position.z));
		}
		else if (mycamera == Cameras.FixedTo)
		{
			if (mouseOrbitFixedCamera)
			{
				x += Input.GetAxis("Mouse X") * xSpeed;
				y -= Input.GetAxis("Mouse Y") * ySpeed;
				y = ClampAngle(y, yMinLimit, yMaxLimit);
			}
			else
			{
				x = (y = 0f);
			}
			rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.Euler(y + target.eulerAngles.x + pitchAngle + deltaPitchAngle, x + target.eulerAngles.y + yawAngle, target.eulerAngles.z), Time.time);
			myTransform.rotation = rotation;
			if (dampFixedCamera)
			{
				Vector3 position2 = new Vector3(target.position.x, target.position.y + height, target.position.z) - myTransform.forward * distance - (deltaMovement.z * target.forward + deltaMovement.y * target.up + deltaMovement.x * target.right);
				myTransform.position = position2;
			}
			else
			{
				myTransform.eulerAngles = new Vector3(target.eulerAngles.x + pitchAngle, target.eulerAngles.y + yawAngle, target.eulerAngles.z);
				myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z) - myTransform.forward * distance;
			}
		}
	}

	private void FixedUpdate()
	{
		if (dampFixedCamera)
		{
			oldVelocity = velocity;
			velocity = mtarget.InverseTransformDirection(mtarget.GetComponent<Rigidbody>().velocity);
			deltaVelocity = velocity - oldVelocity;
			if (cardynamics != null)
			{
				centrifugalAccel = cardynamics.GetCentrifugalAccel();
			}
			accel = deltaVelocity / Time.deltaTime + centrifugalAccel * Vector3.right;
			deltaMovement = accel * Time.deltaTime * Time.deltaTime * 5f;
			deltaMovement.z = Mathf.Clamp(deltaMovement.z, -0.2f, 0.2f);
			deltaMovement.y = Mathf.Clamp(deltaMovement.y, -0.1f, 0.1f);
			deltaMovement.x = Mathf.Clamp(deltaMovement.x, -0.01f, 0.01f);
			deltaPitchAngle = deltaMovement.y * 20f - deltaMovement.z * 20f;
			deltaPitchAngle = Mathf.Clamp(deltaPitchAngle, -5f, 5f);
		}
	}

	private static float ClampAngle(float yawAngle, float min, float max)
	{
		if (yawAngle < -360f)
		{
			yawAngle += 360f;
		}
		else if (yawAngle > 360f)
		{
			yawAngle -= 360f;
		}
		return Mathf.Clamp(yawAngle, min, max);
	}
}
