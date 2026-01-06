using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a PlayMaker HashTable Proxy component")]
	[ActionCategory("Easy Save 2")]
	public class HashTableEasySaveEncrypted : HashTableActions
	{
		[RequiredField]
		[ActionSection("Set up")]
		[Tooltip("The Game Object to add the Hashtable Component to.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
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
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
			encryption = false;
			password = string.Empty;
			saveFile = "defaultES2File.txt";
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SaveHashTable();
			}
			Finish();
		}

		public void SaveHashTable()
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
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (object key in proxy.hashTable.Keys)
			{
				dictionary[(string)key] = PlayMakerUtils.ParseValueToString(proxy.hashTable[key]);
			}
			ES2.Save(dictionary, string.Concat(saveFile.Value, "?tag=", uniqueTag, "&encrypt=", text, "&password=", password.Value));
			Log("Saved to " + saveFile.Value + "?tag=" + uniqueTag);
			Finish();
		}
	}
}
