using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Sets the lock mode of the mouse cursor.")]
	public class SetCursorMode : FsmStateAction
	{
		public enum RenderMode
		{
			Auto = 0,
			ForceSoftware = 1
		}

		public enum CurState
		{
			None = 0,
			LockedToCenter = 1,
			ConfinedToGameWindow = 2
		}

		[Tooltip("The texture to use for the cursor or null to set the default cursor. \n\nNote that a texture needs to be imported with 'Read/Write enabled' in the texture importer (or using the 'Cursor' defaults), in order to be used as a cursor.")]
		[ObjectType(typeof(Texture2D))]
		public FsmObject cursorTexture;

		[Tooltip("The offset from the top left of the texture to use as the target point (must be within the bounds of the cursor). \n\n0,0 is normal behavior.")]
		public FsmVector2 hotSpot;

		[Tooltip("\nAuto: Use hardware cursors on supported platforms.\n\nOr\n\nForce the use of software cursors.")]
		public RenderMode renderMode;

		[Tooltip("\nFree Movement\nLocked to window center\nFree, but Confined to the game window")]
		public CurState lockMode;

		[Tooltip("Hide the cursor?")]
		public FsmBool hideCursor;

		private CursorMode _renderAs;

		private CursorLockMode _newMode;

		public override void Reset()
		{
			cursorTexture = null;
			hotSpot = new FsmVector2
			{
				UseVariable = true
			};
			renderMode = RenderMode.Auto;
			lockMode = CurState.None;
			hideCursor = true;
		}

		public override void OnEnter()
		{
			switch (lockMode)
			{
			case CurState.None:
				_newMode = CursorLockMode.None;
				break;
			case CurState.LockedToCenter:
				_newMode = CursorLockMode.Locked;
				Cursor.visible = false;
				break;
			case CurState.ConfinedToGameWindow:
				_newMode = CursorLockMode.Confined;
				break;
			}
			switch (renderMode)
			{
			case RenderMode.Auto:
				_renderAs = CursorMode.Auto;
				break;
			case RenderMode.ForceSoftware:
				_renderAs = CursorMode.ForceSoftware;
				break;
			}
			Cursor.visible = !hideCursor.Value;
			Texture2D texture = cursorTexture.Value as Texture2D;
			Cursor.SetCursor(texture, (!hotSpot.IsNone) ? hotSpot.Value : new Vector2(0f, 0f), _renderAs);
			Cursor.lockState = _newMode;
			Finish();
		}
	}
}
