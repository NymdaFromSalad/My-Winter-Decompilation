using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a PlayMaker Array List Proxy component to MySQL Server via ES2.php file. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	[ActionCategory("Easy Save 2")]
	public class ArrayListEasyUpload : ArrayListActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data. Leave as default if unsure.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

		[RequiredField]
		[Tooltip("The URL to our ES2.PHP file. See http://www.moodkie.com/easysave/WebSetup.php for more information on setting up ES2Web")]
		[ActionSection("Upload Set up")]
		public FsmString urlToPHPFile = "http://www.mysite.com/ES2.php";

		[RequiredField]
		[Tooltip("The username that you have specified in your ES2.php file.")]
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
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				UploadArrayList();
			}
		}

		private void UploadArrayList()
		{
			if (!isProxyValid())
			{
				return;
			}
			string text = uniqueTag.Value;
			if (string.IsNullOrEmpty(text))
			{
				text = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/arraylist/" + reference.Value;
			}
			List<string> list = new List<string>();
			foreach (object array in proxy.arrayList)
			{
				list.Add(PlayMakerUtils.ParseValueToString(array));
			}
			web = new ES2Web(string.Concat(urlToPHPFile, "?tag=", text, "&webfilename=", saveFile.Value, "&webpassword=", password.Value, "&webusername=", username.Value));
			base.Fsm.Owner.StartCoroutine(web.Upload(list));
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
