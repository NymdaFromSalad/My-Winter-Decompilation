using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets data from a url and store it in variables, Accept Post variables. See Unity WWW docs for more details.")]
	[ActionCategory("Web")]
	public class WWWPOST : FsmStateAction
	{
		[Tooltip("Url to download data from.")]
		[RequiredField]
		public FsmString url;

		[ActionSection("POST Data")]
		[CompoundArray("POST", "Key", "Value")]
		public FsmString[] postKeys;

		public FsmVar[] postValues;

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets text from the url.")]
		[ActionSection("Results")]
		public FsmString storeText;

		[Tooltip("Gets a Texture from the url.")]
		[UIHint(UIHint.Variable)]
		public FsmTexture storeTexture;

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets a Texture from the url.")]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject storeMovieTexture;

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets a audio from the url.")]
		[ObjectType(typeof(AudioClip))]
		public FsmObject storeAudio;

		[Tooltip("Audio setting: Is it 3d")]
		public FsmBool audio3d;

		[Tooltip("Audio setting: Is it a stream")]
		public FsmBool audioStream;

		[Tooltip("Audio setting: type")]
		public AudioType audioType;

		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;

		[UIHint(UIHint.Variable)]
		[Tooltip("How far the download progressed (0-1).")]
		public FsmFloat progress;

		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		[ActionSection("Events")]
		public FsmEvent isDone;

		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			url = null;
			postKeys = new FsmString[0];
			postValues = new FsmVar[0];
			storeText = null;
			storeTexture = null;
			storeAudio = null;
			audio3d = false;
			audioStream = false;
			audioType = AudioType.UNKNOWN;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(url.Value))
			{
				Finish();
			}
			else if (postKeys.Length > 0)
			{
				WWWForm wWWForm = new WWWForm();
				int num = 0;
				FsmString[] array = postKeys;
				foreach (FsmString fsmString in array)
				{
					string value = fsmString.Value;
					switch (postValues[num].Type)
					{
					case VariableType.Texture:
					{
						Texture2D texture2D = (Texture2D)postValues[num].textureValue;
						wWWForm.AddBinaryData(value, texture2D.EncodeToPNG());
						break;
					}
					default:
						wWWForm.AddField(value, postValues[num].ToString());
						break;
					case VariableType.Unknown:
					case VariableType.Material:
					case VariableType.Object:
						break;
					}
					num++;
				}
				wwwObject = new WWW(url.Value, wWWForm);
			}
			else
			{
				wwwObject = new WWW(url.Value);
			}
		}

		public override void OnUpdate()
		{
			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish();
				base.Fsm.Event(isError);
				return;
			}
			errorString.Value = wwwObject.error;
			if (!string.IsNullOrEmpty(wwwObject.error))
			{
				Finish();
				base.Fsm.Event(isError);
				return;
			}
			progress.Value = wwwObject.progress;
			if (progress.Value.Equals(1f))
			{
				storeText.Value = wwwObject.text;
				storeTexture.Value = wwwObject.texture;
				storeMovieTexture.Value = wwwObject.movie;
				if (!storeAudio.IsNone)
				{
					storeAudio.Value = wwwObject.GetAudioClip(audio3d.Value, audioStream.Value, audioType);
				}
				errorString.Value = wwwObject.error;
				base.Fsm.Event((!string.IsNullOrEmpty(errorString.Value)) ? isError : isDone);
				Finish();
			}
		}

		public override string ErrorCheck()
		{
			FsmVar[] array = postValues;
			foreach (FsmVar fsmVar in array)
			{
				switch (fsmVar.Type)
				{
				case VariableType.Unknown:
				case VariableType.Material:
				case VariableType.Object:
					return string.Concat(fsmVar.Type, " not supported");
				}
			}
			return string.Empty;
		}
	}
}
