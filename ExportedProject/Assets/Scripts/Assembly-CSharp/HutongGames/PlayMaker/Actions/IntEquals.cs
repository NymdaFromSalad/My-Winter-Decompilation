namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends Events based on the comparison of multiple Integers.")]
	[ActionCategory(ActionCategory.Logic)]
	public class IntEquals : FsmStateAction
	{
		[RequiredField]
		public FsmInt[] integers;

		[Tooltip("Event sent if all ints equal")]
		public FsmEvent equal;

		[Tooltip("Event sent if ints not equal")]
		public FsmEvent notEqual;

		public bool everyFrame;

		public override void Reset()
		{
			integers = new FsmInt[2];
			equal = null;
			notEqual = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIntCompare();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIntCompare();
		}

		private void DoIntCompare()
		{
			if (integers.Length > 1)
			{
				int value = integers[0].Value;
				FsmInt[] array = integers;
				foreach (FsmInt fsmInt in array)
				{
					if (value != fsmInt.Value)
					{
						base.Fsm.Event(notEqual);
						return;
					}
				}
			}
			base.Fsm.Event(equal);
		}

		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(equal) && FsmEvent.IsNullOrEmpty(notEqual))
			{
				return "Action sends no events!";
			}
			if (integers.Length < 2)
			{
				return "Action needs more than 1 int to compare";
			}
			return string.Empty;
		}
	}
}
