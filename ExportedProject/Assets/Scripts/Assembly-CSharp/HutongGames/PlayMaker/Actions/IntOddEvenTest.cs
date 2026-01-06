namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Return if a given Int is odd or even. Store result in boolean variable and/or send events.")]
	[ActionCategory(ActionCategory.Math)]
	public class IntOddEvenTest : FsmStateAction
	{
		public FsmInt value;

		[UIHint(UIHint.Variable)]
		public FsmBool isOdd;

		[UIHint(UIHint.Variable)]
		public FsmBool isEven;

		public FsmEvent isOddEvent;

		public FsmEvent isEvenEvent;

		public bool everyFrame;

		public override void Reset()
		{
			value = null;
			isOdd = null;
			isEven = null;
			isOddEvent = null;
			isEvenEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCheck();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCheck();
		}

		private void DoCheck()
		{
			bool flag = value.Value % 2 != 0;
			if (!isOdd.IsNone)
			{
				isOdd.Value = flag;
			}
			if (isEven.IsNone)
			{
				isEven.Value = !flag;
			}
			if (isOddEvent != null && flag)
			{
				base.Fsm.Event(isOddEvent);
			}
			if (isEvenEvent != null && !flag)
			{
				base.Fsm.Event(isEvenEvent);
			}
		}
	}
}
