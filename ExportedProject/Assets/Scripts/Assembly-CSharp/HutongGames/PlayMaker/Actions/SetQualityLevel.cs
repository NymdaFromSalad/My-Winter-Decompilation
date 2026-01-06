using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the name of a Game Object and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetQualityLevel : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("the number of the quality level so to say. 0 is the first quality level in the list")]
		[RequiredField]
		public FsmInt levelIndex;

		[Tooltip("Anti-aliasing takes a lot of power to change, so you should only make this true in a menu or if ytou don't care for a noticable drop in perf for half a sec")]
		public FsmBool applyExpensiveChanges;

		public override void Reset()
		{
			levelIndex = 0;
			applyExpensiveChanges = false;
		}

		public override void OnEnter()
		{
			int value = levelIndex.Value;
			bool value2 = applyExpensiveChanges.Value;
			QualitySettings.SetQualityLevel(value, value2);
		}
	}
}
