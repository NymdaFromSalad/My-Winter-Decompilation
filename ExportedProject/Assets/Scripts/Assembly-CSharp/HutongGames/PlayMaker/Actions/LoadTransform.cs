using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Loads a previously saved Transform.")]
	public class LoadTransform : FsmStateAction
	{
		[Tooltip("The GameObject containing the Transform we want to load into.")]
		[RequiredField]
		public FsmOwnerDefault loadValue;

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
			loadFromResources = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(loadValue);
			if (ownerDefaultTarget == null)
			{
				LogError("Could not save Transform. No GameObject has been specified.");
				Finish();
				return;
			}
			Transform component;
			if ((component = ownerDefaultTarget.GetComponent<Transform>()) == null)
			{
				LogError("Could not save Transform. GameObject does not contain a Transform.");
				Finish();
				return;
			}
			ES2Settings eS2Settings = new ES2Settings();
			if (loadFromResources)
			{
				eS2Settings.saveLocation = ES2Settings.SaveLocation.Resources;
			}
			ES2.Load(string.Concat(saveFile, "?tag=", uniqueTag), component, eS2Settings);
			Log(string.Concat("Loaded from ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
