using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a previously saved SphereCollider.")]
	[ActionCategory("Easy Save 2")]
	public class LoadSphereCollider : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject containing the SphereCollider we want to load into.")]
		public FsmOwnerDefault loadValue;

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
			loadFromResources = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(loadValue);
			if (ownerDefaultTarget == null)
			{
				LogError("Could not save SphereCollider. No GameObject has been specified.");
				Finish();
				return;
			}
			SphereCollider component;
			if ((component = ownerDefaultTarget.GetComponent<SphereCollider>()) == null)
			{
				LogError("Could not save SphereCollider. GameObject does not contain a SphereCollider.");
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
