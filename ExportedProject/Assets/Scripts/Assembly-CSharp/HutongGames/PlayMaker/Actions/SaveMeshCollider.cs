using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a MeshCollider.")]
	[ActionCategory("Easy Save 2")]
	public class SaveMeshCollider : FsmStateAction
	{
		[Tooltip("The GameObject containing the MeshCollider we want to save.")]
		[RequiredField]
		public FsmOwnerDefault saveValue;

		[RequiredField]
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data.")]
		public FsmString saveFile = "defaultES2File.txt";

		public override void Reset()
		{
			uniqueTag = string.Empty;
			saveFile = "defaultES2File.txt";
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(saveValue);
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
			ES2.Save(component, string.Concat(saveFile, "?tag=", uniqueTag));
			Log(string.Concat("Saved to ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
