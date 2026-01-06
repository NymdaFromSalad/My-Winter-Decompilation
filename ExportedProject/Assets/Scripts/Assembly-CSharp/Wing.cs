using UnityEngine;

public class Wing : MonoBehaviour
{
	public float dragCoefficient = 0.05f;

	public int angleOfAttack = 1;

	public float area = 1f;

	public float downForce;

	public float dragForce;

	private Rigidbody body;

	private Transform myTransform;

	private CarDynamics cardynamics;

	private void Start()
	{
		Transform parent = base.transform;
		myTransform = base.transform;
		while (parent != null && parent.GetComponent<Rigidbody>() == null)
		{
			parent = parent.parent;
		}
		if (parent != null)
		{
			body = parent.GetComponent<Rigidbody>();
		}
		parent = base.transform;
		while (parent.GetComponent<CarDynamics>() == null)
		{
			parent = parent.parent;
		}
		cardynamics = parent.GetComponent<CarDynamics>();
	}

	private void FixedUpdate()
	{
		if (body != null)
		{
			float num = body.velocity.x * body.velocity.x + body.velocity.z * body.velocity.z;
			if (num > 0.1f)
			{
				downForce = 0.5f * area * (float)angleOfAttack * dragCoefficient * cardynamics.airDensity * num;
				dragForce = 0.5f * dragCoefficient * area * cardynamics.airDensity * num;
				body.AddForceAtPosition((0f - downForce) * myTransform.up, myTransform.position);
				body.AddForceAtPosition((0f - dragForce) * myTransform.forward, myTransform.position);
				Debug.DrawRay(myTransform.position, (0f - downForce) * myTransform.up / 1000f, Color.white);
				Debug.DrawRay(myTransform.position, (0f - dragForce) * myTransform.forward / 1000f, Color.white);
			}
		}
	}
}
