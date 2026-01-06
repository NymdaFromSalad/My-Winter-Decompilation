namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Checks if data exists at the specified path.")]
	[ActionCategory("Easy Save 2")]
	public class Exists : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Event to send if this it does exist.")]
		public FsmEvent ifExists;

		[Tooltip("The event to sent if it doesn't exist.")]
		[RequiredField]
		public FsmEvent ifDoesNotExist;

		[Tooltip("The tag that we want to check for (Optional).")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The file we want to check the existence of.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

		public override void Reset()
		{
			uniqueTag = string.Empty;
			saveFile = "defaultES2File.txt";
		}

		public override void OnEnter()
		{
			string text = ((!(uniqueTag.Value != string.Empty)) ? saveFile.Value : (saveFile.Value + "?tag=" + uniqueTag.Value));
			Log("Checked existence of " + text);
			if (ES2.Exists(text))
			{
				base.Fsm.Event(ifExists);
			}
			else
			{
				base.Fsm.Event(ifDoesNotExist);
			}
		}
	}
}
