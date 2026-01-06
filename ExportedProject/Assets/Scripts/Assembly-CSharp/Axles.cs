using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Axles : MonoBehaviour
{
	[HideInInspector]
	public Wheel[] otherWheels = new Wheel[0];

	[HideInInspector]
	public Wheel[] allWheels;

	public Axle frontAxle = new Axle();

	public Axle rearAxle = new Axle();

	public Axle[] otherAxles = new Axle[0];

	private void CheckWheels()
	{
		if (frontAxle.leftWheel == null)
		{
			Debug.LogWarning("UnityCar: front left wheel not assigned  (" + base.transform.name + ")");
		}
		if (frontAxle.rightWheel == null)
		{
			Debug.LogWarning("UnityCar: front right wheel not assigned  (" + base.transform.name + ")");
		}
		if (rearAxle.leftWheel == null)
		{
			Debug.LogWarning("UnityCar: rear left wheel not assigned  (" + base.transform.name + ")");
		}
		if (rearAxle.rightWheel == null)
		{
			Debug.LogWarning("UnityCar: rear right wheel not assigned  (" + base.transform.name + ")");
		}
	}

	private void Start()
	{
		CheckWheels();
	}

	private void Awake()
	{
		SetWheels();
	}

	public void SetWheels()
	{
		if ((bool)frontAxle.leftWheel)
		{
			frontAxle.leftWheel.wheelPos = WheelPos.FRONT_LEFT;
		}
		if ((bool)frontAxle.rightWheel)
		{
			frontAxle.rightWheel.wheelPos = WheelPos.FRONT_RIGHT;
		}
		if ((bool)rearAxle.leftWheel)
		{
			rearAxle.leftWheel.wheelPos = WheelPos.REAR_LEFT;
		}
		if ((bool)rearAxle.rightWheel)
		{
			rearAxle.rightWheel.wheelPos = WheelPos.REAR_RIGHT;
		}
		frontAxle.wheels = new Wheel[0];
		if (frontAxle.leftWheel != null && frontAxle.rightWheel != null)
		{
			frontAxle.wheels = new Wheel[2];
			frontAxle.wheels[0] = frontAxle.leftWheel;
			frontAxle.wheels[1] = frontAxle.rightWheel;
		}
		else if (frontAxle.leftWheel != null || frontAxle.rightWheel != null)
		{
			frontAxle.wheels = new Wheel[1];
			if (frontAxle.leftWheel != null)
			{
				frontAxle.wheels[0] = frontAxle.leftWheel;
			}
			else
			{
				frontAxle.wheels[0] = frontAxle.rightWheel;
			}
		}
		frontAxle.camber = Mathf.Clamp(frontAxle.camber, -10f, 10f);
		rearAxle.wheels = new Wheel[0];
		if (rearAxle.leftWheel != null && rearAxle.rightWheel != null)
		{
			rearAxle.wheels = new Wheel[2];
			rearAxle.wheels[0] = rearAxle.leftWheel;
			rearAxle.wheels[1] = rearAxle.rightWheel;
		}
		else if (rearAxle.leftWheel != null || rearAxle.rightWheel != null)
		{
			rearAxle.wheels = new Wheel[1];
			if (rearAxle.leftWheel != null)
			{
				rearAxle.wheels[0] = rearAxle.leftWheel;
			}
			else
			{
				rearAxle.wheels[0] = rearAxle.rightWheel;
			}
		}
		rearAxle.camber = Mathf.Clamp(rearAxle.camber, -10f, 10f);
		Wheel[] array = new Wheel[otherAxles.Length * 2];
		int num = 0;
		Axle[] array2 = otherAxles;
		foreach (Axle axle in array2)
		{
			if (axle.leftWheel != null && axle.rightWheel != null)
			{
				axle.wheels = new Wheel[2];
				axle.wheels[0] = (array[num] = axle.leftWheel);
				axle.wheels[1] = (array[num + 1] = axle.rightWheel);
				num += 2;
			}
			else
			{
				axle.wheels = new Wheel[1];
				if (axle.leftWheel != null)
				{
					axle.wheels[0] = (array[0] = axle.leftWheel);
				}
				else
				{
					axle.wheels[0] = (array[0] = axle.rightWheel);
				}
				num++;
			}
			axle.camber = Mathf.Clamp(axle.camber, -10f, 10f);
		}
		otherWheels = new Wheel[num];
		array.CopyTo(otherWheels, 0);
		allWheels = new Wheel[frontAxle.wheels.Length + rearAxle.wheels.Length + otherWheels.Length];
		frontAxle.wheels.CopyTo(allWheels, 0);
		rearAxle.wheels.CopyTo(allWheels, frontAxle.wheels.Length);
		if (otherWheels.Length != 0)
		{
			otherWheels.CopyTo(allWheels, frontAxle.wheels.Length + rearAxle.wheels.Length);
		}
	}
}
