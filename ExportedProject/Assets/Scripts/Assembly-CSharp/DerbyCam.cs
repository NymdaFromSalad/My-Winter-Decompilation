using UnityEngine;

public class DerbyCam : MonoBehaviour
{
	public GameObject Car;

	private void Update()
	{
		base.transform.LookAt(Car.transform);
	}
}
