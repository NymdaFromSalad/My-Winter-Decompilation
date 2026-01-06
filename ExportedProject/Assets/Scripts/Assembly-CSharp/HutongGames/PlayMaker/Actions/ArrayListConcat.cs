using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Concat joins two or more arrayList proxy components. if a target is specified, the method use the target store the concatenation, else the ")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListConcat : ArrayListActions
	{
		[ActionSection("Storage")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to store the concatenation ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The GameObject with the PlayMaker ArrayList Proxy component to copy to")]
		[RequiredField]
		[CompoundArray("ArrayLists", "ArrayList GameObject", "Reference")]
		[ObjectType(typeof(PlayMakerArrayListProxy))]
		[ActionSection("ArrayLists to concatenate")]
		public FsmOwnerDefault[] arrayListGameObjectTargets;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
		public FsmString[] referenceTargets;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			arrayListGameObjectTargets = null;
			referenceTargets = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DoArrayListConcat(proxy.arrayList);
			}
			Finish();
		}

		public void DoArrayListConcat(ArrayList source)
		{
			if (!isProxyValid())
			{
				return;
			}
			for (int i = 0; i < arrayListGameObjectTargets.Length; i++)
			{
				if (!SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(arrayListGameObjectTargets[i]), referenceTargets[i].Value) || !isProxyValid())
				{
					continue;
				}
				foreach (object array in proxy.arrayList)
				{
					source.Add(array);
					Debug.Log("count " + source.Count);
				}
			}
		}
	}
}
