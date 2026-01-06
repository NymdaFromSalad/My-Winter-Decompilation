using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Assigns a random path of the list to the walker object and starts movement.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_SetPathRandom : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		[Tooltip("Path array")]
		public PathManager[] paths;

		public override void Reset()
		{
			walkerObject = null;
			paths = null;
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
				int num = Random.Range(0, paths.Length);
				PathManager pathManager = paths[num];
				if (pathManager is PathManager)
				{
					ownerDefaultTarget.SendMessage("SetPath", pathManager);
				}
				else
				{
					ownerDefaultTarget.SendMessage("SetPath", pathManager as BezierPathManager);
				}
			}
		}
	}
}
