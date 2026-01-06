namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a bool.")]
	public class SaveBool : FsmStateAction
	{
		[Tooltip("The variable we want to save.")]
		[RequiredField]
		public FsmBool saveValue;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		[RequiredField]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data.")]
		public FsmString saveFile = "defaultES2File.txt";

		public override void Reset()
		{
			uniqueTag = string.Empty;
			saveFile = "defaultES2File.txt";
			saveValue = null;
		}

		public override void OnEnter()
		{
			ES2.Save(saveValue.Value, string.Concat(saveFile, "?tag=", uniqueTag));
			Log(string.Concat("Saved to ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
