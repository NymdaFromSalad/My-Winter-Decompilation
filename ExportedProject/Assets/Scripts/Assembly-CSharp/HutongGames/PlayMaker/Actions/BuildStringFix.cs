namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Action removes a character from last position.")]
	[ActionCategory("♥ Caligula ♥")]
	public class BuildStringFix : FsmStateAction
	{
		[RequiredField]
		[Tooltip("A variable to be changed.")]
		public FsmString variable;

		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			if (variable.Value.Length > 0)
			{
				int length = variable.Value.Length;
				variable.Value = variable.Value.Remove(length - 1);
				Finish();
			}
			else
			{
				LogWarning("Variable is empty!");
			}
			Finish();
		}
	}
}
