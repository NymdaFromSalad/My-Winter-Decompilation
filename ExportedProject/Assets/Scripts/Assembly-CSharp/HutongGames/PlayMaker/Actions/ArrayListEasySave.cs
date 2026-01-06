using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a PlayMaker Array List Proxy component")]
	public class ArrayListEasySave : ArrayListActions
	{
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		[ActionSection("Easy Save Set Up")]
		public FsmString uniqueTag = string.Empty;

		[Tooltip("The name of the file that we'll create to store our data.")]
		[RequiredField]
		public FsmString saveFile = "defaultES2File.txt";

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			uniqueTag = new FsmString
			{
				UseVariable = true
			};
			saveFile = "defaultES2File.txt";
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SaveArrayList();
			}
			Finish();
		}

		public void SaveArrayList()
		{
			if (!isProxyValid())
			{
				return;
			}
			string value = uniqueTag.Value;
			if (string.IsNullOrEmpty(value))
			{
				value = base.Fsm.GameObjectName + "/" + base.Fsm.Name + "/arraylist/" + reference;
			}
			List<string> list = new List<string>();
			foreach (object array in proxy.arrayList)
			{
				list.Add(PlayMakerUtils.ParseValueToString(array));
			}
			ES2.Save(list, saveFile.Value + "?tag=" + uniqueTag);
			Log("Saved to " + saveFile.Value + "?tag=" + uniqueTag);
			Finish();
		}
	}
}
