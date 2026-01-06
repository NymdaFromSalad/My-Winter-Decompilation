using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Bool Variable to True or False randomly.")]
	[ActionCategory(ActionCategory.Math)]
	public class RandomBool : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = Random.Range(0, 100) < 50;
			Finish();
		}
	}
}
