using UnityEngine;

public class Deformable : MonoBehaviour
{
	public MeshFilter meshFilter;

	public float Hardness = 0.5f;

	public bool DeformMeshCollider = true;

	public float UpdateFrequency;

	public float MaxVertexMov;

	public Color32 DeformedVertexColor = Color.gray;

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

	private void Awake()
	{
		meshCollider = GetComponent<MeshCollider>();
		if (!meshFilter)
		{
			meshFilter = GetComponent<MeshFilter>();
		}
		if ((bool)meshFilter)
		{
			LoadMesh();
		}
		else
		{
			Debug.LogWarning("Deformable component warning: No mesh filter assigned for object " + base.gameObject.ToString());
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
			Debug.LogWarning("Deformable component warning: Mesh at mesh filter is null " + base.gameObject.ToString());
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
			map = new float[uv.Length];
			int num = 0;
			Vector2[] array = uv;
			for (int i = 0; i < array.Length; i++)
			{
				Vector2 vector = array[i];
				try
				{
					map[num] = HardnessMap.GetPixelBilinear(vector.x, vector.y).a;
				}
				catch
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
		if ((double)num < 0.01)
		{
			return;
		}
		float num2 = num * (sizeFactor * (0.1f / Mathf.Max(0.1f, Hardness)));
		ContactPoint[] contacts = collision.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 vector = meshFilter.transform.InverseTransformPoint(contactPoint.point);
				float sqrMagnitude = (vertices[j] - vector).sqrMagnitude;
				if (!(sqrMagnitude <= num2))
				{
					continue;
				}
				Vector3 vector2 = meshFilter.transform.InverseTransformDirection(contactPoint.normal * (1f - sqrMagnitude / num2) * num2);
				if (map != null)
				{
					vector2 *= 1f - map[j];
				}
				vertices[j] += vector2;
				if (MaxVertexMov > 0f)
				{
					float maxVertexMov = MaxVertexMov;
					vector2 = vertices[j] - baseVertices[j];
					sqrMagnitude = vector2.magnitude;
					if (sqrMagnitude > maxVertexMov)
					{
						vertices[j] = baseVertices[j] + vector2 * (maxVertexMov / sqrMagnitude);
					}
					if (colors.Length > 0)
					{
						sqrMagnitude /= MaxVertexMov;
						colors[j] = Color.Lerp(baseColors[j], DeformedVertexColor, sqrMagnitude);
					}
				}
				else if (colors.Length > 0)
				{
					colors[j] = Color.Lerp(baseColors[j], DeformedVertexColor, (vertices[j] - baseVertices[j]).magnitude / (num2 * 10f));
				}
			}
		}
		RequestUpdateMesh();
	}

	public void Repair(float repair)
	{
		Repair(repair, Vector3.zero, 0f);
	}

	public void Repair(float repair, Vector3 point, float radius)
	{
		if (!mesh || !meshFilter)
		{
			return;
		}
		point = meshFilter.transform.InverseTransformPoint(point);
		int num = 0;
		Vector3[] array = vertices;
		foreach (Vector3 vector in array)
		{
			try
			{
				if (!(radius > 0f) || !((point - vector).magnitude >= radius))
				{
					Vector3 vector2 = vector - baseVertices[num];
					vertices[num] = baseVertices[num] + vector2 * (1f - repair);
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
		mesh.RecalculateBounds();
		if ((bool)meshCollider && DeformMeshCollider)
		{
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		lastUpdate = Time.time;
		meshUpdate = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer != 19 && collision.gameObject.layer != 20)
		{
			Deform(collision);
		}
	}

	private void FixedUpdate()
	{
		if (((bool)meshFilter && mesh != meshFilter.mesh) || (!meshFilter && (bool)mesh))
		{
			LoadMesh();
		}
		if (HardnessMap != appliedMap)
		{
			LoadMap();
		}
		if (meshUpdate && Time.time - lastUpdate >= 1f / UpdateFrequency)
		{
			UpdateMesh();
		}
	}
}
