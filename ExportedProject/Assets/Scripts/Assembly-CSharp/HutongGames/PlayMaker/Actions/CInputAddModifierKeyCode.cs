using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Adds a modifier key to cInput keys.")]
	public class CInputAddModifierKeyCode : FsmStateAction
	{
		[Tooltip("The modifier key to add.")]
		public KeyCode keyName;

		public override void Reset()
		{
			keyName = KeyCode.None;
		}

		public override void OnEnter()
		{
			cInput.AddModifier(keyName);
			Finish();
		}
	}
}
