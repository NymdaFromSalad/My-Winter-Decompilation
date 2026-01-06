using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the resolution")]
	[ActionCategory(ActionCategory.Application)]
	public class ScreenSetResolution : FsmStateAction
	{
		[Tooltip("Full Screen mode")]
		public FsmBool fullScreen;

		[Tooltip("The resolution width")]
		public FsmInt width;

		[Tooltip("The resolution height")]
		public FsmInt height;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current resolution refresh rate")]
		public FsmInt preferedRefreshRate;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current resolution ( width, height, refreshRate )")]
		public FsmVector3 orResolution;

		public override void Reset()
		{
			width = null;
			height = null;
			preferedRefreshRate = new FsmInt();
			preferedRefreshRate.UseVariable = true;
			orResolution = null;
			fullScreen = null;
		}

		public override void OnEnter()
		{
			if (!orResolution.IsNone)
			{
				if (preferedRefreshRate.IsNone)
				{
					Screen.SetResolution((int)orResolution.Value.x, (int)orResolution.Value.y, fullScreen.Value);
				}
				else
				{
					Screen.SetResolution((int)orResolution.Value.x, (int)orResolution.Value.y, fullScreen.Value, (int)orResolution.Value.z);
				}
			}
			else if (preferedRefreshRate.IsNone)
			{
				Screen.SetResolution(width.Value, height.Value, fullScreen.Value);
			}
			else
			{
				Screen.SetResolution(width.Value, height.Value, fullScreen.Value, preferedRefreshRate.Value);
			}
			Finish();
		}
	}
}
