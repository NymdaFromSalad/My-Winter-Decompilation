namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Check if an HashTable Proxy component is empty.")]
	public class HashTableIsEmpty : ArrayListActions
	{
		[RequiredField]
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[ActionSection("Result")]
		[Tooltip("Store in a bool wether it is empty or not")]
		[UIHint(UIHint.Variable)]
		public FsmBool isEmpty;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this HashTable is empty ")]
		public FsmEvent isEmptyEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this HashTable is not empty")]
		public FsmEvent isNotEmptyEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			isEmpty = null;
			isEmptyEvent = null;
			isNotEmptyEvent = null;
		}

		public override void OnEnter()
		{
			PlayMakerHashTableProxy hashTableProxyPointer = GetHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value, true);
			bool flag = hashTableProxyPointer.hashTable.Count == 0;
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
