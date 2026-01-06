using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Pixel Inset Position of the GUITexture attached to a Game Object. Useful for moving GUI elements around.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class SetGUITexturePixelInsetPosition : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmFloat PixelInsetX;

		public FsmFloat PixelInsetY;

		public FsmBool AsIncrement;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			PixelInsetX = null;
			PixelInsetY = null;
			AsIncrement = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGUITexturePixelInsetPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGUITexturePixelInsetPosition();
		}

		private void DoGUITexturePixelInsetPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null && ownerDefaultTarget.GetComponent<GUITexture>() != null)
			{
				Rect pixelInset = ownerDefaultTarget.GetComponent<GUITexture>().pixelInset;
				if (AsIncrement.Value)
				{
					pixelInset.x += PixelInsetX.Value;
					pixelInset.y += PixelInsetY.Value;
				}
				else
				{
					pixelInset.x = PixelInsetX.Value;
					pixelInset.y = PixelInsetY.Value;
				}
				ownerDefaultTarget.GetComponent<GUITexture>().pixelInset = pixelInset;
			}
		}
	}
}
