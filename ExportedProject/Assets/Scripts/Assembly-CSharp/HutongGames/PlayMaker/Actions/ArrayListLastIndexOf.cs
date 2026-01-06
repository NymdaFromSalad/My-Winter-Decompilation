using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Return the Last index occurence of an item from a PlayMaker Array List Proxy component. Can search within a range")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListLastIndexOf : ArrayListActions
	{
		[RequiredField]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("Optional start index to search from: set to 0 to ignore")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt startIndex;

		[Tooltip("Optional amount of elements to search within: set to 0 to ignore")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt count;

		[RequiredField]
		[Tooltip("The variable to get the index of.")]
		[ActionSection("Data")]
		public FsmVar variable;

		[ActionSection("Result")]
		[Tooltip("The index of the last item described below")]
		[RequiredField]
		public FsmInt lastIndexOf;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this arraList contains that element ( described below)")]
		public FsmEvent itemFound;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this arraList does not contains that element ( described below)")]
		public FsmEvent itemNotFound;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Optional Event to trigger if the action fails ( likely an out of range exception when using wrong values for index and/or count)")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			startIndex = null;
			count = null;
			lastIndexOf = null;
			itemFound = null;
			itemNotFound = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DoArrayListLastIndex();
			}
			Finish();
		}

		public void DoArrayListLastIndex()
		{
			if (!isProxyValid())
			{
				return;
			}
			object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable);
			int num = -1;
			try
			{
				if (startIndex.Value == 0 && count.Value == 0)
				{
					num = PlayMakerUtils_Extensions.LastIndexOf(proxy.arrayList, valueFromFsmVar);
				}
				else if (count.Value == 0)
				{
					if (startIndex.Value < 0 || startIndex.Value >= proxy.arrayList.Count)
					{
						Debug.LogError("start index out of range");
						return;
					}
					num = PlayMakerUtils_Extensions.LastIndexOf(proxy.arrayList, valueFromFsmVar, startIndex.Value);
				}
				else
				{
					if (startIndex.Value < 0 || startIndex.Value >= proxy.arrayList.Count - count.Value)
					{
						Debug.LogError("start index and count out of range");
						return;
					}
					num = PlayMakerUtils_Extensions.LastIndexOf(proxy.arrayList, valueFromFsmVar, startIndex.Value, count.Value);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				base.Fsm.Event(failureEvent);
				return;
			}
			lastIndexOf.Value = num;
			if (num == -1)
			{
				base.Fsm.Event(itemNotFound);
			}
			else
			{
				base.Fsm.Event(itemFound);
			}
		}
	}
}
