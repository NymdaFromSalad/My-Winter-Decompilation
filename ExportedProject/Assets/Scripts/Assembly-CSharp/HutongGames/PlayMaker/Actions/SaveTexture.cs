using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a Texture.")]
	[ActionCategory("Easy Save 2")]
	public class SaveTexture : FsmStateAction
	{
		[Tooltip("The variable we want to save.")]
		[RequiredField]
		public FsmTexture saveValue;

		[RequiredField]
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
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
			ES2.Save(saveValue.Value as Texture2D, string.Concat(saveFile, "?tag=", uniqueTag));
			Log(string.Concat("Saved to ", saveFile, "?tag=", uniqueTag));
			Finish();
		}
	}
}
