using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Check if a gameObject is active.This lets you know if a gameObject is active in the game. That is the case if its GameObject.activeSelf property is enabled, as well as that of all it's parents.")]
	public class IsActive : FsmStateAction
	{
		[Tooltip("The GameObject to check activate state.")]
		[RequiredField]
		public FsmGameObject gameObject;

		[Tooltip("The active state of this gameObject. It uses activeInHierarchy, not activeSelf. So it will return true if this gameobject is active in the game.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isActive;

		public FsmEvent isActiveEvent;

		public FsmEvent isNotActiveEvent;

		[Tooltip("Repeat this action every frame. Useful if Activate changes over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			isActive = false;
			isActiveEvent = null;
			isNotActiveEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsActiveGameObject();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsActiveGameObject();
		}

		private void DoIsActiveGameObject()
		{
			GameObject value = gameObject.Value;
			if (!(value == null))
			{
				bool flag = false;
				flag = value.activeInHierarchy;
				isActive.Value = flag;
				if (flag)
				{
					base.Fsm.Event(isActiveEvent);
				}
				else
				{
					base.Fsm.Event(isNotActiveEvent);
				}
			}
		}
	}
}
