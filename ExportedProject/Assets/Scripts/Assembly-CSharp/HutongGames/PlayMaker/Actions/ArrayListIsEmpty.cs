namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Check if an ArrayList Proxy component is empty.")]
	public class ArrayListIsEmpty : ArrayListActions
	{
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[ActionSection("Set up")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;

		[Tooltip("Store in a bool wether it is empty or not")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmBool isEmpty;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this arrayList is empty ")]
		public FsmEvent isEmptyEvent;

		[Tooltip("Event sent if this arrayList is not empty")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent isNotEmptyEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			isEmpty = null;
			isNotEmptyEvent = null;
			isEmptyEvent = null;
		}

		public override void OnEnter()
		{
			PlayMakerArrayListProxy arrayListProxyPointer = GetArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value, true);
			bool flag = arrayListProxyPointer.arrayList.Count == 0;
			isEmpty.Value = flag;
			if (flag)
			{
				base.Fsm.Event(isEmptyEvent);
			}
			else
			{
				base.Fsm.Event(isNotEmptyEvent);
			}
			Finish();
		}
	}
}
