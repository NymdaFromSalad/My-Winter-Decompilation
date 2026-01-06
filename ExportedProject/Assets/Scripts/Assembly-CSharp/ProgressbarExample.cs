using UnityEngine;

public class ProgressbarExample : MonoBehaviour
{
	public float progressPercent = 100f;

	private void Update()
	{
		float value = Mathf.Clamp01(progressPercent * 0.01f);
		GetComponent<Renderer>().material.SetFloat("_Progress", value);
	}
}
