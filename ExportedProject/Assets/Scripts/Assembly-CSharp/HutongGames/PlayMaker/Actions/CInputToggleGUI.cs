namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Toggles the cInput test GUI on or off.")]
	public class CInputToggleGUI : FsmStateAction
	{
		public override void OnEnter()
		{
			cGUI.ToggleGUI();
			Finish();
		}
	}
}
