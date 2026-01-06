namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clears all data stored by cInput from PlayerPrefs.")]
	[ActionCategory("cInput")]
	public class CInputClear : FsmStateAction
	{
		public override void OnEnter()
		{
			cInput.Clear();
			Finish();
		}
	}
}
