using UnityEngine;

public class AerodynamicResistance : MonoBehaviour
{
	public float Cx = 0.3f;

	public float Area = 1.858f;

	public float dragForce;

	private Rigidbody body;

	private CarDynamics cardynamics;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		cardynamics = GetComponent<CarDynamics>();
	}

	private void FixedUpdate()
	{
		if (body.velocity.sqrMagnitude <= 0.001f)
		{
			dragForce = 0f;
		}
		else
		{
			dragForce = 0.5f * Cx * Area * cardynamics.airDensity * body.velocity.sqrMagnitude;
		}
		body.AddForce((0f - dragForce) * body.velocity.normalized);
	}
}
