using System.Collections.Generic;
using UnityEngine;

public class SubHull
{
	public List<Vector3> m_Vertices;

	public List<Vector3> m_Normals;

	public List<Vector4> m_Tangents;

	public List<Vector2> m_Uvs;

	public List<int> m_Triangles;

	public HashSet<int> m_EdgeVertexIndices;

	public Dictionary<int, List<int>> m_EdgeVertexIndexToOtherEdgeVertexIndices;

	public SubHull()
	{
		m_Vertices = new List<Vector3>();
		m_Normals = new List<Vector3>();
		m_Tangents = new List<Vector4>();
		m_Uvs = new List<Vector2>();
		m_Triangles = new List<int>();
		m_EdgeVertexIndices = new HashSet<int>();
		m_EdgeVertexIndexToOtherEdgeVertexIndices = new Dictionary<int, List<int>>();
	}

	public void CalculateEdges()
	{
		if (m_EdgeVertexIndices == null || m_EdgeVertexIndices.Count == 0)
		{
			return;
		}
		for (int i = 0; i < m_Triangles.Count; i += 3)
		{
			int num = m_Triangles[i];
			int num2 = m_Triangles[i + 1];
			int num3 = m_Triangles[i + 2];
			if (!m_EdgeVertexIndices.Contains(num) && !m_EdgeVertexIndices.Contains(num2) && !m_EdgeVertexIndices.Contains(num3))
			{
				continue;
			}
			if (m_EdgeVertexIndices.Contains(num))
			{
				if (!m_EdgeVertexIndexToOtherEdgeVertexIndices.ContainsKey(num))
				{
					m_EdgeVertexIndexToOtherEdgeVertexIndices.Add(num, new List<int>());
				}
				List<int> list = m_EdgeVertexIndexToOtherEdgeVertexIndices[num];
				if (m_EdgeVertexIndices.Contains(num2))
				{
					list.Add(num2);
				}
				if (m_EdgeVertexIndices.Contains(num3))
				{
					list.Add(num3);
				}
			}
			if (m_EdgeVertexIndices.Contains(num2))
			{
				if (!m_EdgeVertexIndexToOtherEdgeVertexIndices.ContainsKey(num2))
				{
					m_EdgeVertexIndexToOtherEdgeVertexIndices.Add(num2, new List<int>());
				}
				List<int> list2 = m_EdgeVertexIndexToOtherEdgeVertexIndices[num2];
				if (m_EdgeVertexIndices.Contains(num))
				{
					list2.Add(num);
				}
				if (m_EdgeVertexIndices.Contains(num3))
				{
					list2.Add(num3);
				}
			}
			if (m_EdgeVertexIndices.Contains(num3))
			{
				if (!m_EdgeVertexIndexToOtherEdgeVertexIndices.ContainsKey(num3))
				{
					m_EdgeVertexIndexToOtherEdgeVertexIndices.Add(num3, new List<int>());
				}
				List<int> list3 = m_EdgeVertexIndexToOtherEdgeVertexIndices[num3];
				if (m_EdgeVertexIndices.Contains(num))
				{
					list3.Add(num);
				}
				if (m_EdgeVertexIndices.Contains(num2))
				{
					list3.Add(num2);
				}
			}
		}
	}

	public bool IsEmpty()
	{
		return m_Vertices.Count < 3 || m_Triangles.Count < 3;
	}

	public Mesh GetMesh()
	{
		if (!IsEmpty())
		{
			Mesh mesh = new Mesh();
			mesh.vertices = m_Vertices.ToArray();
			mesh.triangles = m_Triangles.ToArray();
			if (m_Normals != null)
			{
				mesh.normals = m_Normals.ToArray();
			}
			if (m_Tangents != null)
			{
				mesh.tangents = m_Tangents.ToArray();
			}
			if (m_Uvs != null)
			{
				mesh.uv = m_Uvs.ToArray();
			}
			mesh.RecalculateBounds();
			return mesh;
		}
		return null;
	}
}
