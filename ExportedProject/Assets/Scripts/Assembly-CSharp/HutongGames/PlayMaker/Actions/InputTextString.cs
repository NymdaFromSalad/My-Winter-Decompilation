using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("♥ Caligula ♥")]
	[Tooltip("Saves pressed keys to a string variable")]
	public class InputTextString : FsmStateAction
	{
		[RequiredField]
		[Tooltip("A variable where the keys are stored.")]
		public FsmString variable;

		[RequiredField]
		[Tooltip("Manimum number of numbers!")]
		public FsmInt min;

		[Tooltip("Maximum number of numbers!")]
		[RequiredField]
		public FsmInt max;

		private int laskuri;

		private bool isPressed;

		public override void Reset()
		{
			laskuri = 0;
			isPressed = false;
		}

		private void addLetter(string s, bool b)
		{
			if (laskuri < max.Value)
			{
				if (b)
				{
					s = s.ToUpper();
					variable.Value += s;
				}
				else
				{
					variable.Value += s;
				}
			}
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Backspace) && variable.Value.Length > 0)
			{
				int length = variable.Value.Length;
				variable.Value = variable.Value.Remove(length - 1);
				laskuri--;
			}
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
			{
				isPressed = true;
			}
			else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
			{
				isPressed = false;
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				addLetter("q", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.W))
			{
				addLetter("w", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				addLetter("e", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				addLetter("r", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				addLetter("t", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.Y))
			{
				addLetter("y", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.U))
			{
				addLetter("u", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.I))
			{
				addLetter("i", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				addLetter("o", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				addLetter("p", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				addLetter("a", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				addLetter("s", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				addLetter("d", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				addLetter("f", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				addLetter("g", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				addLetter("h", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.J))
			{
				addLetter("j", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				addLetter("k", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				addLetter("l", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				addLetter("z", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				addLetter("x", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				addLetter("c", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				addLetter("v", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				addLetter("b", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.N))
			{
				addLetter("n", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				addLetter("m", isPressed);
			}
			if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
			{
				addLetter("0", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
			{
				addLetter("1", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
			{
				addLetter("2", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
			{
				addLetter("3", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
			{
				addLetter("4", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
			{
				addLetter("5", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
			{
				addLetter("6", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
			{
				addLetter("7", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
			{
				addLetter("8", false);
			}
			if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
			{
				addLetter("9", false);
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				addLetter(" ", false);
			}
			if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && laskuri >= min.Value)
			{
				Finish();
			}
			laskuri = variable.Value.Length;
		}
	}
}
