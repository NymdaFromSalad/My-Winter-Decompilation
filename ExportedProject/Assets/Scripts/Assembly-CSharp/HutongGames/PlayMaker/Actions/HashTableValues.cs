using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Store all the values of a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy) into a PlayMaker arrayList Proxy component (PlayMakerArrayListProxy).")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableValues : HashTableActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component that will store the values")]
		[ActionSection("Result")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[RequiredField]
		public FsmOwnerDefault arrayListGameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component that will store the values ( necessary if several component coexists on the same GameObject")]
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
				doHashTableValues();
			}
			Finish();
		}

		public void doHashTableValues()
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
					arrayListProxyPointer.arrayList.AddRange(proxy.hashTable.Values);
				}
			}
		}
	}
}
