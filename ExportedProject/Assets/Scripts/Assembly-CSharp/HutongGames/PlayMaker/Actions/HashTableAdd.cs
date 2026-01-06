namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Add an key/value pair to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy).")]
	public class HashTableAdd : HashTableActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[UIHint(UIHint.FsmString)]
		[RequiredField]
		[ActionSection("Data")]
		[Tooltip("The Key value for that hash set")]
		public FsmString key;

		[RequiredField]
		[Tooltip("The variable to add.")]
		public FsmVar variable;

		[ActionSection("Result")]
		[Tooltip("The event to trigger when element is added")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent successEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger when element exists already")]
		public FsmEvent keyExistsAlreadyEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			key = null;
			variable = null;
			successEvent = null;
			keyExistsAlreadyEvent = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				if (proxy.hashTable.ContainsKey(key.Value))
				{
					base.Fsm.Event(keyExistsAlreadyEvent);
				}
				else
				{
					AddToHashTable();
					base.Fsm.Event(successEvent);
				}
			}
			Finish();
		}

		public void AddToHashTable()
		{
			if (isProxyValid())
			{
				proxy.hashTable.Add(key.Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable));
			}
		}
	}
}
