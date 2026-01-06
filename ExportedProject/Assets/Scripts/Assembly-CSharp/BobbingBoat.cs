using UnityEngine;

public class BobbingBoat : MonoBehaviour
{
	private void Update()
	{
		Vector3 position = base.transform.position;
		position.y = Mathf.Cos(Time.time) * 0.06f - 0.15f;
		base.transform.position = position;
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.x = Mathf.Sin(Time.time) * 5f;
		eulerAngles.z = Mathf.Sin(Time.time * 0.8f) * 5f;
		base.transform.eulerAngles = eulerAngles;
	}
}
