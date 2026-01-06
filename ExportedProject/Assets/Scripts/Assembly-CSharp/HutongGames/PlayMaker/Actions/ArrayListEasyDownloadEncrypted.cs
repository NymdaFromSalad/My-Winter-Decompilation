using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a PlayMaker Array List Proxy component From MySQL Server via ES2.php file. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	[ActionCategory("Easy Save 2")]
	public class ArrayListEasyDownloadEncrypted : ArrayListActions
	{
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file in the online database.")]
		public FsmString saveFile = "defaultES2File.txt";

		[Tooltip("The name of the local file we want to create to store our data. Leave blank if you don't want to store data locally.")]
		public FsmString localFile = string.Empty;

		[Tooltip("Activate Decryption")]
		public FsmBool encryption;

		[Tooltip("the password of the saved item")]
		public FsmString filePassword = string.Empty;

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
			encryption = false;
			filePassword = string.Empty;
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
				DownloadArrayList();
			}
		}

		private void DownloadArrayList()
		{
			if (isProxyValid())
			{
				_tag = uniqueTag.Value;
				if (string.IsNullOrEmpty(_tag))
				{
					_tag = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/arraylist/" + reference.Value;
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
				string text = "false";
				if (encryption.Value)
				{
					text = "true";
				}
				if (localFile.Value != string.Empty)
				{
					web.SaveToFile(string.Concat(localFile.Value, "?tag=", uniqueTag, "&encrypt=", text, "&password=", password.Value));
				}
				Log("Loaded from " + saveFile.Value + "?tag=" + uniqueTag);
				string text2 = "&savelocation=file";
				List<string> list = ES2.LoadList<string>(localFile.Value + "?tag=" + uniqueTag.Value + "&encrypt=" + text + "&password=" + filePassword.Value + text2);
				Log("Loaded from " + localFile.Value + "?tag=" + uniqueTag);
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
}
