using SWS;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Recalculates waypoints of a bezier path in case it moved. Note that this is a performance-heavy action.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_UpdateBezierPath : FsmStateAction
	{
		[Tooltip("Define path name OR path object")]
		[UIHint(UIHint.FsmString)]
		public FsmString pathName;

		[ObjectType(typeof(BezierPathManager))]
		[Tooltip("Define path name OR path object")]
		public FsmObject pathObject;

		[Tooltip("Update per frame")]
		[UIHint(UIHint.FsmBool)]
		public bool everyFrame;

		public override void Reset()
		{
			pathName = null;
			pathObject = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			Execute();
			Finish();
		}

		private void Execute()
		{
			PathManager value = null;
			if (pathName.Value != string.Empty)
			{
				WaypointManager.Paths.TryGetValue(pathName.Value, out value);
			}
			else if (pathObject != null)
			{
				value = pathObject.Value as BezierPathManager;
			}
			if (!(value == null))
			{
				(value as BezierPathManager).CalculatePath();
			}
		}
	}
}
