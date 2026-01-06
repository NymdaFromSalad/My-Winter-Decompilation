using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a PlayMaker Array List Proxy component")]
	public class ArrayListEasySaveEncrypted : ArrayListActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;

		[ActionSection("Easy Save Set Up")]
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("Activate Decryption")]
		public FsmBool encryption;

		[Tooltip("the password of the saved item")]
		public FsmString password = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			encryption = false;
			password = string.Empty;
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
			saveFile = "defaultES2File.txt";
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SaveArrayList();
			}
			Finish();
		}

		public void SaveArrayList()
		{
			if (!isProxyValid())
			{
				return;
			}
			string text = "false";
			if (encryption.Value)
			{
				text = "true";
			}
			List<string> list = new List<string>();
			foreach (object array in proxy.arrayList)
			{
				list.Add(PlayMakerUtils.ParseValueToString(array));
			}
			ES2.Save(list, string.Concat(saveFile.Value, "?tag=", uniqueTag, "&encrypt=", text, "&password=", password.Value));
			Log("Saved to " + saveFile.Value + "?tag=" + uniqueTag);
			Debug.LogWarning("Persistent Data Path:" + Application.persistentDataPath);
			Finish();
		}
	}
}
