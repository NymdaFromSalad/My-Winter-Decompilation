using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Loads a previously saved Vector3.")]
	public class LoadVector3 : FsmStateAction
	{
		[Tooltip("The variable where we'll load our data into.")]
		[RequiredField]
		public FsmVector3 loadValue;

		[Tooltip("The unique tag for the data we want to load.")]
		[RequiredField]
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
			loadValue = null;
			loadFromResources = false;
		}

		public override void OnEnter()
		{
			ES2Settings eS2Settings = new ES2Settings();
			if (loadFromResources)
			{
				eS2Settings.saveLocation = ES2Settings.SaveLocation.Resources;
			}
			loadValue.Value = ES2.Load<Vector3>(string.Concat(saveFile, "?tag=", uniqueTag), eS2Settings);
			Log(string.Concat("Loaded from ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
