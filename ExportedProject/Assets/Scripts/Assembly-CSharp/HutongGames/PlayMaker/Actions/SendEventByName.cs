namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event by name after an optional delay. NOTE: Use this over Send Event if you store events as string variables.")]
	public class SendEventByName : FsmStateAction
	{
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;

		[RequiredField]
		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		public FsmString sendEvent;

		[Tooltip("Optional delay in seconds.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		[Tooltip("Repeat every frame. Rarely needed.")]
		public bool everyFrame;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			eventTarget = null;
			sendEvent = null;
			delay = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (delay.Value < 0.001f)
			{
				base.Fsm.Event(eventTarget, sendEvent.Value);
				Finish();
			}
			else
			{
				delayedEvent = base.Fsm.DelayedEvent(eventTarget, FsmEvent.GetFsmEvent(sendEvent.Value), delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (!everyFrame && DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}
	}
}
