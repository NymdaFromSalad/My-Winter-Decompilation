using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class SetGUITexture : ComponentAction<GUITexture>
	{
		[Tooltip("The GameObject that owns the GUITexture.")]
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Texture to apply.")]
		public FsmTexture texture;

		public override void Reset()
		{
			gameObject = null;
			texture = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.guiTexture.texture = texture.Value;
			}
			Finish();
		}
	}
}
