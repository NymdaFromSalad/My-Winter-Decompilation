using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a PlayMaker Array List Proxy component using EasySave")]
	[ActionCategory("Easy Save 2")]
	public class ArrayListEasyLoadEncrypted : ArrayListActions
	{
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("A unique tag for this Load. For example, the object's name if no other objects use the same name.  --- Disabled: Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		[ActionSection("Easy Save Set Up")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("Activate Decryption")]
		public FsmBool encryption;

		[Tooltip("the password of the saved item")]
		public FsmString password = string.Empty;

		[Tooltip("The name of the file that contains our data.")]
		[RequiredField]
		public FsmString loadFile = "defaultES2File.txt";

		[Tooltip("Whether the data we are loading is stored in the Resources folder. (data needs .bytes extension!!!)")]
		public FsmBool loadFromResources;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
			encryption = false;
			password = string.Empty;
			loadFile = "defaultES2File.txt";
			loadFromResources = false;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				LoadArrayList();
			}
			Finish();
		}

		public void LoadArrayList()
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
			string text2 = "&savelocation=file";
			if (loadFromResources.Value)
			{
				text2 = "&savelocation=resources";
			}
			List<string> list = ES2.LoadList<string>(loadFile.Value + "?tag=" + uniqueTag.Value + "&encrypt=" + text + "&password=" + password.Value + text2);
			Log("Loaded from " + loadFile.Value + "?tag=" + uniqueTag);
			Debug.LogWarning("Persistent Data Path:" + Application.persistentDataPath);
			proxy.arrayList.Clear();
			foreach (string item in list)
			{
				proxy.arrayList.Add(PlayMakerUtils.ParseValueFromString(item));
			}
			Finish();
		}
	}
}
