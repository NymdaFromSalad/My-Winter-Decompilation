using UnityEngine;

namespace SWS
{
	public class PathManager : MonoBehaviour
	{
		public Transform[] waypoints;

		public bool drawCurved = true;

		public Color color1 = new Color(1f, 0f, 1f, 0.5f);

		public Color color2 = new Color(1f, 47f / 51f, 0.015686275f, 0.5f);

		public Vector3 size = new Vector3(0.7f, 0.7f, 0.7f);

		public float radius = 0.4f;

		public GameObject replaceObject;

		private void OnDrawGizmos()
		{
			if (waypoints.Length > 0)
			{
				Vector3[] pathPoints = GetPathPoints();
				Vector3 vector = pathPoints[0];
				Vector3 vector2 = pathPoints[pathPoints.Length - 1];
				Gizmos.color = color1;
				Gizmos.DrawWireCube(vector, size * GetHandleSize(vector));
				Gizmos.DrawWireCube(vector2, size * GetHandleSize(vector2));
				Gizmos.color = color2;
				for (int i = 1; i < pathPoints.Length - 1; i++)
				{
					Gizmos.DrawWireSphere(pathPoints[i], radius * GetHandleSize(pathPoints[i]));
				}
				if (drawCurved && pathPoints.Length >= 2)
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

		public virtual Vector3[] GetPathPoints()
		{
			Vector3[] array = new Vector3[waypoints.Length];
			for (int i = 0; i < waypoints.Length; i++)
			{
				array[i] = waypoints[i].position;
			}
			return array;
		}
	}
}
