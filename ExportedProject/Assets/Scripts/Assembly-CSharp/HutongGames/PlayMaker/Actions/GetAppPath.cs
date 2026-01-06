using System.IO;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get Data Path, Application Folder and persistentDataPath.")]
	[ActionCategory(ActionCategory.Application)]
	public class GetAppPath : FsmStateAction
	{
		[Tooltip("Path Result")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString DataPathResult;

		public FsmString AppFolderResult;

		public FsmString PersistentPathResult;

		[UIHint(UIHint.Variable)]
		[Tooltip("Platform Result Result")]
		public FsmString PlaformResult;

		public override void OnEnter()
		{
			string dataPath = Application.dataPath;
			string persistentDataPath = Application.persistentDataPath;
			string fullPath = Path.GetFullPath(".");
			dataPath += "/";
			persistentDataPath += "/";
			fullPath += "/";
			DataPathResult.Value = dataPath;
			AppFolderResult.Value = fullPath;
			PersistentPathResult.Value = persistentDataPath;
			PlaformResult.Value = Application.platform.ToString();
			Finish();
		}
	}
}
