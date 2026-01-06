using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	public class AddScript : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to add the script to.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The Script to add to the GameObject.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString script;

		[Tooltip("Remove the script from the GameObject when this State is exited.")]
		public FsmBool removeOnExit;

		private Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			script = null;
		}

		public override void OnEnter()
		{
			DoAddComponent((gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? gameObject.GameObject.Value : base.Owner);
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				UnityEngine.Object.Destroy(addedComponent);
			}
		}

		private void DoAddComponent(GameObject go)
		{
			if (!(go == null))
			{
				addedComponent = go.AddComponent(GetType(script.Value));
				if (addedComponent == null)
				{
					LogError("Can't add script: " + script.Value);
				}
			}
		}

		private static Type GetType(string name)
		{
			Type globalType = ReflectionUtils.GetGlobalType(name);
			if (globalType != null)
			{
				return globalType;
			}
			globalType = ReflectionUtils.GetGlobalType("UnityEngine." + name);
			if (globalType != null)
			{
				return globalType;
			}
			return ReflectionUtils.GetGlobalType("HutongGames.PlayMaker." + name);
		}
	}
}
