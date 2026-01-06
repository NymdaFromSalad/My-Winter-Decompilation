namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Sets a cInput key.")]
	public class CInputSetKey : FsmStateAction
	{
		[Tooltip("The name of the action name.")]
		public FsmString actionName;

		[Tooltip("The name of the primary key.")]
		public FsmString primary;

		[Tooltip("The name of the secondary key.")]
		public FsmString secondary;

		[Tooltip("The name of the primary modifier key.")]
		public FsmString primaryModifier;

		[Tooltip("The name of the secondary modifier key.")]
		public FsmString secondaryModifier;

		public override void Reset()
		{
			actionName = string.Empty;
			primary = null;
			secondary = null;
			primaryModifier = null;
			secondaryModifier = null;
		}

		public override void OnEnter()
		{
			cInput.SetKey(actionName.Value, primary.Value, secondary.Value, primaryModifier.Value, secondaryModifier.Value);
			Finish();
		}
	}
}
