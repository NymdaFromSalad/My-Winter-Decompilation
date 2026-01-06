using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Changes the key mapping for cInput keys. If you press Escape the action finishes, because cInput doesn't allow to bind it.")]
	[ActionCategory("cInput")]
	public class CInputChangeKey : FsmStateAction
	{
		[Tooltip("The action you want to change the key for. If this field is empty, the index value will be taken.")]
		public FsmString actionName;

		[Tooltip("If Action Name is empty, the index will be taken.")]
		public FsmInt index;

		[Tooltip("change mapping for 1 = primary, 2 = secondary")]
		public FsmInt input;

		[Tooltip("If checked you are able to bind multiple actions to this key")]
		public FsmBool allowDuplicates;

		public FsmBool allowMouseAxis;

		public FsmBool allowMouseButtons;

		public FsmBool allowGamepadAxis;

		public FsmBool allowGamepadButtons;

		public FsmBool allowKeyboard;

		[Tooltip("Sends after a key has pressed.")]
		public FsmEvent keyChangedEvent;

		public override void OnEnter()
		{
			cInput.allowDuplicates = allowDuplicates.Value;
			if (string.IsNullOrEmpty(actionName.Value))
			{
				cInput.ChangeKey(index.Value, input.Value, allowMouseAxis.Value, allowMouseButtons.Value, allowGamepadAxis.Value, allowGamepadButtons.Value, allowKeyboard.Value);
			}
			else
			{
				cInput.ChangeKey(actionName.Value, input.Value, allowMouseAxis.Value, allowMouseButtons.Value, allowGamepadAxis.Value, allowGamepadButtons.Value, allowKeyboard.Value);
			}
		}

		public override void OnLateUpdate()
		{
			if (cInput.GetKey(actionName.Value))
			{
				base.Fsm.Event(keyChangedEvent);
				Finish();
			}
			else if (Input.GetKey(KeyCode.Escape))
			{
				Finish();
			}
		}

		public override void Reset()
		{
			actionName = null;
			index = null;
			input = null;
			allowDuplicates = null;
			allowMouseAxis = null;
			allowMouseButtons = null;
			allowGamepadAxis = null;
			allowGamepadButtons = null;
			allowKeyboard = null;
		}
	}
}
