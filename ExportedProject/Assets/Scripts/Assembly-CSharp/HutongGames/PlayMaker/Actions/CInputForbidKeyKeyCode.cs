using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Forbids a cInput key.")]
	public class CInputForbidKeyKeyCode : FsmStateAction
	{
		[Tooltip("The name of the key to forbid.")]
		public KeyCode keyName;

		public override void OnEnter()
		{
			cInput.ForbidKey(keyName);
			Finish();
		}
	}
}
