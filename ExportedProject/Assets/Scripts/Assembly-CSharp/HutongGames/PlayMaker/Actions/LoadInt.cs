namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a previously saved float.")]
	[ActionCategory("Easy Save 2")]
	public class LoadInt : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The variable where we'll load our data into.")]
		public FsmInt loadValue;

		[RequiredField]
		[Tooltip("The unique tag for the data we want to load.")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The name of the file our data is stored in.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

		[Tooltip("Whether the data we are loading is stored in the Resources folder.")]
		public bool loadFromResources;

		public override void Reset()
		{
			uniqueTag = string.Empty;
			saveFile = "defaultES2File.txt";
			loadValue = 0;
			loadFromResources = false;
		}

		public override void OnEnter()
		{
			ES2Settings eS2Settings = new ES2Settings();
			if (loadFromResources)
			{
				eS2Settings.saveLocation = ES2Settings.SaveLocation.Resources;
			}
			loadValue.Value = ES2.Load<int>(string.Concat(saveFile, "?tag=", uniqueTag), eS2Settings);
			Log(string.Concat("Loaded from ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
