using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the pressed state of the specified Mouse Button and stores it in a Bool Variable. See Unity Input Manager doc.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButton : FsmStateAction
	{
		[RequiredField]
		public MouseButton button;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;

		public override void Reset()
		{
			button = MouseButton.Left;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			storeResult.Value = Input.GetMouseButton((int)button);
		}
	}
}
