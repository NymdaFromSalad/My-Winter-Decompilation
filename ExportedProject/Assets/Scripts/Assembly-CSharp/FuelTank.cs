using UnityEngine;

public class FuelTank : MonoBehaviour
{
	private Rigidbody fuelTank;

	private BoxCollider boxCollider;

	private Drivetrain drivetrain;

	private Transform myTransform;

	public float _tankCapacity = 50f;

	public float _currentFuel = 50f;

	public float _tankWeight = 10f;

	public float _fuelDensity = 0.73722f;

	public float tankWeight
	{
		get
		{
			return _tankWeight;
		}
		set
		{
			if (value <= 0f)
			{
				_tankWeight = 0.001f;
			}
			else
			{
				_tankWeight = value;
			}
		}
	}

	public float tankCapacity
	{
		get
		{
			return _tankCapacity;
		}
		set
		{
			if (value < 0f)
			{
				_tankCapacity = 0f;
			}
			else
			{
				_tankCapacity = value;
			}
			if (_currentFuel > _tankCapacity)
			{
				_currentFuel = _tankCapacity;
			}
		}
	}

	public float currentFuel
	{
		get
		{
			return _currentFuel;
		}
		set
		{
			if (value < 0f)
			{
				_currentFuel = 0f;
			}
			else if (value > tankCapacity)
			{
				_currentFuel = tankCapacity;
			}
			else
			{
				_currentFuel = value;
			}
		}
	}

	public float fuelDensity
	{
		get
		{
			return _fuelDensity;
		}
		set
		{
			if (value < 0f)
			{
				_fuelDensity = 0f;
			}
			else
			{
				_fuelDensity = value;
			}
		}
	}

	private void Start()
	{
		myTransform = base.transform;
		Transform parent = myTransform.parent;
		while (parent.GetComponent<Drivetrain>() == null)
		{
			parent = parent.parent;
		}
		drivetrain = parent.GetComponent<Drivetrain>();
		tankCapacity = tankCapacity;
		currentFuel = currentFuel;
		fuelDensity = fuelDensity;
		myTransform.gameObject.layer = drivetrain.transform.gameObject.layer;
		fuelTank = myTransform.gameObject.GetComponent<Rigidbody>();
		if (fuelTank == null)
		{
			fuelTank = myTransform.gameObject.AddComponent<Rigidbody>();
		}
		boxCollider = myTransform.gameObject.GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = myTransform.gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(0.7f, 0.25f, 0.6f);
		}
		fuelTank.drag = 0f;
		fuelTank.angularDrag = 0f;
		fuelTank.useGravity = true;
		fuelTank.isKinematic = false;
		ConfigurableJoint configurableJoint = fuelTank.GetComponent<ConfigurableJoint>();
		if (configurableJoint == null)
		{
			configurableJoint = fuelTank.gameObject.AddComponent<ConfigurableJoint>();
			configurableJoint.xMotion = ConfigurableJointMotion.Locked;
			configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			configurableJoint.zMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
		}
		configurableJoint.connectedBody = drivetrain.transform.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (tankCapacity > 0f && currentFuel >= 0f && drivetrain.rpm >= 20f)
		{
			currentFuel -= drivetrain.istantConsumption * Time.deltaTime * (1f / (float)drivetrain.fuelTanks.Length);
			currentFuel = Mathf.Clamp(currentFuel, 0f, currentFuel);
		}
		fuelTank.mass = currentFuel * fuelDensity + tankWeight;
	}
}
