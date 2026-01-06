using System.Collections.Generic;
using UnityEngine;

public class Hull
{
	private const int c_MinTrianglesPerImpact = 48;

	private const float c_CompressionResistance = 0.95f;

	private List<Vector3> m_Vertices;

	private List<Vector3> m_Normals;

	private List<Vector4> m_Tangents;

	private List<Vector2> m_Uvs;

	private List<int> m_Triangles;

	private SubHull m_FirstSubHull;

	private SubHull m_SecondSubHull;

	public Hull(Mesh mesh)
	{
		m_Vertices = new List<Vector3>(mesh.vertices);
		m_Triangles = new List<int>(mesh.triangles);
		if (mesh.normals.Length > 0)
		{
			m_Normals = new List<Vector3>(mesh.normals);
		}
		if (mesh.tangents.Length > 0)
		{
			m_Tangents = new List<Vector4>(mesh.tangents);
		}
		if (mesh.uv.Length > 0)
		{
			m_Uvs = new List<Vector2>(mesh.uv);
		}
	}

	public void Impact(Vector3 impactPoint, Vector3 impactForce, Meshinator.ImpactShapes impactShape, Meshinator.ImpactTypes impactType)
	{
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		foreach (int intersectedTriangleIndex in GetIntersectedTriangleIndices(impactPoint, impactForce.magnitude))
		{
			float areaOfTriangle = GetAreaOfTriangle(intersectedTriangleIndex);
			dictionary.Add(intersectedTriangleIndex, areaOfTriangle);
		}
		while (dictionary.Keys.Count < 48 && m_Vertices.Count <= 64988)
		{
			int indexOfLargestTriangle = GetIndexOfLargestTriangle(dictionary);
			List<int> list = BreakDownTriangle(indexOfLargestTriangle);
			dictionary.Remove(indexOfLargestTriangle);
			foreach (int item in list)
			{
				if (IsTriangleIndexIntersected(item, impactPoint, impactForce.magnitude))
				{
					float areaOfTriangle2 = GetAreaOfTriangle(item);
					dictionary.Add(item, areaOfTriangle2);
				}
			}
		}
		AdjustVerticesForImpact(impactPoint, impactForce, impactShape, impactType);
	}

	private void AdjustVerticesForImpact(Vector3 impactPoint, Vector3 impactForce, Meshinator.ImpactShapes impactShape, Meshinator.ImpactTypes impactType)
	{
		float magnitude = impactForce.magnitude;
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		for (int i = 0; i < m_Vertices.Count; i++)
		{
			Vector3 vector = m_Vertices[i];
			Vector3 vector2 = Vector3.zero;
			switch (impactShape)
			{
			case Meshinator.ImpactShapes.FlatImpact:
				vector2 = Vector3.Project(vector - impactPoint, impactForce.normalized);
				break;
			case Meshinator.ImpactShapes.SphericalImpact:
				vector2 = vector - impactPoint;
				break;
			}
			float num = vector2.magnitude;
			if (impactShape == Meshinator.ImpactShapes.FlatImpact && Vector3.Angle(vector - impactPoint, impactForce) > 90f)
			{
				num = 0f - num;
			}
			float num2 = Mathf.Max(0f, magnitude - num) * 0.95f;
			if (num < magnitude && num2 > 0f)
			{
				dictionary.Add(i, num2);
			}
		}
		switch (impactType)
		{
		case Meshinator.ImpactTypes.Compression:
			CompressMeshVertices(dictionary, impactForce);
			break;
		case Meshinator.ImpactTypes.Fracture:
			FractureMeshVertices(dictionary, impactForce);
			break;
		}
	}

	private void CompressMeshVertices(Dictionary<int, float> movedVertexToForceMagnitudeMap, Vector3 impactForce)
	{
		foreach (int key in movedVertexToForceMagnitudeMap.Keys)
		{
			Vector3 vector = m_Vertices[key];
			float num = movedVertexToForceMagnitudeMap[key];
			Vector3 vector2 = impactForce.normalized * num;
			m_Vertices[key] = vector + vector2;
		}
	}

	private void FractureMeshVertices(Dictionary<int, float> movedVertexToForceMagnitudeMap, Vector3 impactForce)
	{
		if (movedVertexToForceMagnitudeMap.Keys.Count < m_Vertices.Count)
		{
			SetupSubHulls(movedVertexToForceMagnitudeMap);
			FixUpSubHulls(impactForce);
			m_Vertices = m_FirstSubHull.m_Vertices;
			m_Normals = m_FirstSubHull.m_Normals;
			m_Tangents = m_FirstSubHull.m_Tangents;
			m_Uvs = m_FirstSubHull.m_Uvs;
			m_Triangles = m_FirstSubHull.m_Triangles;
			m_FirstSubHull = null;
		}
	}

	public void SetupSubHulls(Dictionary<int, float> movedVertexToForceMagnitudeMap)
	{
		m_FirstSubHull = new SubHull();
		m_SecondSubHull = new SubHull();
		Dictionary<SubHull, Dictionary<int, int>> dictionary = new Dictionary<SubHull, Dictionary<int, int>>();
		for (int i = 0; i < m_Vertices.Count; i++)
		{
			SubHull subHull = m_FirstSubHull;
			if (movedVertexToForceMagnitudeMap.ContainsKey(i))
			{
				subHull = m_SecondSubHull;
			}
			subHull.m_Vertices.Add(m_Vertices[i]);
			if (m_Normals.Count > i)
			{
				subHull.m_Normals.Add(m_Normals[i]);
			}
			if (m_Tangents.Count > i)
			{
				subHull.m_Tangents.Add(m_Tangents[i]);
			}
			if (m_Uvs.Count > i)
			{
				subHull.m_Uvs.Add(m_Uvs[i]);
			}
			if (!dictionary.ContainsKey(subHull))
			{
				dictionary.Add(subHull, new Dictionary<int, int>());
			}
			dictionary[subHull].Add(i, subHull.m_Vertices.Count - 1);
		}
		Dictionary<Vector3, List<int>> dictionary2 = new Dictionary<Vector3, List<int>>();
		for (int j = 0; j < m_Vertices.Count; j++)
		{
			Vector3 key = m_Vertices[j];
			if (!dictionary2.ContainsKey(key))
			{
				dictionary2.Add(key, new List<int>());
			}
			List<int> list = dictionary2[key];
			list.Add(j);
		}
		for (int k = 0; k < m_Triangles.Count; k += 3)
		{
			List<int> list2 = new List<int>();
			for (int l = k; l < k + 3; l++)
			{
				if (movedVertexToForceMagnitudeMap.ContainsKey(m_Triangles[l]))
				{
					list2.Add(m_Triangles[l]);
				}
			}
			if (list2.Count != 3 && list2.Count != 0)
			{
				for (int m = k; m < k + 3; m++)
				{
					int num = m_Triangles[m];
					SubHull subHull2 = m_FirstSubHull;
					if (list2.Contains(num))
					{
						subHull2 = m_SecondSubHull;
					}
					Dictionary<int, int> dictionary3 = dictionary[subHull2];
					foreach (int item in dictionary2[m_Vertices[num]])
					{
						subHull2.m_EdgeVertexIndices.Add(dictionary3[item]);
					}
				}
				continue;
			}
			SubHull subHull3 = m_FirstSubHull;
			if (list2.Count == 3)
			{
				subHull3 = m_SecondSubHull;
			}
			for (int n = k; n < k + 3; n++)
			{
				int key2 = m_Triangles[n];
				Dictionary<int, int> dictionary4 = dictionary[subHull3];
				if (dictionary4.ContainsKey(key2))
				{
					subHull3.m_Triangles.Add(dictionary4[key2]);
				}
			}
		}
		m_FirstSubHull.CalculateEdges();
		m_SecondSubHull.CalculateEdges();
	}

	public void FixUpSubHulls(Vector3 impactForce)
	{
		for (int i = 0; i < 2; i++)
		{
			SubHull subHull = ((i != 0) ? m_SecondSubHull : m_FirstSubHull);
			Vector3 zero = Vector3.zero;
			Vector4 zero2 = Vector4.zero;
			Vector2 zero3 = Vector2.zero;
			foreach (int key in subHull.m_EdgeVertexIndexToOtherEdgeVertexIndices.Keys)
			{
				zero += subHull.m_Vertices[key];
				zero2 += subHull.m_Tangents[key];
				zero3 += subHull.m_Uvs[key];
			}
			int count = subHull.m_EdgeVertexIndexToOtherEdgeVertexIndices.Count;
			zero /= (float)count;
			zero2 /= (float)count;
			zero3 /= (float)count;
			subHull.m_Vertices.Add(zero);
			subHull.m_Tangents.Add(zero2);
			subHull.m_Uvs.Add(zero3);
			if (i == 0)
			{
				subHull.m_Normals.Add(-impactForce.normalized);
			}
			else
			{
				subHull.m_Normals.Add(impactForce.normalized);
			}
			foreach (int key2 in subHull.m_EdgeVertexIndexToOtherEdgeVertexIndices.Keys)
			{
				List<int> list = subHull.m_EdgeVertexIndexToOtherEdgeVertexIndices[key2];
				foreach (int item in list)
				{
					subHull.m_Triangles.Add(subHull.m_Vertices.Count - 1);
					subHull.m_Triangles.Add(item);
					subHull.m_Triangles.Add(key2);
				}
			}
		}
	}

	private List<int> GetIntersectedTriangleIndices(Vector3 impactPoint, float impactRadius)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < m_Triangles.Count; i += 3)
		{
			if (IsTriangleIndexIntersected(i, impactPoint, impactRadius))
			{
				list.Add(i);
			}
		}
		return list;
	}

	private bool IsTriangleIndexIntersected(int triangleIndex, Vector3 impactPoint, float impactRadius)
	{
		if (triangleIndex % 3 != 0)
		{
			Debug.LogError("Invalid Triangle index: " + triangleIndex + "  Must be a multiple of 3!");
			return false;
		}
		Vector3 vector = m_Vertices[m_Triangles[triangleIndex]] - impactPoint;
		Vector3 vector2 = m_Vertices[m_Triangles[triangleIndex + 1]] - impactPoint;
		Vector3 vector3 = m_Vertices[m_Triangles[triangleIndex + 2]] - impactPoint;
		float num = impactRadius * impactRadius;
		Vector3 vector4 = Vector3.Cross(vector2 - vector, vector3 - vector);
		float num2 = Vector3.Dot(vector, vector4);
		float num3 = Vector3.Dot(vector4, vector4);
		if (num2 * num2 > num * num3)
		{
			return false;
		}
		float num4 = Vector3.Dot(vector, vector);
		float num5 = Vector3.Dot(vector, vector2);
		float num6 = Vector3.Dot(vector, vector3);
		float num7 = Vector3.Dot(vector2, vector2);
		float num8 = Vector3.Dot(vector2, vector3);
		float num9 = Vector3.Dot(vector3, vector3);
		bool flag = num4 > num && num5 > num4 && num6 > num4;
		bool flag2 = num7 > num && num5 > num7 && num8 > num7;
		bool flag3 = num9 > num && num6 > num9 && num8 > num9;
		if (flag || flag2 || flag3)
		{
			return false;
		}
		Vector3 vector5 = vector2 - vector;
		Vector3 vector6 = vector3 - vector2;
		Vector3 vector7 = vector - vector3;
		float num10 = num5 - num4;
		float num11 = num8 - num7;
		float num12 = num6 - num9;
		float num13 = Vector3.Dot(vector5, vector5);
		float num14 = Vector3.Dot(vector6, vector6);
		float num15 = Vector3.Dot(vector7, vector7);
		Vector3 vector8 = vector5 * num13 - num10 * vector5;
		Vector3 vector9 = vector6 * num14 - num11 * vector6;
		Vector3 vector10 = vector7 * num15 - num12 * vector7;
		Vector3 rhs = vector3 * num13 - vector8;
		Vector3 rhs2 = vector * num14 - vector9;
		Vector3 rhs3 = vector2 * num15 - vector10;
		bool flag4 = Vector3.Dot(vector8, vector8) > num * num13 * num13 && Vector3.Dot(vector8, rhs) > 0f;
		bool flag5 = Vector3.Dot(vector9, vector9) > num * num14 * num14 && Vector3.Dot(vector9, rhs2) > 0f;
		bool flag6 = Vector3.Dot(vector10, vector10) > num * num15 * num15 && Vector3.Dot(vector10, rhs3) > 0f;
		if (flag4 || flag5 || flag6)
		{
			return false;
		}
		return true;
	}

	private List<int> BreakDownTriangle(int triangleIndex)
	{
		List<int> list = new List<int>();
		list.Add(triangleIndex);
		if (m_Vertices.Count > 64988)
		{
			return list;
		}
		int index = m_Triangles[triangleIndex];
		int index2 = m_Triangles[triangleIndex + 1];
		int index3 = m_Triangles[triangleIndex + 2];
		Vector3 vector = m_Vertices[index];
		Vector3 vector2 = m_Vertices[index2];
		Vector3 vector3 = m_Vertices[index3];
		Vector3 item = (vector + vector2) / 2f;
		Vector3 item2 = (vector + vector3) / 2f;
		Vector3 item3 = (vector2 + vector3) / 2f;
		m_Vertices.Add(item);
		m_Vertices.Add(item2);
		m_Triangles[triangleIndex + 1] = m_Vertices.Count - 2;
		m_Triangles[triangleIndex + 2] = m_Vertices.Count - 1;
		m_Vertices.Add(item2);
		m_Vertices.Add(item3);
		m_Vertices.Add(vector3);
		m_Triangles.Add(m_Vertices.Count - 2);
		m_Triangles.Add(m_Vertices.Count - 1);
		m_Triangles.Add(m_Vertices.Count - 3);
		list.Add(m_Triangles.Count - 3);
		m_Vertices.Add(item);
		m_Vertices.Add(item3);
		m_Vertices.Add(vector2);
		m_Triangles.Add(m_Vertices.Count - 1);
		m_Triangles.Add(m_Vertices.Count - 2);
		m_Triangles.Add(m_Vertices.Count - 3);
		list.Add(m_Triangles.Count - 3);
		m_Vertices.Add(item);
		m_Vertices.Add(item3);
		m_Vertices.Add(item2);
		m_Triangles.Add(m_Vertices.Count - 2);
		m_Triangles.Add(m_Vertices.Count - 1);
		m_Triangles.Add(m_Vertices.Count - 3);
		list.Add(m_Triangles.Count - 3);
		if (m_Normals.Count > 0)
		{
			Vector3 vector4 = m_Normals[index];
			Vector3 vector5 = m_Normals[index2];
			Vector3 vector6 = m_Normals[index3];
			Vector3 item4 = (vector4 + vector5) / 2f;
			Vector3 item5 = (vector4 + vector6) / 2f;
			Vector3 item6 = (vector5 + vector6) / 2f;
			m_Normals.Add(item4);
			m_Normals.Add(item5);
			m_Normals.Add(item5);
			m_Normals.Add(item6);
			m_Normals.Add(vector6);
			m_Normals.Add(item4);
			m_Normals.Add(item6);
			m_Normals.Add(vector5);
			m_Normals.Add(item4);
			m_Normals.Add(item6);
			m_Normals.Add(item5);
		}
		if (m_Tangents.Count > 0)
		{
			Vector4 vector7 = m_Tangents[index];
			Vector4 vector8 = m_Tangents[index2];
			Vector4 vector9 = m_Tangents[index3];
			Vector4 item7 = (vector7 + vector8) / 2f;
			Vector4 item8 = (vector7 + vector9) / 2f;
			Vector4 item9 = (vector8 + vector9) / 2f;
			m_Tangents.Add(item7);
			m_Tangents.Add(item8);
			m_Tangents.Add(item8);
			m_Tangents.Add(item9);
			m_Tangents.Add(vector9);
			m_Tangents.Add(item7);
			m_Tangents.Add(item9);
			m_Tangents.Add(vector8);
			m_Tangents.Add(item7);
			m_Tangents.Add(item9);
			m_Tangents.Add(item8);
		}
		if (m_Uvs.Count > 0)
		{
			Vector2 vector10 = m_Uvs[index];
			Vector2 vector11 = m_Uvs[index2];
			Vector2 vector12 = m_Uvs[index3];
			Vector2 item10 = (vector10 + vector11) / 2f;
			Vector2 item11 = (vector10 + vector12) / 2f;
			Vector2 item12 = (vector11 + vector12) / 2f;
			m_Uvs.Add(item10);
			m_Uvs.Add(item11);
			m_Uvs.Add(item11);
			m_Uvs.Add(item12);
			m_Uvs.Add(vector12);
			m_Uvs.Add(item10);
			m_Uvs.Add(item12);
			m_Uvs.Add(vector11);
			m_Uvs.Add(item10);
			m_Uvs.Add(item12);
			m_Uvs.Add(item11);
		}
		return list;
	}

	private float GetAreaOfTriangle(int triangleIndex)
	{
		Vector3 vector = m_Vertices[m_Triangles[triangleIndex]];
		Vector3 vector2 = m_Vertices[m_Triangles[triangleIndex + 1]];
		Vector3 vector3 = m_Vertices[m_Triangles[triangleIndex + 2]];
		return Vector3.Cross(vector - vector2, vector - vector3).magnitude * 0.5f;
	}

	private int GetIndexOfLargestTriangle(Dictionary<int, float> triangleIndexToTriangleArea)
	{
		int result = 0;
		float num = 0f;
		foreach (int key in triangleIndexToTriangleArea.Keys)
		{
			float num2 = triangleIndexToTriangleArea[key];
			if (num2 > num)
			{
				result = key;
				num = num2;
			}
		}
		return result;
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

	public Mesh GetSubHullMesh()
	{
		if (m_SecondSubHull != null && !m_SecondSubHull.IsEmpty())
		{
			return m_SecondSubHull.GetMesh();
		}
		return null;
	}
}
