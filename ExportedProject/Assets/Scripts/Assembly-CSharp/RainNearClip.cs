using UnityEngine;

public class RainNearClip : MonoBehaviour
{
	public float clipValue = 1f;

	private void SetNearClip(float clipValue)
	{
		Shader.SetGlobalFloat("_NearClipDistance", clipValue);
	}
}
