namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a modifier key to cInput keys.")]
	[ActionCategory("cInput")]
	public class CInputAddModifierString : FsmStateAction
	{
		[Tooltip("The modifier key to add.")]
		public FsmString keyName;

		public override void Reset()
		{
			keyName = null;
		}

		public override void OnEnter()
		{
			cInput.AddModifier(keyName.Value);
			Finish();
		}
	}
}
