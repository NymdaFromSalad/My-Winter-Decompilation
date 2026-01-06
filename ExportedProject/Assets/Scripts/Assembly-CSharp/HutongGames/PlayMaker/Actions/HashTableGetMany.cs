namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets items from a PlayMaker HashTable Proxy component")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableGetMany : HashTableActions
	{
		[RequiredField]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[ActionSection("Data")]
		[CompoundArray("Count", "Key", "Value")]
		[RequiredField]
		[Tooltip("The Key value for that hash set")]
		[UIHint(UIHint.FsmString)]
		public FsmString[] keys;

		[Tooltip("The value for that key")]
		[UIHint(UIHint.Variable)]
		public FsmVar[] results;

		public override void Reset()
		{
			gameObject = null;
			keys = null;
			results = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				Get();
			}
			Finish();
		}

		public void Get()
		{
			if (!isProxyValid())
			{
				return;
			}
			for (int i = 0; i < keys.Length; i++)
			{
				if (proxy.hashTable.ContainsKey(keys[i].Value))
				{
					PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, results[i], proxy.hashTable[keys[i].Value]);
				}
			}
		}
	}
}
