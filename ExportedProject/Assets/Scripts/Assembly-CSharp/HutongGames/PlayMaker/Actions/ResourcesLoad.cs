using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads an asset stored at path in a Resources folder. The path is relative to any Resources folder inside the Assets folder of your project, extensions must be omitted.")]
	[ActionCategory("Resources")]
	public class ResourcesLoad : FsmStateAction
	{
		[Tooltip("The path is relative to any Resources folder inside the Assets folder of your project, extensions must be omitted.")]
		[RequiredField]
		public FsmString assetPath;

		[Tooltip("The stored asset")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVar storeAsset;

		public FsmEvent successEvent;

		public FsmEvent failureEvent;

		public override void Reset()
		{
			assetPath = null;
			storeAsset = new FsmVar();
			storeAsset.Type = VariableType.Texture;
		}

		public override void OnEnter()
		{
			bool flag = false;
			try
			{
				flag = loadResource();
			}
			catch (UnityException ex)
			{
				Debug.LogWarning(ex.Message);
			}
			if (flag)
			{
				base.Fsm.Event(successEvent);
			}
			else
			{
				base.Fsm.Event(failureEvent);
			}
			Finish();
		}

		public override string ErrorCheck()
		{
			switch (storeAsset.Type)
			{
			default:
				return "Only GameObject, Texture, AudioClip, and Material are supported";
			case VariableType.GameObject:
			case VariableType.String:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				return string.Empty;
			}
		}

		public bool loadResource()
		{
			switch (storeAsset.Type)
			{
			case VariableType.GameObject:
			{
				GameObject gameObject = (GameObject)Resources.Load(assetPath.Value, typeof(GameObject));
				if (gameObject == null)
				{
					return false;
				}
				GameObject gameObject2 = Object.Instantiate(gameObject);
				if (gameObject2 == null)
				{
					return false;
				}
				FsmGameObject fsmGameObject = base.Fsm.Variables.GetFsmGameObject(storeAsset.variableName);
				fsmGameObject.Value = gameObject2;
				break;
			}
			case VariableType.Texture:
			{
				Texture2D texture2D = (Texture2D)Resources.Load(assetPath.Value, typeof(Texture2D));
				if (texture2D == null)
				{
					return false;
				}
				FsmTexture fsmTexture = base.Fsm.Variables.GetFsmTexture(storeAsset.variableName);
				fsmTexture.Value = texture2D;
				break;
			}
			case VariableType.Material:
			{
				Material material = (Material)Resources.Load(assetPath.Value, typeof(Material));
				if (material == null)
				{
					return false;
				}
				FsmMaterial fsmMaterial = base.Fsm.Variables.GetFsmMaterial(storeAsset.variableName);
				fsmMaterial.Value = material;
				break;
			}
			case VariableType.String:
			{
				TextAsset textAsset = (TextAsset)Resources.Load(assetPath.Value, typeof(TextAsset));
				if (textAsset == null)
				{
					return false;
				}
				FsmString fsmString = base.Fsm.Variables.GetFsmString(storeAsset.variableName);
				fsmString.Value = textAsset.text;
				break;
			}
			case VariableType.Object:
			{
				AudioClip audioClip = (AudioClip)Resources.Load(assetPath.Value, typeof(AudioClip));
				if (audioClip == null)
				{
					return false;
				}
				FsmObject fsmObject = base.Fsm.Variables.GetFsmObject(storeAsset.variableName);
				fsmObject.Value = audioClip;
				break;
			}
			default:
				return false;
			}
			return true;
		}
	}
}
