namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Check if an HashTable Proxy component exists.")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableExists : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store in a bool wether it exists or not")]
		public FsmBool doesExists;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this HashTable exists ")]
		public FsmEvent doesExistsEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this HashTable does not exists")]
		public FsmEvent doesNotExistsEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			doesExists = null;
			doesExistsEvent = null;
			doesNotExistsEvent = null;
		}

		public override void OnEnter()
		{
			PlayMakerHashTableProxy hashTableProxyPointer = GetHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value, true);
			bool flag = hashTableProxyPointer != null;
			doesExists.Value = flag;
			if (flag)
			{
				base.Fsm.Event(doesExistsEvent);
			}
			else
			{
				base.Fsm.Event(doesNotExistsEvent);
			}
			Finish();
		}
	}
}
