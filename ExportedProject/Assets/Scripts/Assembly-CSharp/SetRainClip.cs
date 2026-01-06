using UnityEngine;

public class SetRainClip : MonoBehaviour
{
	public void SetClipValue(float clipValue)
	{
		Shader.SetGlobalFloat("_NearClipDistance", clipValue);
	}
}
