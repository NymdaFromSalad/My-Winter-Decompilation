using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Motion Blur Edge")]
public class MotionBlurEdge : ImageEffectBase
{
	public float blurAmount = 0.8f;

	public float endRadius = 0.5f;

	public float startRadius = 0.2f;

	public float minSpeed = 10f;

	public float maxSpeed = 60f;

	public float centerOffsetX;

	public float centerOffsetY;

	public float transitionCurve = 1f;

	public bool debugView;

	private float speedFactor;

	private float speed;

	private Vector3 lastPos = Vector3.zero;

	private Transform myTransform;

	private RenderTexture accumTexture;

	protected new void OnDisable()
	{
		base.OnDisable();
		Object.DestroyImmediate(accumTexture);
	}

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void FixedUpdate()
	{
		if (maxSpeed < 0f)
		{
			maxSpeed = 0f;
		}
		minSpeed = Mathf.Clamp(minSpeed, 0f, maxSpeed);
		speed = Mathf.Abs(Vector3.Dot(myTransform.forward, myTransform.position - lastPos)) / Time.deltaTime;
		lastPos = myTransform.position;
		speedFactor = Mathf.Clamp01(Mathf.InverseLerp(minSpeed, maxSpeed, speed));
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
		{
			if (accumTexture != null)
			{
				Object.DestroyImmediate(accumTexture);
			}
			accumTexture = new RenderTexture(source.width, source.height, 0);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			ImageEffects.Blit(source, accumTexture);
		}
		if (debugView)
		{
			Shader.EnableKeyword("MOTIONBLUREDGE_DEBUG");
			Shader.DisableKeyword("MOTIONBLUREDGE_NORMAL");
		}
		else
		{
			Shader.EnableKeyword("MOTIONBLUREDGE_NORMAL");
			Shader.DisableKeyword("MOTIONBLUREDGE_DEBUG");
		}
		endRadius = Mathf.Clamp(endRadius, 0.01f, 10f);
		startRadius = Mathf.Clamp(startRadius, 0.01f, 10f);
		float num = endRadius * (float)source.height * 0.5f;
		float num2 = startRadius * (float)source.height * 0.5f;
		float num3 = 1f / (num - num2);
		float w = num3 * num2;
		Shader.SetGlobalVector("_WindowsCorrection", new Vector4(source.width, source.height, 0f, 0f));
		base.material.SetTexture("_MainTex", accumTexture);
		base.material.SetFloat("_AccumOrig", 1f - blurAmount * speedFactor);
		base.material.SetVector("_CenterPos", new Vector4((float)source.width * (0.5f + centerOffsetX), (float)source.height * (0.5f + centerOffsetY), num3, w));
		base.material.SetFloat("_TransitionCurve", transitionCurve);
		ImageEffects.BlitWithMaterial(base.material, source, accumTexture);
		ImageEffects.Blit(accumTexture, destination);
	}
}
