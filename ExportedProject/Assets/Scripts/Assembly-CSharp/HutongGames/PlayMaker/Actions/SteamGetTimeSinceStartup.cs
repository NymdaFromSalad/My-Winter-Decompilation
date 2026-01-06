using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s time since startup.")]
	public class SteamGetTimeSinceStartup : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt time;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			time = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GetTime();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			GetTime();
		}

		public void GetTime()
		{
			string s = SteamUtils.GetSecondsSinceAppActive().ToString();
			time.Value = int.Parse(s);
		}
	}
}
