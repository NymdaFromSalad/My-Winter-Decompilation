namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Revert a PlayMaker HashTable Proxy component to the prefill data, either defined at runtime or when the action HashTableTakeSnapShot was used.")]
	public class HashTableRevertSnapShot : HashTableActions
	{
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DoHashTableRevertToSnapShot();
			}
			Finish();
		}

		public void DoHashTableRevertToSnapShot()
		{
			if (isProxyValid())
			{
				proxy.RevertToSnapShot();
			}
		}
	}
}
