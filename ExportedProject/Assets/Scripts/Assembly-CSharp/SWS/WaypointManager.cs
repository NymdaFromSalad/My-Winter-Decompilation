using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

namespace SWS
{
	public class WaypointManager : MonoBehaviour
	{
		public static readonly Dictionary<string, PathManager> Paths = new Dictionary<string, PathManager>();

		private void Awake()
		{
			foreach (Transform item in base.transform)
			{
				AddPath(item.gameObject);
			}
			HOTween.Init(true, true, true);
			HOTween.EnableOverwriteManager();
			HOTween.showPathGizmos = true;
			HOTween.warningLevel = WarningLevel.Important;
		}

		public static void AddPath(GameObject path)
		{
			if (path.name.Contains("(Clone)"))
			{
				path.name = path.name.Replace("(Clone)", string.Empty);
			}
			if (Paths.ContainsKey(path.name))
			{
				Debug.LogWarning("Called AddPath() but Scene already contains Path " + path.name + ".");
				return;
			}
			PathManager componentInChildren = path.GetComponentInChildren<PathManager>();
			if ((bool)componentInChildren)
			{
				Paths.Add(path.name, componentInChildren);
			}
			else
			{
				Debug.LogWarning("Called AddPath() but Transform " + path.name + " has no Path Component attached.");
			}
		}

		private void OnDestroy()
		{
			Paths.Clear();
		}

		public static void DrawStraight(Vector3[] waypoints)
		{
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
			}
		}

		public static void DrawCurved(Vector3[] waypoints)
		{
			Vector3[] array = new Vector3[waypoints.Length + 2];
			waypoints.CopyTo(array, 1);
			array[0] = waypoints[1];
			array[array.Length - 1] = array[array.Length - 2];
			int num = array.Length * 10;
			Vector3[] array2 = new Vector3[num + 1];
			for (int i = 0; i <= num; i++)
			{
				float t = (float)i / (float)num;
				Vector3 point = GetPoint(array, t);
				array2[i] = point;
			}
			Vector3 to = array2[0];
			for (int j = 1; j < array2.Length; j++)
			{
				Vector3 point = array2[j];
				Gizmos.DrawLine(point, to);
				to = point;
			}
		}

		public static Vector3 GetPoint(Vector3[] gizmoPoints, float t)
		{
			int num = gizmoPoints.Length - 3;
			int num2 = (int)Mathf.Floor(t * (float)num);
			int num3 = num - 1;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = t * (float)num - (float)num3;
			Vector3 vector = gizmoPoints[num3];
			Vector3 vector2 = gizmoPoints[num3 + 1];
			Vector3 vector3 = gizmoPoints[num3 + 2];
			Vector3 vector4 = gizmoPoints[num3 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num4 * num4 * num4) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num4 * num4) + (-vector + vector3) * num4 + 2f * vector2);
		}

		public static float GetPathLength(Vector3[] waypoints)
		{
			float num = 0f;
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				num += Vector3.Distance(waypoints[i], waypoints[i + 1]);
			}
			return num;
		}

		public static List<Vector3> SmoothCurve(List<Vector3> pathToCurve, int interpolations)
		{
			int num = 0;
			int num2 = 0;
			if (interpolations < 1)
			{
				interpolations = 1;
			}
			num = pathToCurve.Count;
			num2 = num * Mathf.RoundToInt(interpolations) - 1;
			List<Vector3> list = new List<Vector3>(num2);
			float num3 = 0f;
			for (int i = 0; i < num2 + 1; i++)
			{
				num3 = Mathf.InverseLerp(0f, num2, i);
				List<Vector3> list2 = new List<Vector3>(pathToCurve);
				for (int num4 = num - 1; num4 > 0; num4--)
				{
					for (int j = 0; j < num4; j++)
					{
						list2[j] = (1f - num3) * list2[j] + num3 * list2[j + 1];
					}
				}
				list.Add(list2[0]);
			}
			return list;
		}
	}
}
