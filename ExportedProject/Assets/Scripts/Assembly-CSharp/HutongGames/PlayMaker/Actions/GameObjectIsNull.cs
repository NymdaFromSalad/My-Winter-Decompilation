namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a GameObject Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
	public class GameObjectIsNull : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The GameObject variable to test.")]
		public FsmGameObject gameObject;

		[Tooltip("Event to send if the GamObject is null.")]
		public FsmEvent isNull;

		[Tooltip("Event to send if the GamObject is NOT null.")]
		public FsmEvent isNotNull;

		[Tooltip("Store the result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			isNull = null;
			isNotNull = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsGameObjectNull();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsGameObjectNull();
		}

		private void DoIsGameObjectNull()
		{
			bool flag = gameObject.Value == null;
			if (storeResult != null)
			{
				storeResult.Value = flag;
			}
			base.Fsm.Event((!flag) ? isNotNull : isNull);
		}
	}
}
