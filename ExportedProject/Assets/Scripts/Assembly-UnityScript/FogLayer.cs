using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Rendering/Fog Layer")]
public class FogLayer : MonoBehaviour
{
	private bool revertFogState;

	public virtual void OnPreRender()
	{
		revertFogState = RenderSettings.fog;
		RenderSettings.fog = enabled;
	}

	public virtual void OnPostRender()
	{
		RenderSettings.fog = revertFogState;
	}

	public virtual void Main()
	{
	}
}
