using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("♥ Caligula ♥")]
	[Tooltip("Saves a key to a string variable")]
	public class SaveKey : FsmStateAction
	{
		[RequiredField]
		[Tooltip("A variable where the key is stored.")]
		public FsmString variable;

		public override void Reset()
		{
			variable = string.Empty;
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
			{
				variable.Value += "0";
			}
			if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
			{
				variable.Value += "1";
			}
			if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
			{
				variable.Value += "2";
			}
			if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
			{
				variable.Value += "3";
			}
			if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
			{
				variable.Value += "4";
			}
			if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
			{
				variable.Value += "5";
			}
			if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
			{
				variable.Value += "6";
			}
			if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
			{
				variable.Value += "7";
			}
			if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
			{
				variable.Value += "8";
			}
			if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
			{
				variable.Value += "9";
			}
			if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
			{
				Finish();
			}
		}
	}
}
