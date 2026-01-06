using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Removes key and its corresponding value from the preferences.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsDeleteKey : FsmStateAction
	{
		public FsmString key;

		public override void Reset()
		{
			key = string.Empty;
		}

		public override void OnEnter()
		{
			if (!key.IsNone && !key.Value.Equals(string.Empty))
			{
				PlayerPrefs.DeleteKey(key.Value);
			}
			Finish();
		}
	}
}
