using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the target frame rate")]
	[ActionCategory(ActionCategory.Application)]
	public class TargetFrameRate : FsmStateAction
	{
		[Tooltip("The target frame rate")]
		[RequiredField]
		public FsmInt targetFrameRate;

		public override void Reset()
		{
			targetFrameRate = 30;
		}

		public override void OnEnter()
		{
			Application.targetFrameRate = targetFrameRate.Value;
			Finish();
		}
	}
}
