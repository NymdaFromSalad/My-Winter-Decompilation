using UnityEngine;

public class Car : MonoBehaviour
{
	private void FixedUpdate()
	{
		GetComponent<Rigidbody>().AddForce(base.transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * 20f, ForceMode.Impulse);
		GetComponent<Rigidbody>().AddTorque(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * 12f, ForceMode.Impulse);
	}

	private void OnGUI()
	{
		GUILayout.Label("Arrows to control the car. Click and hold on car to repair damages under mouse cursor.");
	}
}
