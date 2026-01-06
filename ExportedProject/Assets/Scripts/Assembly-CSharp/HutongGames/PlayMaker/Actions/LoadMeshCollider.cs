using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a previously saved MeshCollider.")]
	[ActionCategory("Easy Save 2")]
	public class LoadMeshCollider : FsmStateAction
	{
		[Tooltip("The GameObject containing the MeshCollider we want to load into.")]
		[RequiredField]
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
				LogError("Could not save MeshCollider. No GameObject has been specified.");
				Finish();
				return;
			}
			MeshCollider component;
			if ((component = ownerDefaultTarget.GetComponent<MeshCollider>()) == null)
			{
				LogError("Could not save MeshCollider. GameObject does not contain a MeshCollider.");
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
