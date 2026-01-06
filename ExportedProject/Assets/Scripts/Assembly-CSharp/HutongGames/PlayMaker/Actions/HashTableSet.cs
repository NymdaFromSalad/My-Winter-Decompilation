namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set an key/value pair to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableSet : HashTableActions
	{
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[RequiredField]
		[Tooltip("The Key value for that hash set")]
		[UIHint(UIHint.FsmString)]
		public FsmString key;

		[Tooltip("The variable to set.")]
		[ActionSection("Result")]
		public FsmVar variable;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			key = null;
			variable = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SetHashTable();
			}
			Finish();
		}

		public void SetHashTable()
		{
			if (isProxyValid())
			{
				proxy.hashTable[key.Value] = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable);
			}
		}
	}
}
