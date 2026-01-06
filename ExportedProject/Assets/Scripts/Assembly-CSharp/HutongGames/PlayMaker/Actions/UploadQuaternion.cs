namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Saves a Quaternion to MySQL Server via ES2.php file. See moodkie.com/easysave/WebSetup.php for how to set up MySQL.")]
	[ActionCategory("Easy Save 2")]
	public class UploadQuaternion : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The variable we want to save.")]
		public FsmQuaternion saveValue;

		[RequiredField]
		[Tooltip("The URL to our ES2.PHP file. See http://www.moodkie.com/easysave/WebSetup.php for more information on setting up ES2Web")]
		public FsmString urlToPHPFile = "http://www.mysite.com/ES2.php";

		[RequiredField]
		[Tooltip("The username that you have specified in your ES2.php file.")]
		public FsmString username = "ES2";

		[Tooltip("The password that you have specified in your ES2.php file.")]
		[RequiredField]
		public FsmString password = "65w84e4p994z3Oq";

		[RequiredField]
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data. Leave as default if unsure.")]
		public FsmString saveFile = "defaultES2File.txt";

		[Tooltip("The Event to send if Upload succeeded.")]
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
			uniqueTag = string.Empty;
			saveFile = "defaultES2File.txt";
			saveValue = null;
			urlToPHPFile = "http://www.mysite.com/ES2.php";
			web = null;
			errorMessage = string.Empty;
			errorCode = string.Empty;
		}

		public override void OnEnter()
		{
			web = new ES2Web(string.Concat(urlToPHPFile, "?tag=", uniqueTag, "&webfilename=", saveFile, "&webpassword=", password, "&webusername=", username));
			base.Fsm.Owner.StartCoroutine(web.Upload(saveValue.Value));
			Log(string.Concat("Uploading to ", urlToPHPFile, "?tag=", uniqueTag, "&webfilename=", saveFile));
		}

		public override void OnUpdate()
		{
			if (web.isError)
			{
				errorMessage.Value = web.error;
				errorCode.Value = web.errorCode;
				base.Fsm.Event(isError);
			}
			else if (web.isDone)
			{
				base.Fsm.Event(isUploaded);
			}
		}
	}
}
