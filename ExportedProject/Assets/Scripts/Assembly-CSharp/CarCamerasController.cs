using UnityEngine;

public class CarCamerasController : MonoBehaviour
{
	private CarCameras carCameras;

	private float sizex;

	private float sizey;

	private float sizez;

	[HideInInspector]
	public float externalSizex;

	[HideInInspector]
	public float externalSizey;

	[HideInInspector]
	public float externalSizez;

	private int i;

	private void Awake()
	{
		carCameras = GetComponent<CarCameras>();
	}

	private float FindMaxBoundingSize(Collider[] colliders)
	{
		sizex = 0f;
		sizey = 0f;
		sizez = 0f;
		if (colliders.Length != 0)
		{
			foreach (Collider collider in colliders)
			{
				if (collider.gameObject.layer != LayerMask.NameToLayer("Wheel") && collider.transform.GetComponent<FuelTank>() == null)
				{
					sizex += collider.bounds.size.x;
					sizey += collider.bounds.size.y;
					sizez += collider.bounds.size.z;
				}
			}
		}
		return Mathf.Max(sizex + externalSizex, sizey + externalSizey, sizez + externalSizez);
	}

	public void SetCamera(int index, Transform target, bool newTarget)
	{
		this.i = index;
		if (newTarget)
		{
			carCameras.mtarget = target;
		}
		carCameras.dampFixedCamera = false;
		carCameras.mouseOrbitFixedCamera = false;
		carCameras.driverView = false;
		if (index == 0 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
			carCameras.target = target;
			carCameras.rotationDamping = 3f;
			carCameras.heightDamping = 100f;
			Collider[] componentsInChildren = target.gameObject.GetComponentsInChildren<Collider>();
			carCameras.distance = FindMaxBoundingSize(componentsInChildren) * 1.5f;
			if (carCameras.distance < 4f)
			{
				carCameras.distance = 4f;
			}
			carCameras.height = carCameras.distance / 2f;
			carCameras.pitchAngle = (0f - carCameras.height) / 1.5f;
			carCameras.yawAngle = 0f;
		}
		else if (index == 1 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.FixedTo;
			Transform[] componentsInChildren2 = carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>();
			foreach (Transform transform in componentsInChildren2)
			{
				if (transform.gameObject.tag == "Fixed_Camera_Driver_View" || transform.gameObject.name == "Fixed_Camera_Driver_View")
				{
					carCameras.target = transform;
				}
			}
			carCameras.distance = 0f;
			carCameras.height = 0f;
			carCameras.pitchAngle = 0f;
			carCameras.yawAngle = 0f;
			carCameras.dampFixedCamera = true;
			carCameras.mouseOrbitFixedCamera = true;
			carCameras.x = 0f;
			carCameras.y = 0f;
			carCameras.driverView = true;
		}
		else if (index == 2 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
			carCameras.target = target;
			carCameras.rotationDamping = 3f;
			carCameras.heightDamping = 100f;
			Collider[] componentsInChildren3 = target.gameObject.GetComponentsInChildren<Collider>();
			carCameras.distance = FindMaxBoundingSize(componentsInChildren3);
			if (carCameras.distance < 4f)
			{
				carCameras.distance = 4f;
			}
			carCameras.distance *= 2f;
			carCameras.height = carCameras.distance / 2f;
			carCameras.pitchAngle = (0f - carCameras.height) / 2.5f;
			carCameras.yawAngle = 0f;
		}
		else if (index == 3 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.FixedTo;
			Transform[] componentsInChildren4 = carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>();
			foreach (Transform transform2 in componentsInChildren4)
			{
				if (transform2.gameObject.tag == "Fixed_Camera_1" || transform2.gameObject.name == "Fixed_Camera_1")
				{
					carCameras.target = transform2;
				}
			}
			carCameras.distance = 0f;
			carCameras.height = 0f;
			carCameras.pitchAngle = 0f;
			carCameras.yawAngle = 0f;
		}
		else if (index == 4 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.FixedTo;
			Transform[] componentsInChildren5 = carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>();
			foreach (Transform transform3 in componentsInChildren5)
			{
				if (transform3.gameObject.tag == "Fixed_Camera_2" || transform3.gameObject.name == "Fixed_Camera_2")
				{
					carCameras.target = transform3;
				}
			}
			carCameras.distance = 0f;
			carCameras.height = 0f;
			carCameras.pitchAngle = 0f;
			carCameras.yawAngle = 0f;
		}
		else if (index == 5 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
			carCameras.rotationDamping = 3f;
			carCameras.heightDamping = 50f;
			carCameras.target = target;
			Collider[] componentsInChildren6 = target.gameObject.GetComponentsInChildren<Collider>();
			carCameras.distance = FindMaxBoundingSize(componentsInChildren6);
			if (carCameras.distance < 4f)
			{
				carCameras.distance = 4f;
			}
			carCameras.height = carCameras.distance / 2f;
			carCameras.pitchAngle = 0f - carCameras.height;
			carCameras.yawAngle = 90f;
		}
		else if (index == 6 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.MouseOrbit;
			carCameras.target = target;
			if (carCameras.distance < carCameras.distanceMin || carCameras.distance > carCameras.distanceMax)
			{
				carCameras.distance = 5f;
			}
			carCameras.x = carCameras.myTransform.eulerAngles.y;
			carCameras.y = carCameras.myTransform.eulerAngles.x;
		}
		else if (index == 7 && target != null)
		{
			carCameras.mycamera = CarCameras.Cameras.Map;
			carCameras.target = target;
			carCameras.distance = 0f;
			carCameras.height = 0f;
			carCameras.pitchAngle = 0f;
			carCameras.yawAngle = 0f;
		}
	}

	private void Start()
	{
		SetCamera(0, carCameras.mtarget, false);
	}

	private void Update()
	{
		if (carCameras.mycamera != CarCameras.Cameras.Map && Input.GetKeyDown(KeyCode.C))
		{
			i++;
			if (i == 7)
			{
				i = 0;
			}
			SetCamera(i, carCameras.mtarget, false);
		}
	}
}
