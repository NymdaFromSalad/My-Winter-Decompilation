using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Sets the cGUI skin")]
	public class CInputSetGuiSkin : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The gui to set for cGUI.")]
		public GUISkin guiSkin;

		public override void OnEnter()
		{
			cGUI.cSkin = guiSkin;
			Finish();
		}
	}
}
