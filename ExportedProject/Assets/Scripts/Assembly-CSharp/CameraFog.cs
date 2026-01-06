using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraFog : MonoBehaviour
{
	public bool Enabled;

	public float StartDistance;

	public float EndDistance;

	public FogMode Mode;

	public float Density;

	public Color Color;

	private float _startDistance;

	private float _endDistance;

	private FogMode _mode;

	private float _density;

	private Color _color;

	private bool _enabled;

	private void OnPreRender()
	{
		_startDistance = RenderSettings.fogStartDistance;
		_endDistance = RenderSettings.fogEndDistance;
		_mode = RenderSettings.fogMode;
		_density = RenderSettings.fogDensity;
		_color = RenderSettings.fogColor;
		_enabled = RenderSettings.fog;
		RenderSettings.fog = Enabled;
		RenderSettings.fogStartDistance = StartDistance;
		RenderSettings.fogEndDistance = EndDistance;
		RenderSettings.fogMode = Mode;
		RenderSettings.fogDensity = Density;
		RenderSettings.fogColor = Color;
	}

	private void OnPostRender()
	{
		RenderSettings.fog = _enabled;
		RenderSettings.fogStartDistance = _startDistance;
		RenderSettings.fogEndDistance = _endDistance;
		RenderSettings.fogMode = _mode;
		RenderSettings.fogDensity = _density;
		RenderSettings.fogColor = _color;
	}
}
