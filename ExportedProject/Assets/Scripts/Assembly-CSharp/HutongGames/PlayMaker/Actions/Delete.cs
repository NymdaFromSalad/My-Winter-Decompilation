namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Checks if data Delete at the specified path.")]
	public class Delete : FsmStateAction
	{
		[Tooltip("The tag that we want to delete (Optional).")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The file we want to delete, or delete the tag from.")]
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
			ES2.Delete(text);
			Log("Deleted " + text);
			Finish();
		}
	}
}
