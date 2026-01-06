using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the path of a walker object and starts movement. Define path name OR path object.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_SetPath : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Walker object")]
		public FsmOwnerDefault walkerObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Define path name OR path object")]
		public FsmString pathName;

		[Tooltip("Define path name OR path object")]
		[ObjectType(typeof(PathManager))]
		public FsmObject pathObject;

		public override void Reset()
		{
			walkerObject = null;
			pathObject = null;
			pathName = null;
		}

		public override void OnEnter()
		{
			Execute();
			Finish();
		}

		private void Execute()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(walkerObject);
			if (!(ownerDefaultTarget == null))
			{
				if (pathName.Value != string.Empty)
				{
					ownerDefaultTarget.SendMessage("SetPath", WaypointManager.Paths[pathName.Value]);
				}
				else if (pathObject != null)
				{
					ownerDefaultTarget.SendMessage("SetPath", pathObject.Value as PathManager);
				}
			}
		}
	}
}
