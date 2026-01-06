using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
	public float maxSteeringAngle = 270f;

	public bool rotateAroundY;

	public bool invertRotation;

	private Transform myTransform;

	[HideInInspector]
	public CarController carController;

	private float z;

	private int sign = 1;

	private void Start()
	{
		myTransform = base.transform;
		if (rotateAroundY)
		{
			z = myTransform.localEulerAngles.y;
		}
		else
		{
			z = myTransform.localEulerAngles.z;
		}
		if (invertRotation)
		{
			sign = -1;
		}
	}

	private void Update()
	{
		if ((bool)carController)
		{
			sign = 1;
			if (invertRotation)
			{
				sign = -1;
			}
			if (rotateAroundY)
			{
				myTransform.localEulerAngles = new Vector3(myTransform.localEulerAngles.x, z + (float)sign * carController.steering * maxSteeringAngle, myTransform.localEulerAngles.z);
			}
			else
			{
				myTransform.localEulerAngles = new Vector3(myTransform.localEulerAngles.x, myTransform.localEulerAngles.y, z + (float)sign * carController.steering * maxSteeringAngle);
			}
		}
	}
}
