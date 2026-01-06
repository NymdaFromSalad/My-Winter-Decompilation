using UnityEngine;

public class CutoffModifier : MonoBehaviour
{
	public float cutoff;

	private void Update()
	{
		GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);
	}
}
