using System;
using System.Collections.Generic;
using UnityEngine;

public class windshield : MonoBehaviour
{
	[Serializable]
	public struct RainType
	{
		[Range(0f, 1000f)]
		public int dropsPerFrame;

		[Range(0f, 10f)]
		public float dryingSpeed;

		[Range(0f, 10f)]
		public float dropSize;

		public RainType(int dropsPerFrame, float dryingSpeed, float dropSize)
		{
			this.dropsPerFrame = dropsPerFrame;
			this.dryingSpeed = dryingSpeed;
			this.dropSize = dropSize;
		}
	}

	private const int randomBufferSize = 29999;

	[Range(0f, 1f)]
	public float rain;

	public float wind;

	public float gravityMultiplier = 100f;

	public Rect frontUV = new Rect(0f, 0f, 1f, 0.4f);

	public Rect sideUV = new Rect(0f, 0.4f, 1f, 0.3f);

	public Rect rearUV = new Rect(0f, 0.7f, 1f, 0.3f);

	public Color Wet = new Color(1f, 1f, 1f, 1f);

	public Color Dry = new Color(1f, 1f, 1f, 0f);

	public RainType[] raintypes = new RainType[3]
	{
		new RainType(0, 10f, 1f),
		new RainType(200, 2f, 3f),
		new RainType(500, 1f, 4f)
	};

	public Vector2 wiper1 = new Vector2(0.25f, 0f);

	public Vector2 wiper2 = new Vector2(0.75f, 0f);

	public float wiper1AngleOffset;

	public float wiper2AngleOffset;

	public float wiper1Length = 0.33f;

	public float wiper2Length = 0.33f;

	public float wiper1MaxAngle = 45f;

	public float wiper2MaxAngle = 45f;

	[Range(0f, 1f)]
	public float wiper1Position;

	[Range(0f, 1f)]
	public float wiper2Position;

	public bool wipersTestMove;

	public int textureSize = 512;

	public MeshRenderer[] windowRenderers;

	public Transform frontWindowNormal;

	public Transform sideWindowNormal;

	public Transform rearWindowNormal;

	public Transform frontWind;

	public Transform sideWind;

	public Transform rearWind;

	public Vector2 frontFlow;

	public Vector2 sideFlow;

	public Vector2 rearFlow;

	private float previous1Angle = -0.1f;

	private float previous2Angle = -0.1f;

	private Vector2[] randoms;

	private Material mat;

	private Material mat2;

	private List<Material> cachedWindowMaterials;

	private RenderTexture renderTextureA;

	private RenderTexture renderTextureB;

	private RenderTexture active;

	private int randomIndex;

	private bool disabledRain;

	private float dryingSpeed
	{
		get
		{
			if (raintypes != null && raintypes.Length > 1)
			{
				return floor.dryingSpeed * (1f - normalizedRain) + ceil.dryingSpeed * normalizedRain;
			}
			return 0f;
		}
	}

	private float dropSize
	{
		get
		{
			if (raintypes != null && raintypes.Length > 1)
			{
				return floor.dropSize * (1f - normalizedRain) + ceil.dropSize * normalizedRain;
			}
			return 0f;
		}
	}

	private float dropsPerFrame
	{
		get
		{
			if (raintypes != null && raintypes.Length > 1)
			{
				return (float)floor.dropsPerFrame * (1f - normalizedRain) + (float)ceil.dropsPerFrame * normalizedRain;
			}
			return 0f;
		}
	}

	private RainType floor
	{
		get
		{
			return raintypes[floorIndex];
		}
	}

	private RainType ceil
	{
		get
		{
			return raintypes[floorIndex + 1];
		}
	}

	private float normalizedRain
	{
		get
		{
			return rain * (float)(raintypes.Length - 1) % 1f;
		}
	}

	private int floorIndex
	{
		get
		{
			int num = raintypes.Length;
			int num2 = num - 1;
			return Mathf.FloorToInt((float)num2 * Mathf.Clamp01(rain - 0.001f));
		}
	}

	private void OnEnable()
	{
		if (renderTextureA == null)
		{
			RenderTextureFormat format = RenderTextureFormat.ARGB32;
			renderTextureA = new RenderTexture(textureSize, textureSize, 0, format);
			renderTextureA.wrapMode = TextureWrapMode.Repeat;
			renderTextureA.name = "WindshieldA";
			renderTextureA.Create();
			renderTextureB = new RenderTexture(textureSize, textureSize, 0, format);
			renderTextureB.wrapMode = TextureWrapMode.Repeat;
			renderTextureB.name = "WindshieldB";
			renderTextureB.Create();
			active = renderTextureA;
			Graphics.SetRenderTarget(renderTextureA);
			GL.Clear(true, true, Dry);
			Graphics.SetRenderTarget(renderTextureB);
			GL.Clear(true, true, Dry);
			randoms = new Vector2[29999];
			for (int i = 0; i < 29999; i++)
			{
				Vector2 vector = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
				randoms[i] = vector * textureSize;
			}
			cachedWindowMaterials = new List<Material>();
			MeshRenderer[] array = windowRenderers;
			foreach (MeshRenderer meshRenderer in array)
			{
				cachedWindowMaterials.Add(meshRenderer.material);
			}
		}
		mat = new Material(Shader.Find("Custom/windshield"));
		mat2 = new Material(Shader.Find("Custom/windshield2"));
	}

	private void OnDisable()
	{
		if (renderTextureA != null)
		{
			UnityEngine.Object.DestroyImmediate(renderTextureA);
		}
		if (renderTextureB != null)
		{
			UnityEngine.Object.DestroyImmediate(renderTextureB);
		}
	}

	private Vector2 WorldOrientationToUV(Transform t, Vector3 vector)
	{
		return t.InverseTransformPoint(Vector3.ProjectOnPlane(vector, t.forward) + t.position);
	}

	private void Update()
	{
		if (disabledRain && rain == 0f)
		{
			return;
		}
		disabledRain = false;
		foreach (Material cachedWindowMaterial in cachedWindowMaterials)
		{
			cachedWindowMaterial.SetFloat("_Raining", rain);
		}
		if (frontWindowNormal != null)
		{
			frontFlow = WorldOrientationToUV(frontWindowNormal, Vector3.down) * gravityMultiplier + WorldOrientationToUV(frontWindowNormal, frontWind.forward) * wind;
		}
		if (sideWindowNormal != null)
		{
			sideFlow = WorldOrientationToUV(sideWindowNormal, Vector3.down) * gravityMultiplier + WorldOrientationToUV(sideWindowNormal, sideWind.forward) * wind;
		}
		if (rearWindowNormal != null)
		{
			rearFlow = WorldOrientationToUV(rearWindowNormal, Vector3.down) * gravityMultiplier + WorldOrientationToUV(rearWindowNormal, rearWind.forward) * wind;
		}
		if (wipersTestMove)
		{
			wiper1Position = (Mathf.Sin(Time.timeSinceLevelLoad * 7f) + 1f) / 2f;
			wiper2Position = (Mathf.Sin(Time.timeSinceLevelLoad * 7f) + 1f) / 2f;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 1f / 60f);
		mat2.SetColor(Shader.PropertyToID("_Dry"), Dry);
		mat2.SetColor(Shader.PropertyToID("_Wet"), Wet);
		mat2.SetFloat(Shader.PropertyToID("_DryingSpeed"), dryingSpeed * num * 0.1f);
		mat2.SetVector(Shader.PropertyToID("_FrontFlow"), new Vector4(frontFlow.x, frontFlow.y, 0f, 0f) * num);
		mat2.SetVector(Shader.PropertyToID("_SideFlow"), new Vector4(sideFlow.x, sideFlow.y, 0f, 0f) * num);
		mat2.SetVector(Shader.PropertyToID("_RearFlow"), new Vector4(rearFlow.x, rearFlow.y, 0f, 0f) * num);
		mat2.SetVector(Shader.PropertyToID("_FrontUV"), new Vector4(frontUV.xMin, frontUV.yMin, frontUV.xMax, frontUV.yMax));
		mat2.SetVector(Shader.PropertyToID("_SideUV"), new Vector4(sideUV.xMin, sideUV.yMin, sideUV.xMax, sideUV.yMax));
		mat2.SetVector(Shader.PropertyToID("_RearUV"), new Vector4(rearUV.xMin, rearUV.yMin, rearUV.xMax, rearUV.yMax));
		RenderTexture renderTexture = active;
		active = ((!(active == renderTextureA)) ? renderTextureA : renderTextureB);
		Graphics.SetRenderTarget(renderTexture);
		mat.SetColor(Shader.PropertyToID("_Color"), Wet);
		mat.SetPass(0);
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, textureSize, 0f, textureSize);
		GL.Begin(7);
		float num2 = dropSize * 0.5f;
		float num3 = (UnityEngine.Random.value - 0.5f) * 20f;
		float num4 = (UnityEngine.Random.value - 0.5f) * 20f;
		int num5 = Mathf.RoundToInt(dropsPerFrame * (num * 60f));
		if (randomIndex + num5 * 2 > 29999)
		{
			randomIndex = 0;
		}
		for (int i = 0; i < num5; i++)
		{
			Vector2 vector = randoms[randomIndex++];
			GL.Vertex3(vector.x, vector.y - num2, 0f);
			GL.Vertex3(vector.x + num2, vector.y, 0f);
			GL.Vertex3(vector.x, vector.y + num2, 0f);
			GL.Vertex3(vector.x - num2, vector.y, 0f);
		}
		GL.End();
		float f = (0f - wiper1MaxAngle + wiper1Position * wiper1MaxAngle * 2f + wiper1AngleOffset) * ((float)Math.PI / 180f);
		float f2 = (0f - wiper2MaxAngle + wiper2Position * wiper2MaxAngle * 2f + wiper2AngleOffset) * ((float)Math.PI / 180f);
		Vector2 vector2 = new Vector2(Mathf.Sin(previous1Angle), Mathf.Cos(previous1Angle)) * wiper1Length;
		Vector2 vector3 = new Vector2(Mathf.Sin(f), Mathf.Cos(f)) * wiper1Length;
		Vector2 vector4 = new Vector2(Mathf.Sin(previous2Angle), Mathf.Cos(previous2Angle)) * wiper2Length;
		Vector2 vector5 = new Vector2(Mathf.Sin(f2), Mathf.Cos(f2)) * wiper2Length;
		Vector3 v = new Vector3(wiper1.x * (float)textureSize, wiper1.y * (float)textureSize, 0f);
		Vector3 v2 = new Vector3((wiper1.x + vector2.x) * (float)textureSize, (wiper1.y + vector2.y) * (float)textureSize, 0f);
		Vector3 v3 = new Vector3((wiper1.x + vector3.x) * (float)textureSize, (wiper1.y + vector3.y) * (float)textureSize, 0f);
		Vector3 v4 = new Vector3(wiper2.x * (float)textureSize, wiper2.y * (float)textureSize, 0f);
		Vector3 v5 = new Vector3((wiper2.x + vector4.x) * (float)textureSize, (wiper2.y + vector4.y) * (float)textureSize, 0f);
		Vector3 v6 = new Vector3((wiper2.x + vector5.x) * (float)textureSize, (wiper2.y + vector5.y) * (float)textureSize, 0f);
		mat.SetPass(0);
		GL.Begin(7);
		GL.Color(Dry);
		GL.Vertex(v);
		GL.Vertex(v2);
		GL.Vertex(v3);
		GL.Vertex(v);
		GL.Vertex(v4);
		GL.Vertex(v5);
		GL.Vertex(v6);
		GL.Vertex(v4);
		GL.End();
		GL.PopMatrix();
		Graphics.SetRenderTarget(active);
		mat2.mainTexture = renderTexture;
		mat2.SetPass(0);
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, textureSize, textureSize, 0f);
		GL.Begin(7);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(0f, 0f, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(textureSize, 0f, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(textureSize, textureSize, 0f);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(0f, textureSize, 0f);
		GL.End();
		GL.PopMatrix();
		foreach (Material cachedWindowMaterial2 in cachedWindowMaterials)
		{
			cachedWindowMaterial2.SetTexture("_Rain", active);
		}
		previous1Angle = f;
		previous2Angle = f2;
		if (rain == 0f)
		{
			disabledRain = true;
		}
	}
}
