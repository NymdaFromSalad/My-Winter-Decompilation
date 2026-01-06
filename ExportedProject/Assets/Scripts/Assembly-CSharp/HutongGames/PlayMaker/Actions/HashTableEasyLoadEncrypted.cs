using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Loads a PlayMaker HashTable Proxy component using EasySave")]
	public class HashTableEasyLoadEncrypted : HashTableActions
	{
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		[ActionSection("Easy Save Set Up")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("Activate Decryption")]
		public FsmBool encryption;

		[Tooltip("the password of the saved item")]
		public FsmString password = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data.")]
		[RequiredField]
		public FsmString loadFile = "defaultES2File.txt";

		[Tooltip("Whether the data we are loading is stored in the Resources folder.")]
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
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				LoadHashTable();
			}
			Finish();
		}

		public void LoadHashTable()
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
			Dictionary<string, string> dictionary = ES2.LoadDictionary<string, string>(loadFile.Value + "?tag=" + uniqueTag.Value + "&encrypt=" + text + "&password=" + password.Value + text2);
			Log("Loaded from " + loadFile.Value + "?tag=" + uniqueTag);
			proxy.hashTable.Clear();
			foreach (string key in dictionary.Keys)
			{
				proxy.hashTable[key] = PlayMakerUtils.ParseValueFromString(dictionary[key]);
			}
			Finish();
		}
	}
}
