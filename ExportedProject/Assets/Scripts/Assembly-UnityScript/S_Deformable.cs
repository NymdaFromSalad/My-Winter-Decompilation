using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class S_Deformable : MonoBehaviour
{
	public MeshFilter meshFilter;

	public float Hardness;

	public bool DeformMeshCollider;

	public float UpdateFrequency;

	public float MaxVertexMov;

	public Color32 DeformedVertexColor;

	public Texture2D HardnessMap;

	private Mesh mesh;

	private MeshCollider meshCollider;

	private Vector3[] baseVertices;

	private Color32[] baseColors;

	private float sizeFactor;

	private Vector3[] vertices;

	private Color32[] colors;

	private float[] map;

	private bool meshUpdate;

	private float lastUpdate;

	private Texture2D appliedMap;

	public S_Deformable()
	{
		Hardness = 0.5f;
		DeformMeshCollider = true;
		DeformedVertexColor = Color.gray;
	}

	public virtual void Awake()
	{
		meshCollider = (MeshCollider)GetComponent(typeof(MeshCollider));
		if (!meshFilter)
		{
			meshFilter = (MeshFilter)GetComponent(typeof(MeshFilter));
		}
		if ((bool)meshFilter)
		{
			LoadMesh();
		}
		else
		{
			Debug.LogWarning("Deformable component warning: No mesh filter assigned for object " + gameObject.ToString());
		}
	}

	private void LoadMesh()
	{
		if ((bool)meshFilter)
		{
			mesh = meshFilter.mesh;
		}
		else
		{
			mesh = null;
		}
		if (!mesh)
		{
			Debug.LogWarning("Deformable component warning: Mesh at mesh filter is null " + gameObject.ToString());
			return;
		}
		vertices = mesh.vertices;
		colors = mesh.colors32;
		baseVertices = mesh.vertices;
		baseColors = mesh.colors32;
		Vector3 size = mesh.bounds.size;
		sizeFactor = Mathf.Max(1f, Mathf.Min(size.x, size.y, size.z));
	}

	private void LoadMap()
	{
		appliedMap = HardnessMap;
		if ((bool)HardnessMap)
		{
			Vector2[] uv = mesh.uv;
			map = new float[Extensions.get_length((System.Array)uv)];
			int num = 0;
			int i = 0;
			Vector2[] array = uv;
			for (int length = array.Length; i < length; i++)
			{
				try
				{
					map[num] = HardnessMap.GetPixelBilinear(array[i].x, array[i].y).a;
				}
				catch (Exception)
				{
					Debug.LogWarning("Deformable component warning: Texture at HardnessMap must be readable (check Read/Write Enabled at import settings). Hardness map not applied.");
					map = null;
					break;
				}
				num++;
			}
		}
		else
		{
			map = null;
		}
	}

	private void Deform(Collision collision)
	{
		if (!mesh || !meshFilter)
		{
			return;
		}
		float num = Mathf.Min(1f, collision.relativeVelocity.sqrMagnitude / 1000f);
		if (!(num >= 0.01f))
		{
			return;
		}
		float num2 = num * (sizeFactor * (0.1f / Mathf.Max(0.1f, Hardness)));
		int i = 0;
		ContactPoint[] contacts = collision.contacts;
		for (int length = contacts.Length; i < length; i++)
		{
			int num3 = 0;
			int j = 0;
			Vector3[] array = vertices;
			for (int length2 = array.Length; j < length2; j++)
			{
				Vector3 vector = meshFilter.transform.InverseTransformPoint(contacts[i].point);
				float sqrMagnitude = (array[j] - vector).sqrMagnitude;
				if (!(sqrMagnitude > num2))
				{
					Vector3 vector2 = meshFilter.transform.InverseTransformDirection(contacts[i].normal * (1f - sqrMagnitude / num2) * num2);
					if (map != null)
					{
						vector2 *= 1f - map[num3];
					}
					array[j] += vector2;
					if (!(MaxVertexMov <= 0f))
					{
						float maxVertexMov = MaxVertexMov;
						vector2 = array[j] - baseVertices[num3];
						sqrMagnitude = vector2.magnitude;
						if (!(sqrMagnitude <= maxVertexMov))
						{
							array[j] = baseVertices[num3] + vector2 * (maxVertexMov / sqrMagnitude);
						}
						if (colors.Length > 0)
						{
							sqrMagnitude /= MaxVertexMov;
							colors[num3] = Color.Lerp(baseColors[num3], DeformedVertexColor, sqrMagnitude);
						}
					}
					else if (colors.Length > 0)
					{
						colors[num3] = Color.Lerp(baseColors[num3], DeformedVertexColor, (array[j] - baseVertices[num3]).magnitude / (num2 * 10f));
					}
				}
				num3++;
			}
		}
		RequestUpdateMesh();
	}

	public virtual void Repair(float repair)
	{
		Repair(repair, Vector3.zero, 0f);
	}

	public virtual void Repair(float repair, Vector3 point, float radius)
	{
		if (!mesh || !meshFilter)
		{
			return;
		}
		point = meshFilter.transform.InverseTransformPoint(point);
		int num = 0;
		int i = 0;
		Vector3[] array = vertices;
		for (int length = array.Length; i < length; i++)
		{
			try
			{
				if (radius <= 0f || (point - array[i]).magnitude < radius)
				{
					Vector3 vector = array[i] - baseVertices[num];
					array[i] = baseVertices[num] + vector * (1f - repair);
					if (colors.Length > 0)
					{
						colors[num] = Color.Lerp(colors[num], baseColors[num], repair);
					}
				}
			}
			finally
			{
				num++;
			}
		}
		RequestUpdateMesh();
	}

	private void RequestUpdateMesh()
	{
		if (UpdateFrequency == 0f)
		{
			UpdateMesh();
		}
		else
		{
			meshUpdate = true;
		}
	}

	private void UpdateMesh()
	{
		mesh.vertices = vertices;
		if (colors.Length > 0)
		{
			mesh.colors32 = colors;
		}
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if ((bool)meshCollider && DeformMeshCollider)
		{
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		lastUpdate = Time.time;
		meshUpdate = false;
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		Deform(collision);
	}

	public virtual void OnCollisionStay(Collision collision)
	{
		Deform(collision);
	}

	public virtual void FixedUpdate()
	{
		if (((bool)meshFilter && mesh != meshFilter.mesh) || (!meshFilter && (bool)mesh))
		{
			LoadMesh();
		}
		if (HardnessMap != appliedMap)
		{
			LoadMap();
		}
		if (meshUpdate && !(Time.time - lastUpdate < 1f / UpdateFrequency))
		{
			UpdateMesh();
		}
	}

	public virtual void Main()
	{
	}
}
