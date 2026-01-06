using UnityEngine;

public class RegPlateGen : MonoBehaviour
{
	private enum CharType
	{
		CHARTYPE_LETTER = 0,
		CHARTYPE_NUMBER = 1,
		CHARTYPE_NONE = 2
	}

	private enum RegPlateFiltering
	{
		REGPLATE_OK = 0,
		REGPLATE_NOK = 1
	}

	public Texture2D RegPlateBlank;

	public Texture2D RegPlateAtlas;

	public string PlateString = "VBX-403";

	public bool GenerateOnStart;

	public bool UseFilter = true;

	public bool AllowLeadingZero;

	public bool AdjustCharColor;

	public float BrightnessChar = 1f;

	public float ContrastChar = 1f;

	public Color TintChar = Color.white;

	public bool AdjustBgColor;

	public float BrightnessBg = 1f;

	public float ContrastBg = 1f;

	public Color TintBg = Color.white;

	public bool SaveToFile;

	public string PlateFileName = "reg_place_test";

	private int DashWidth = 16;

	private int FontWidth = 32;

	private int FontHeight = 48;

	private int PlateStartX = 22;

	private int PlateStartY = 17;

	private RegPlateFiltering RegPlateFilterResult;

	private void Start()
	{
		if (GenerateOnStart)
		{
			GenerateRegPlate(PlateString);
		}
	}

	private void GenerateRegPlate(char[] PlateLetterChars)
	{
		GenerateRegPlate(new string(PlateLetterChars));
	}

	private void GenerateRegPlate(string PlateLettersValue)
	{
		GenerateRegPlate(PlateLettersValue, UseFilter, AllowLeadingZero, SaveToFile, PlateFileName, AdjustCharColor, BrightnessChar, ContrastChar, TintChar, AdjustBgColor, BrightnessBg, ContrastBg, TintBg);
	}

	private void GenerateRegPlate(string PlateLettersValue, bool UseFilter, bool AllowLeadingZero, bool SaveToFile, string PlateFileName, bool AdjustCharColor, float BrightnessChar, float ContrastChar, Color TintChar, bool AdjustBgColor, float BrightnessBg, float ContrastBg, Color TintBg)
	{
		string plateLetters = PlateLettersValue;
		if (UseFilter)
		{
			plateLetters = FilterRegPlate(PlateLettersValue, AllowLeadingZero);
			if (RegPlateFilterResult != RegPlateFiltering.REGPLATE_OK)
			{
			}
		}
		GenerateTexture(plateLetters, SaveToFile, PlateFileName, AdjustCharColor, BrightnessChar, ContrastChar, TintChar, AdjustBgColor, BrightnessBg, ContrastBg, TintBg);
	}

	private string FilterRegPlate(string PlateLetters, bool AllowLeadingZero)
	{
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		CharType charType = CharType.CHARTYPE_NONE;
		string text2 = PlateLetters.ToUpper();
		foreach (char c in text2)
		{
			CharType charType2 = CharType.CHARTYPE_NONE;
			if ((c >= 'A' && c <= 'Z') || c == 'Å' || c == 'Ä' || c == 'Ö')
			{
				charType2 = CharType.CHARTYPE_LETTER;
			}
			else if (c >= '0' && c <= '9' && (num2 > 0 || c != '0' || AllowLeadingZero))
			{
				charType2 = CharType.CHARTYPE_NUMBER;
			}
			if (charType2 != CharType.CHARTYPE_NONE && num == 0 && num2 == 0)
			{
				charType = charType2;
			}
			if (charType == CharType.CHARTYPE_LETTER)
			{
				if (charType2 == CharType.CHARTYPE_LETTER && num2 == 0 && num < 3)
				{
					num++;
					text += c;
				}
				else if (charType2 == CharType.CHARTYPE_NUMBER && num > 0)
				{
					if (num2 == 0)
					{
						text += "-";
					}
					num2++;
					text += c;
					if (num2 == 3)
					{
						break;
					}
				}
			}
			else if (charType2 == CharType.CHARTYPE_NUMBER && num == 0 && num2 < 3)
			{
				num2++;
				text += c;
			}
			else if (charType2 == CharType.CHARTYPE_LETTER && num2 > 0)
			{
				if (num == 0)
				{
					text += "-";
				}
				num++;
				text += c;
				if (num == 3)
				{
					break;
				}
			}
		}
		if (num == 0 || num2 == 0)
		{
			RegPlateFilterResult = RegPlateFiltering.REGPLATE_NOK;
			return "-";
		}
		RegPlateFilterResult = RegPlateFiltering.REGPLATE_OK;
		return text;
	}

	private int GetAtlasOffset(char PlateChar)
	{
		switch (PlateChar)
		{
		case '-':
			return 0;
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return (PlateChar - 48 + 1) * FontHeight;
		default:
			if (PlateChar >= 'A' && PlateChar <= 'Z')
			{
				return (PlateChar - 65 + 11) * FontHeight;
			}
			switch (PlateChar)
			{
			case 'Å':
				return 37 * FontHeight;
			case 'Ä':
				return 38 * FontHeight;
			default:
				return 39 * FontHeight;
			}
		}
	}

	private Color AdjustColor(Color ColorValue, Color TintValue, float BrightnessValue = 1f, float ContrastValue = 1f)
	{
		float num = ColorValue.r * BrightnessValue;
		float num2 = ColorValue.g * BrightnessValue;
		float num3 = ColorValue.b * BrightnessValue;
		num = Mathf.Clamp01((num - 0.5f) * ContrastValue + 0.5f);
		num2 = Mathf.Clamp01((num2 - 0.5f) * ContrastValue + 0.5f);
		num3 = Mathf.Clamp01((num3 - 0.5f) * ContrastValue + 0.5f);
		Color color = new Color(num, num2, num3);
		return color * TintValue;
	}

	private void GenerateTexture(string PlateLetters, bool SaveToFile = false, string PlateFileName = "reg_place_test", bool AdjustCharColor = false, float BrightnessChar = 1f, float ContrastChar = 1f, Color? TintCharValue = null, bool AdjustBgColor = false, float BrightnessBg = 1f, float ContrastBg = 1f, Color? TintBgValue = null)
	{
		Color valueOrDefault = TintCharValue.GetValueOrDefault(Color.white);
		Color valueOrDefault2 = TintBgValue.GetValueOrDefault(Color.white);
		int num = (7 - PlateLetters.Length) * FontWidth / 2;
		Texture2D texture2D = new Texture2D(RegPlateBlank.width, RegPlateBlank.height, TextureFormat.RGB24, true);
		Color[] pixels = RegPlateBlank.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			if (AdjustBgColor)
			{
				pixels[i] = AdjustColor(pixels[i], valueOrDefault2, BrightnessBg, ContrastBg);
			}
		}
		texture2D.SetPixels(pixels);
		int num2 = num;
		int num3 = 0;
		string text = PlateLetters.ToUpper();
		foreach (char c in text)
		{
			int atlasOffset = GetAtlasOffset(c);
			int num4 = texture2D.height - PlateStartY;
			for (int k = 0; k < FontHeight; k++)
			{
				int num5 = PlateStartX + FontWidth * num3;
				for (int l = 0; l < FontWidth; l++)
				{
					Color color = RegPlateAtlas.GetPixel(l, RegPlateAtlas.height - 1 - k - atlasOffset);
					if (color != Color.white)
					{
						if (AdjustCharColor)
						{
							color = AdjustColor(color, valueOrDefault, BrightnessChar, ContrastChar);
						}
						texture2D.SetPixel(num5 + l + num2, num4 - k, color);
					}
				}
			}
			if (c == '-')
			{
				num2 -= FontWidth - DashWidth;
			}
			num3++;
		}
		texture2D.Apply(true);
		Renderer component = GetComponent<Renderer>();
		component.material.mainTexture = texture2D;
	}
}
