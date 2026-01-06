namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a Quaternion.")]
	[ActionCategory("Easy Save 2")]
	public class SaveQuaternion : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The variable we want to save.")]
		public FsmQuaternion saveValue;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		[RequiredField]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data.")]
		[RequiredField]
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
