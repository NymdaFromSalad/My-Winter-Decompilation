using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Each time this action is called it iterate to the next value from Start to End. This lets you safely loop and process anything on each iteratation.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class Iterate : FsmStateAction
	{
		[Tooltip("Start value")]
		[RequiredField]
		public FsmInt startIndex;

		[Tooltip("End value")]
		public FsmInt endIndex;

		[Tooltip("increment value at each iteration, absolute value only, it will itself find if it needs to substract or add")]
		public FsmInt increment;

		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when we reached the end.")]
		public FsmEvent finishedEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current value of the iteration process")]
		[ActionSection("Result")]
		public FsmInt currentIndex;

		private bool started;

		private bool _up = true;

		public override void Reset()
		{
			startIndex = 0;
			endIndex = 10;
			currentIndex = null;
			loopEvent = null;
			finishedEvent = null;
			increment = 1;
		}

		public override void OnEnter()
		{
			DoGetNext();
			Finish();
		}

		private void DoGetNext()
		{
			if (!started)
			{
				_up = startIndex.Value < endIndex.Value;
				currentIndex.Value = startIndex.Value;
				started = true;
				if (loopEvent != null)
				{
					base.Fsm.Event(loopEvent);
				}
				return;
			}
			if (_up)
			{
				if (currentIndex.Value >= endIndex.Value)
				{
					started = false;
					base.Fsm.Event(finishedEvent);
					return;
				}
				currentIndex.Value = Mathf.Max(startIndex.Value, currentIndex.Value + Mathf.Abs(increment.Value));
			}
			else
			{
				if (currentIndex.Value <= endIndex.Value)
				{
					started = false;
					base.Fsm.Event(finishedEvent);
					return;
				}
				currentIndex.Value = Mathf.Max(endIndex.Value, currentIndex.Value - Mathf.Abs(increment.Value));
			}
			if (loopEvent != null)
			{
				base.Fsm.Event(loopEvent);
			}
		}
	}
}
