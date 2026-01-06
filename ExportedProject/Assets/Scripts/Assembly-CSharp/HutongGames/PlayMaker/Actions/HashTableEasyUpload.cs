using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a PlayMaker HashTable Proxy component to MySQL Server via ES2.php file. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	public class HashTableEasyUpload : HashTableActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + hashTable Reference as tag.")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data. Leave as default if unsure.")]
		public FsmString saveFile = "defaultES2File.txt";

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

		[Tooltip("The Event to send if Upload succeeded.")]
		[ActionSection("Result")]
		public FsmEvent isUploaded;

		[Tooltip("The event to send if Upload failed.")]
		public FsmEvent isError;

		[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
		public FsmString errorMessage = string.Empty;

		[Tooltip("Where any error codes thrown will be stored. Set this to a variable, or leave it blank.")]
		public FsmString errorCode = string.Empty;

		private ES2Web web;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
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
				UploadHashTable();
			}
		}

		private void UploadHashTable()
		{
			if (!isProxyValid())
			{
				return;
			}
			string text = uniqueTag.Value;
			if (string.IsNullOrEmpty(text))
			{
				text = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/hashTable/" + reference.Value;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (object key in proxy.hashTable.Keys)
			{
				dictionary[(string)key] = PlayMakerUtils.ParseValueToString(proxy.hashTable[key]);
			}
			web = new ES2Web(string.Concat(urlToPHPFile, "?tag=", text, "&webfilename=", saveFile.Value, "&webpassword=", password.Value, "&webusername=", username.Value));
			base.Fsm.Owner.StartCoroutine(web.Upload(dictionary));
			Log("Uploading to " + urlToPHPFile.Value + "?tag=" + text + "&webfilename=" + saveFile.Value);
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
			else if (web.isDone)
			{
				base.Fsm.Event(isUploaded);
				Finish();
			}
		}
	}
}
