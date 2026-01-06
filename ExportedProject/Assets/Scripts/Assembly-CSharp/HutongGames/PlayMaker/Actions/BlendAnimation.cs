using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Blends an Animation towards a Target Weight over a specified Time.\nOptionally sends an Event when finished.")]
	[ActionCategory(ActionCategory.Animation)]
	public class BlendAnimation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("The GameObject to animate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The name of the animation to blend.")]
		[RequiredField]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		[HasFloatSlider(0f, 1f)]
		[Tooltip("Target weight to blend to.")]
		[RequiredField]
		public FsmFloat targetWeight;

		[RequiredField]
		[Tooltip("How long should the blend take.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat time;

		[Tooltip("Event to send when the blend has finished.")]
		public FsmEvent finishEvent;

		private DelayedEvent delayedFinishEvent;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			targetWeight = 1f;
			time = 0.3f;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			DoBlendAnimation((gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? gameObject.GameObject.Value : base.Owner);
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedFinishEvent))
			{
				Finish();
			}
		}

		private void DoBlendAnimation(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			Animation component = go.GetComponent<Animation>();
			if (component == null)
			{
				LogWarning("Missing Animation component on GameObject: " + go.name);
				Finish();
				return;
			}
			AnimationState animationState = component[animName.Value];
			if (animationState == null)
			{
				LogWarning("Missing animation: " + animName.Value);
				Finish();
				return;
			}
			float value = time.Value;
			component.Blend(animName.Value, targetWeight.Value, value);
			if (finishEvent != null)
			{
				delayedFinishEvent = base.Fsm.DelayedEvent(finishEvent, animationState.length);
			}
			else
			{
				Finish();
			}
		}
	}
}
