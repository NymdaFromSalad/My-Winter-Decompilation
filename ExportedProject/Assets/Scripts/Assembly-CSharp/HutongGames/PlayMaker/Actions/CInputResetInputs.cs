namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Resets all cInput settings; keys, axis, modifier keys etc.")]
	public class CInputResetInputs : FsmStateAction
	{
		public override void OnEnter()
		{
			cInput.ResetInputs();
			Finish();
		}
	}
}
