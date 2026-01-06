using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Loads a PlayMaker HashTable Proxy component using EasySave")]
	public class HashTableEasyLoad : HashTableActions
	{
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[ActionSection("Easy Save Set Up")]
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

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
			saveFile = "defaultES2File.txt";
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
			string text = uniqueTag.Value;
			if (string.IsNullOrEmpty(text))
			{
				text = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/hashTable/" + reference;
			}
			ES2Settings eS2Settings = new ES2Settings();
			if (loadFromResources.Value)
			{
				eS2Settings.saveLocation = ES2Settings.SaveLocation.Resources;
			}
			Dictionary<string, string> dictionary = ES2.LoadDictionary<string, string>(saveFile.Value + "?tag=" + text);
			Log("Loaded from " + saveFile.Value + "?tag=" + text);
			proxy.hashTable.Clear();
			foreach (string key in dictionary.Keys)
			{
				proxy.hashTable[key] = PlayMakerUtils.ParseValueFromString(dictionary[key]);
			}
			Finish();
		}
	}
}
