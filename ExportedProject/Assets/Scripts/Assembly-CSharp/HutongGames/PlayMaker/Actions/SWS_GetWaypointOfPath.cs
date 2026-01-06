using SWS;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the desired waypoint of a bezier path.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_GetWaypointOfPath : FsmStateAction
	{
		[ObjectType(typeof(PathManager))]
		[RequiredField]
		[Tooltip("Path manager component")]
		public FsmObject pathObject;

		[Tooltip("Waypoint index")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt wpIndex;

		[Tooltip("Waypoint gameobject")]
		[UIHint(UIHint.FsmGameObject)]
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
			if (wpIndex.Value > pathManager.waypoints.Length - 1)
			{
				wpIndex.Value = pathManager.waypoints.Length - 1;
			}
			if (pathManager is BezierPathManager)
			{
				waypoint.Value = (pathManager as BezierPathManager).bPoints[wpIndex.Value].wp.gameObject;
			}
			else
			{
				waypoint.Value = pathManager.waypoints[wpIndex.Value].gameObject;
			}
		}
	}
}
