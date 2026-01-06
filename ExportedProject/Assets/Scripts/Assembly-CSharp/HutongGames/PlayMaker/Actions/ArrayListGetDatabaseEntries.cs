using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads all filenames the SQL database contains into a PlayMaker Array List Proxy component. http://docs.moodkie.com/easy-save-2/api/es2web-getfilenames/. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	[ActionCategory("Easy Save 2")]
	public class ArrayListGetDatabaseEntries : ArrayListActions
	{
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[ActionSection("Download Set up")]
		[Tooltip("The URL to our ES2.PHP file. See http://www.moodkie.com/easysave/WebSetup.php for more information on setting up ES2Web")]
		[RequiredField]
		public FsmString urlToPHPFile = "http://www.mysite.com/ES2.php";

		[RequiredField]
		[Tooltip("The username that you have specified in your ES2.php file.")]
		public FsmString username = "ES2";

		[RequiredField]
		[Tooltip("The password that you have specified in your ES2.php file.")]
		public FsmString password = string.Empty;

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
			username = string.Empty;
			password = string.Empty;
			urlToPHPFile = "http://www.mysite.com/ES2.php";
			web = null;
			errorMessage = string.Empty;
			errorCode = string.Empty;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DownloadArrayList();
			}
		}

		private void DownloadArrayList()
		{
			if (isProxyValid())
			{
				web = new ES2Web(string.Concat(urlToPHPFile, "?webpassword=", password.Value, "&webusername=", username.Value));
				base.Fsm.Owner.StartCoroutine(web.DownloadFilenames());
				Debug.Log("Downloading from" + urlToPHPFile.Value);
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
			else if (web.isDone)
			{
				base.Fsm.Event(isDownloaded);
				string[] filenames = web.GetFilenames();
				proxy.arrayList.Clear();
				string[] array = filenames;
				foreach (string text in array)
				{
					proxy.arrayList.Add(text);
					Debug.LogWarning(text);
				}
				Finish();
			}
		}
	}
}
