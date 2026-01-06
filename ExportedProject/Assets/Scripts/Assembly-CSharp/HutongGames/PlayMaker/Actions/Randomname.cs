using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("♥ Caligula ♥")]
	[Tooltip("Generates random string variable")]
	public class Randomname : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Lenght of the string variable")]
		public int lenght;

		[Tooltip("Generated name")]
		[RequiredField]
		public FsmString result;

		public override void OnEnter()
		{
			result.Value = generateNameLenght(lenght);
			Finish();
		}

		private string generateNameLenght(int i)
		{
			string text = string.Empty;
			int num = Random.Range(1, 71);
			int num2 = 1;
			while (num2 <= i)
			{
				num2++;
				num = Random.Range(1, 71);
				text += newChar(num);
			}
			return text;
		}

		private string newChar(int i)
		{
			string text = string.Empty;
			switch (i)
			{
			case 1:
				text = "a";
				break;
			case 2:
				text = "b";
				break;
			case 3:
				text = "c";
				break;
			case 4:
				text = "d";
				break;
			case 5:
				text = "e";
				break;
			case 6:
				text = "f";
				break;
			case 7:
				text = "g";
				break;
			case 8:
				text = "q";
				break;
			case 9:
				text = "w";
				break;
			case 10:
				text = "e";
				break;
			case 11:
				text = "r";
				break;
			case 12:
				text = "t";
				break;
			case 13:
				text = "y";
				break;
			case 14:
				text = "u";
				break;
			case 15:
				text = "i";
				break;
			case 16:
				text = "o";
				break;
			case 17:
				text = "p";
				break;
			case 18:
				text = "a";
				break;
			case 19:
				text = "s";
				break;
			case 20:
				text = "d";
				break;
			case 21:
				text = "f";
				break;
			case 22:
				text = "g";
				break;
			case 23:
				text = "h";
				break;
			case 24:
				text = "j";
				break;
			case 25:
				text = "k";
				break;
			case 26:
				text = "l";
				break;
			case 27:
				text = "m";
				break;
			case 28:
				text = "n";
				break;
			case 29:
				text = "b";
				break;
			case 30:
				text = "v";
				break;
			case 31:
				text = "c";
				break;
			case 32:
				text = "x";
				break;
			case 33:
				text = "x";
				break;
			case 34:
				text = "z";
				break;
			case 35:
				text = "Q";
				break;
			case 36:
				text = "W";
				break;
			case 37:
				text = "E";
				break;
			case 38:
				text = "R";
				break;
			case 39:
				text = "T";
				break;
			case 40:
				text = "Y";
				break;
			case 41:
				text = "U";
				break;
			case 42:
				text = "I";
				break;
			case 43:
				text = "O";
				break;
			case 44:
				text = "P";
				break;
			case 45:
				text = "A";
				break;
			case 46:
				text = "S";
				break;
			case 47:
				text = "D";
				break;
			case 48:
				text = "F";
				break;
			case 49:
				text = "G";
				break;
			case 50:
				text = "H";
				break;
			case 51:
				text = "J";
				break;
			case 52:
				text = "K";
				break;
			case 53:
				text = "L";
				break;
			case 54:
				text = "Z";
				break;
			case 55:
				text = "X";
				break;
			case 56:
				text = "C";
				break;
			case 57:
				text = "V";
				break;
			case 58:
				text = "B";
				break;
			case 59:
				text = "N";
				break;
			case 60:
				text = "M";
				break;
			case 61:
				text = "1";
				break;
			case 62:
				text = "2";
				break;
			case 63:
				text = "3";
				break;
			case 64:
				text = "4";
				break;
			case 65:
				text = "5";
				break;
			case 66:
				text = "6";
				break;
			case 67:
				text = "7";
				break;
			case 68:
				text = "8";
				break;
			case 69:
				text = "9";
				break;
			case 70:
				text = "0";
				break;
			}
			return text;
		}
	}
}
