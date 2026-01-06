namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads external cInput settings from a string.")]
	[ActionCategory("cInput")]
	public class CInputLoadExternal : FsmStateAction
	{
		[Tooltip("The string containing all the cInput settings.")]
		public FsmString cInputSettingsString;

		public override void OnEnter()
		{
			cInput.LoadExternal(cInputSettingsString.Value);
			Finish();
		}
	}
}
