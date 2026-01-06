namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Removes a cInput modifier key.")]
	public class CInputRemoveModifierString : FsmStateAction
	{
		[Tooltip("The modifier key to add.")]
		public FsmString keyName;

		public override void Reset()
		{
			keyName = null;
		}

		public override void OnEnter()
		{
			cInput.RemoveModifier(keyName.Value);
			Finish();
		}
	}
}
