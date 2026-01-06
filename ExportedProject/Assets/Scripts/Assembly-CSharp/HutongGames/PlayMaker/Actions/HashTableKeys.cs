using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Store all the keys of a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy) into a PlayMaker arrayList Proxy component (PlayMakerArrayListProxy).")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableKeys : HashTableActions
	{
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[ActionSection("Set up")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component that will store the keys")]
		[ActionSection("Result")]
		[RequiredField]
		public FsmOwnerDefault arrayListGameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component that will store the keys ( necessary if several component coexists on the same GameObject")]
		public FsmString arrayListReference;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			arrayListGameObject = null;
			arrayListReference = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doHashTableKeys();
			}
			Finish();
		}

		public void doHashTableKeys()
		{
			if (!isProxyValid())
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(arrayListGameObject);
			if (!(ownerDefaultTarget == null))
			{
				PlayMakerArrayListProxy arrayListProxyPointer = GetArrayListProxyPointer(ownerDefaultTarget, arrayListReference.Value, false);
				if (arrayListProxyPointer != null)
				{
					arrayListProxyPointer.arrayList.AddRange(proxy.hashTable.Keys);
				}
			}
		}
	}
}
