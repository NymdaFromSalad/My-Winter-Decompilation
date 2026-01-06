using UnityEngine;

public class BrakeLights : MonoBehaviour
{
	[HideInInspector]
	public CarController carController;

	public Material brakeLights;

	private float startValue;

	private float intensityValue;

	private void Awake()
	{
		carController = base.transform.GetComponent<CarController>();
		if ((bool)brakeLights)
		{
			startValue = brakeLights.GetFloat("_Intensity");
		}
	}

	private void FixedUpdate()
	{
		if (!brakeLights)
		{
			return;
		}
		if (carController.brakeKey)
		{
			if (intensityValue < startValue + 1f)
			{
				intensityValue += Time.deltaTime / 0.1f;
				brakeLights.SetFloat("_Intensity", intensityValue);
			}
		}
		else if (intensityValue > startValue)
		{
			intensityValue -= Time.deltaTime / 0.1f;
			brakeLights.SetFloat("_Intensity", intensityValue);
		}
	}
}
