using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a Transform.")]
	[ActionCategory("Easy Save 2")]
	public class SaveTransform : FsmStateAction
	{
		[Tooltip("The GameObject containing the Transform we want to save.")]
		[RequiredField]
		public FsmOwnerDefault saveValue;

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
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(saveValue);
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
			ES2.Save(component, string.Concat(saveFile, "?tag=", uniqueTag));
			Log(string.Concat("Saved to ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
