namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Calibrates cInput.")]
	public class CInputCalibrate : FsmStateAction
	{
		public override void OnEnter()
		{
			cInput.Calibrate();
			Finish();
		}
	}
}
