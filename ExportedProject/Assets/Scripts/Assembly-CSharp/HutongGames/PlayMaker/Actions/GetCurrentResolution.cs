using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the current resolution")]
	[ActionCategory(ActionCategory.Application)]
	public class GetCurrentResolution : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The current resolution width")]
		public FsmFloat width;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current resolution height")]
		public FsmFloat height;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current resolution refrehs rate")]
		public FsmFloat refreshRate;

		[Tooltip("The current resolution ( width, height, refreshRate )")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 currentResolution;

		public override void Reset()
		{
			width = null;
			height = null;
			refreshRate = null;
			currentResolution = null;
		}

		public override void OnEnter()
		{
			width.Value = Screen.currentResolution.width;
			height.Value = Screen.currentResolution.height;
			refreshRate.Value = Screen.currentResolution.refreshRate;
			currentResolution.Value = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, Screen.currentResolution.refreshRate);
			Finish();
		}
	}
}
