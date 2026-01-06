namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Initialize cInput to work with. You have to execute this first!")]
	[ActionCategory("cInput")]
	public class CInputInit : FsmStateAction
	{
		public override void OnEnter()
		{
			cInput.Init();
			Finish();
		}
	}
}
