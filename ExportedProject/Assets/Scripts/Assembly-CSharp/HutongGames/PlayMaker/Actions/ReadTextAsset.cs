using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the content of a text asset")]
	[ActionCategory(ActionCategory.String)]
	public class ReadTextAsset : FsmStateAction
	{
		[RequiredField]
		public TextAsset textAsset;

		[Tooltip("The content of the text asset")]
		public FsmString content;

		public override void Reset()
		{
			textAsset = null;
			content = string.Empty;
		}

		public override void OnEnter()
		{
			if (textAsset != null)
			{
				content.Value = textAsset.text;
			}
			Finish();
		}
	}
}
