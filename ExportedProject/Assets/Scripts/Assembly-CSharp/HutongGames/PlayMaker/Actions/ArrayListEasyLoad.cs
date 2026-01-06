using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Loads a PlayMaker Array List Proxy component using EasySave")]
	public class ArrayListEasyLoad : ArrayListActions
	{
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name. Leave to none or empty, to use the GameObject Name + Fsm Name + array Reference as tag.")]
		[ActionSection("Easy Save Set Up")]
		public FsmString uniqueTag = string.Empty;

		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data.")]
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
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				LoadArrayList();
			}
			Finish();
		}

		public void LoadArrayList()
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
			ES2Settings eS2Settings = new ES2Settings();
			if (loadFromResources.Value)
			{
				eS2Settings.saveLocation = ES2Settings.SaveLocation.Resources;
			}
			List<string> list = ES2.LoadList<string>(saveFile.Value + "?tag=" + text);
			Log("Loaded from " + saveFile.Value + "?tag=" + uniqueTag);
			proxy.arrayList.Clear();
			foreach (string item in list)
			{
				proxy.arrayList.Add(PlayMakerUtils.ParseValueFromString(item));
			}
			Finish();
		}
	}
}
