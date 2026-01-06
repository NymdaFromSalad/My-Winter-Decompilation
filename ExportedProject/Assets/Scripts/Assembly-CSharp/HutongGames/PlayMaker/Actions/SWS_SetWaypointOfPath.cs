using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Simple Waypoint System")]
	[Tooltip("Sets the desired waypoint of a path.")]
	public class SWS_SetWaypointOfPath : FsmStateAction
	{
		[ObjectType(typeof(PathManager))]
		[Tooltip("Path manager component")]
		[RequiredField]
		public FsmObject pathObject;

		[Tooltip("Waypoint index")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt wpIndex;

		[UIHint(UIHint.FsmGameObject)]
		[Tooltip("Waypoint gameobject")]
		public FsmGameObject waypoint;

		public override void Reset()
		{
			pathObject = null;
			wpIndex = null;
			waypoint = null;
		}

		public override void OnEnter()
		{
			Execute();
			Finish();
		}

		private void Execute()
		{
			PathManager pathManager = pathObject.Value as PathManager;
			if (wpIndex.Value >= pathManager.waypoints.Length - 1)
			{
				wpIndex.Value = pathManager.waypoints.Length - 1;
			}
			else if (wpIndex.Value <= 0)
			{
				wpIndex.Value = 0;
			}
			waypoint.Value.name = "Waypoint " + wpIndex.Value;
			waypoint.Value.transform.parent = pathManager.transform;
			Transform transform = null;
			if (pathManager is BezierPathManager)
			{
				transform = (pathManager as BezierPathManager).bPoints[wpIndex.Value].wp;
				(pathManager as BezierPathManager).bPoints[wpIndex.Value].wp = waypoint.Value.transform;
			}
			else
			{
				transform = pathManager.waypoints[wpIndex.Value];
				pathManager.waypoints[wpIndex.Value] = waypoint.Value.transform;
			}
			Object.Destroy(transform.gameObject);
		}
	}
}
