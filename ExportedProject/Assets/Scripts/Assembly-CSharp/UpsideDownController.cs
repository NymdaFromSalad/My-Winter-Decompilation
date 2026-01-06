using UnityEngine;

public class UpsideDownController : MonoBehaviour
{
	[HideInInspector]
	public CarDynamics carDynamics;

	private bool startTimer1;

	private bool startTimer2;

	private float timer1;

	private float timer2;

	private void Start()
	{
		carDynamics = GetComponent<CarDynamics>();
	}

	private void Update()
	{
		timer1 += Time.deltaTime;
		if (timer1 >= 1f)
		{
			timer1 = 0f;
			startTimer1 = true;
		}
		else
		{
			startTimer1 = false;
		}
		if (startTimer1 && carDynamics != null)
		{
			if (!carDynamics.AllWheelsOnGround() && GetComponent<Rigidbody>().velocity.sqrMagnitude <= 1f)
			{
				startTimer2 = true;
			}
			else
			{
				startTimer2 = false;
			}
		}
		if (startTimer2)
		{
			timer2 += Time.deltaTime;
			if (timer2 >= 1f)
			{
				ResetCarPosition();
				timer1 = 0f;
				timer2 = 0f;
				startTimer2 = false;
			}
		}
		else
		{
			timer2 = 0f;
		}
	}

	private void ResetCarPosition()
	{
		if (!GetComponent<Rigidbody>().isKinematic)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 3f, base.transform.position.z);
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}
