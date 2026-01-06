using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SWS
{
	public class BezierPathManager : PathManager
	{
		public Vector3[] pathPoints;

		public List<BezierPoint> bPoints;

		public bool showHandles = true;

		public Color color3 = new Color(36f / 85f, 0.5921569f, 1f, 1f);

		public float pathDetail = 1f;

		public bool customDetail;

		public List<float> segmentDetail = new List<float>();

		private void Awake()
		{
			CalculatePath();
		}

		private void OnDrawGizmos()
		{
			if (bPoints.Count > 0)
			{
				Vector3 position = bPoints[0].wp.position;
				Vector3 position2 = bPoints[bPoints.Count - 1].wp.position;
				Gizmos.color = color1;
				Gizmos.DrawWireCube(position, size * GetHandleSize(position));
				Gizmos.DrawWireCube(position2, size * GetHandleSize(position2));
				Gizmos.color = color2;
				for (int i = 1; i < bPoints.Count - 1; i++)
				{
					Gizmos.DrawWireSphere(bPoints[i].wp.position, radius * GetHandleSize(bPoints[i].wp.position));
				}
				if (drawCurved && bPoints.Count >= 2)
				{
					WaypointManager.DrawCurved(pathPoints);
				}
				else
				{
					WaypointManager.DrawStraight(pathPoints);
				}
			}
		}

		private float GetHandleSize(Vector3 pos)
		{
			return 1f;
		}

		public override Vector3[] GetPathPoints()
		{
			return pathPoints;
		}

		public void CalculatePath()
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < bPoints.Count - 1; i++)
			{
				BezierPoint bezierPoint = bPoints[i];
				float detail = pathDetail;
				if (customDetail)
				{
					detail = segmentDetail[i];
				}
				list.AddRange(GetPoints(bezierPoint.wp.position, bezierPoint.cp[1].position, bPoints[i + 1].cp[0].position, bPoints[i + 1].wp.position, detail));
			}
			pathPoints = list.Distinct().ToArray();
		}

		private List<Vector3> GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float detail)
		{
			List<Vector3> list = new List<Vector3>();
			float num = detail * 10f;
			for (int i = 0; (float)i <= num; i++)
			{
				float num2 = (float)i / num;
				float num3 = 1f - num2;
				Vector3 zero = Vector3.zero;
				zero += p0 * num3 * num3 * num3;
				zero += p1 * num2 * 3f * num3 * num3;
				zero += p2 * 3f * num2 * num2 * num3;
				zero += p3 * num2 * num2 * num2;
				list.Add(zero);
			}
			return list;
		}
	}
}
