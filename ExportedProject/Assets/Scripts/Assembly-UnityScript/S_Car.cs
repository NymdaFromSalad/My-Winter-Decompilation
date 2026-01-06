using System;
using UnityEngine;

[Serializable]
public class S_Car : MonoBehaviour
{
	public virtual void FixedUpdate()
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * 20f, ForceMode.Impulse);
		GetComponent<Rigidbody>().AddTorque(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * 12f, ForceMode.Impulse);
	}

	public virtual void OnGUI()
	{
		GUILayout.Label("Arrows to control the car. Click and hold on car to repair damages under mouse cursor.");
	}

	public virtual void Main()
	{
	}
}
