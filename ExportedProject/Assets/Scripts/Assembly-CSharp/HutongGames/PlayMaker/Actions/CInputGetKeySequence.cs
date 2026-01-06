using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Waits for user to input a sequence of keys like for a Pin or a cheat")]
	[ActionCategory("cInput")]
	public class CInputGetKeySequence : FsmStateAction
	{
		[Tooltip("The names of the keys in sequence.")]
		[RequiredField]
		public FsmString[] keyNames;

		[Tooltip("The number of missing keys until done.")]
		public FsmInt storeMissingKeysCount;

		[Tooltip("The key the user has to press.")]
		public FsmString storeExpectingKey;

		[Tooltip("sequence finished.")]
		public FsmEvent sendSequenceFinished;

		[Tooltip("sequence failed because wrong order of input.")]
		public FsmEvent sendSequenceFailed;

		private string nextKey;

		private int currentKeyIndex;

		public override void Reset()
		{
			keyNames = null;
			sendSequenceFinished = null;
			sendSequenceFailed = null;
		}

		public override void OnEnter()
		{
			if (keyNames.Length == 0)
			{
				Finish();
				return;
			}
			nextKey = keyNames[0].Value;
			storeExpectingKey.Value = nextKey;
			currentKeyIndex = 0;
			storeMissingKeysCount.Value = keyNames.Length;
		}

		public override void OnLateUpdate()
		{
			if (cInput.GetKeyDown(nextKey))
			{
				if (currentKeyIndex < keyNames.Length - 1)
				{
					currentKeyIndex++;
					nextKey = keyNames[currentKeyIndex].Value;
					storeExpectingKey.Value = nextKey;
				}
				else
				{
					base.Fsm.Event(sendSequenceFinished);
					Finish();
				}
				storeMissingKeysCount.Value--;
			}
			else if (Input.anyKeyDown)
			{
				base.Fsm.Event(sendSequenceFailed);
			}
		}
	}
}
