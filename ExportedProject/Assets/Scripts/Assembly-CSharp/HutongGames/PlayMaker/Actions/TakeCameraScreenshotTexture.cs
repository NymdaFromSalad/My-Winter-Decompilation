using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11308.0")]
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Saves a Screenshot from the camera. Save as png or jpg.")]
	public class TakeCameraScreenshotTexture : FsmStateAction
	{
		public enum depthSelect
		{
			_24 = 0,
			_16 = 1,
			_0 = 2
		}

		public sealed class RequestHelperTex : MonoBehaviour
		{
			private TakeCameraScreenshotTexture _action;

			public void startEndofFrame(TakeCameraScreenshotTexture action)
			{
				_action = action;
				StartCoroutine("getPictureIENUM");
			}

			private IEnumerator getPictureIENUM()
			{
				yield return new WaitForEndOfFrame();
				_action.takePicture(true);
				Object.Destroy(base.gameObject);
			}
		}

		[Tooltip("The GameObject camera to take picture from (Must have a Camera component).")]
		[RequiredField]
		[ActionSection("Camera")]
		[CheckForComponent(typeof(Camera))]
		public FsmGameObject gameObject;

		[ActionSection("Screen Setup")]
		public FsmInt resWidth;

		public FsmInt resHeight;

		[Tooltip("Automatically get the current resolution - RECOMMENDED")]
		public FsmBool Auto;

		[Tooltip("Use the current resolution - NOT RECOMMENDED")]
		public FsmBool useCurrentRes;

		[ActionSection("Output")]
		public FsmTexture newTexture;

		[ActionSection("Option")]
		public FsmBool inclGui;

		[Tooltip("Must be 0 or 16 or 24 - The precision of the render texture's depth buffer in bits / When 0 is used, then no Z buffer is created by a render texture")]
		private int Depth;

		public depthSelect setDepth;

		private static RequestHelperTex _helperTex;

		public override void Reset()
		{
			gameObject = null;
			Auto = true;
			useCurrentRes = false;
			resWidth = 2560;
			resHeight = 1440;
			newTexture = null;
			inclGui = true;
		}

		public override void OnEnter()
		{
			if (useCurrentRes.Value || Auto.Value)
			{
				getResolutions();
			}
			switch (setDepth)
			{
			case depthSelect._0:
				Depth = 0;
				break;
			case depthSelect._16:
				Depth = 16;
				break;
			case depthSelect._24:
				Depth = 24;
				break;
			}
			if (inclGui.Value)
			{
				_helperTex = new GameObject("RequestHelper").AddComponent<RequestHelperTex>();
				_helperTex.startEndofFrame(this);
			}
		}

		public override void OnLateUpdate()
		{
			if (!inclGui.Value)
			{
				takePicture(true);
			}
		}

		public void takePicture(bool state)
		{
			if (state)
			{
				getPicture();
			}
		}

		public void getPicture()
		{
			RenderTexture renderTexture = new RenderTexture(resWidth.Value, resHeight.Value, Depth);
			gameObject.Value.GetComponent<Camera>().targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(resWidth.Value, resHeight.Value, TextureFormat.RGB24, false);
			gameObject.Value.GetComponent<Camera>().Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, resWidth.Value, resHeight.Value), 0, 0);
			texture2D.Apply();
			gameObject.Value.GetComponent<Camera>().targetTexture = null;
			RenderTexture.active = null;
			Object.Destroy(renderTexture);
			newTexture.Value = texture2D;
			if (inclGui.Value)
			{
				_helperTex = null;
			}
			Finish();
		}

		public void getResolutions()
		{
			if (useCurrentRes.Value)
			{
				resWidth.Value = Screen.currentResolution.width;
				resHeight.Value = Screen.currentResolution.height;
			}
			else
			{
				resWidth.Value = Screen.width;
				resHeight.Value = Screen.height;
			}
		}
	}
}
