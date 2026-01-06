using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a point is inside a rectangle. It also takes in account negative width and height")]
	[ActionCategory(ActionCategory.Rect)]
	public class RectContains2 : FsmStateAction
	{
		[Tooltip("Rectangle to test.")]
		[RequiredField]
		public FsmRect rectangle;

		[Tooltip("Point to test.")]
		public FsmVector3 point;

		[Tooltip("Specify/override X value.")]
		public FsmFloat x;

		[Tooltip("Specify/override Y value.")]
		public FsmFloat y;

		[Tooltip("Event to send if the Point is inside the Rectangle.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the Point is outside the Rectangle.")]
		public FsmEvent falseEvent;

		[Tooltip("Store the result in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			rectangle = new FsmRect
			{
				UseVariable = true
			};
			point = new FsmVector3
			{
				UseVariable = true
			};
			x = new FsmFloat
			{
				UseVariable = true
			};
			y = new FsmFloat
			{
				UseVariable = true
			};
			storeResult = null;
			trueEvent = null;
			falseEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoRectContains();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoRectContains();
		}

		private void DoRectContains()
		{
			if (!rectangle.IsNone)
			{
				Vector3 value = point.Value;
				if (!x.IsNone)
				{
					value.x = x.Value;
				}
				if (!y.IsNone)
				{
					value.y = y.Value;
				}
				Rect value2 = rectangle.Value;
				if (value2.width < 0f)
				{
					value2.x = rectangle.Value.x + rectangle.Value.width;
					value2.width = 0f - rectangle.Value.width;
				}
				if (value2.height < 0f)
				{
					value2.y = rectangle.Value.y + rectangle.Value.height;
					value2.height = 0f - rectangle.Value.height;
				}
				bool flag = value2.Contains(value);
				storeResult.Value = flag;
				base.Fsm.Event((!flag) ? falseEvent : trueEvent);
			}
		}
	}
}
