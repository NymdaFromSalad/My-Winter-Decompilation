using UnityEngine;

public class MoveCube : MonoBehaviour
{
	private int direction = 1;

	private void Start()
	{
		Application.targetFrameRate = -1;
	}

	private void LateUpdate()
	{
		if (base.transform.position.x > 10f)
		{
			direction = -1;
		}
		else if (base.transform.position.x < -10f)
		{
			direction = 1;
		}
		base.transform.localPosition += new Vector3(10f * Time.deltaTime * (float)direction, 0f, 0f);
	}
}
