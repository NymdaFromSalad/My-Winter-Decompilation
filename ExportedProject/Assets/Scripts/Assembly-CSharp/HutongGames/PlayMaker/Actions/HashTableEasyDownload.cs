using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a PlayMaker HashTable Proxy component From MySQL Server via ES2.php file. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	[ActionCategory("Easy Save 2")]
	public class HashTableEasyDownload : HashTableActions
	{
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + hashtable Reference as tag.")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data. Leave as default if unsure.")]
		public FsmString saveFile = "defaultES2File.txt";

		[Tooltip("The name of the local file we want to create to store our data. Leave blank if you don't want to store data locally.")]
		public FsmString localFile = string.Empty;

		[RequiredField]
		[Tooltip("The URL to our ES2.PHP file. See http://www.moodkie.com/easysave/WebSetup.php for more information on setting up ES2Web")]
		[ActionSection("Upload Set up")]
		public FsmString urlToPHPFile = "http://www.mysite.com/ES2.php";

		[Tooltip("The username that you have specified in your ES2.php file.")]
		[RequiredField]
		public FsmString username = "ES2";

		[Tooltip("The password that you have specified in your ES2.php file.")]
		[RequiredField]
		public FsmString password = "65w84e4p994z3Oq";

		[ActionSection("Result")]
		[Tooltip("The Event to send if Download succeeded.")]
		public FsmEvent isDownloaded;

		[Tooltip("The event to send if Download failed.")]
		public FsmEvent isError;

		[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
		public FsmString errorMessage = string.Empty;

		[Tooltip("Where any error codes thrown will be stored. Set this to a variable, or leave it blank.")]
		public FsmString errorCode = string.Empty;

		private ES2Web web;

		private string _tag;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
			localFile = string.Empty;
			saveFile = "defaultES2File.txt";
			urlToPHPFile = "http://www.mysite.com/ES2.php";
			web = null;
			errorMessage = string.Empty;
			errorCode = string.Empty;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DownloadHashTable();
			}
		}

		private void DownloadHashTable()
		{
			if (isProxyValid())
			{
				_tag = uniqueTag.Value;
				if (string.IsNullOrEmpty(_tag))
				{
					_tag = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/hashTable/" + reference.Value;
				}
				web = new ES2Web(string.Concat(urlToPHPFile, "?tag=", _tag, "&webfilename=", saveFile.Value, "&webpassword=", password.Value, "&webusername=", username.Value));
				base.Fsm.Owner.StartCoroutine(web.Download());
				Log("Downloading from " + urlToPHPFile.Value + "?tag=" + uniqueTag.Value + "&webfilename=" + saveFile.Value);
			}
		}

		public override void OnUpdate()
		{
			if (web.isError)
			{
				errorMessage = web.error;
				errorCode = web.errorCode;
				base.Fsm.Event(isError);
				Finish();
			}
			else
			{
				if (!web.isDone)
				{
					return;
				}
				base.Fsm.Event(isDownloaded);
				Dictionary<string, string> dictionary = web.LoadDictionary<string, string>(_tag);
				if (localFile.Value != string.Empty)
				{
					web.SaveToFile(localFile.Value);
				}
				Log("DownLoaded from " + saveFile.Value + "?tag=" + _tag);
				proxy.hashTable.Clear();
				foreach (string key in dictionary.Keys)
				{
					proxy.hashTable[key] = PlayMakerUtils.ParseValueFromString(dictionary[key]);
				}
				Finish();
			}
		}
	}
}
