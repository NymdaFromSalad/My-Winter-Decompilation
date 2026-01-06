using System.IO;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get infos from a file by index within a directory.")]
	[ActionCategory("Files")]
	public class GetFileInfoByIndex : FsmStateAction
	{
		[Tooltip("The path of the folder.")]
		[RequiredField]
		public FsmString folderPath;

		[RequiredField]
		[Tooltip("The search pattern. Use * for all")]
		public FsmString searchPattern;

		[Tooltip("The search options. let you search only in the folder or in all sub folders too")]
		public SearchOption searchOption;

		[Tooltip("The number of files in the folder")]
		[RequiredField]
		public FsmInt fileIndex;

		[Tooltip("The datetime format to get datetime properties of the file in the folder")]
		public FsmString dateTimeFormat;

		[Tooltip("The name of the file")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmString fileName;

		[UIHint(UIHint.Variable)]
		[Tooltip("The extension  of the file")]
		public FsmString extension;

		[UIHint(UIHint.Variable)]
		[Tooltip("The full name of the file")]
		public FsmString fullName;

		[UIHint(UIHint.Variable)]
		[Tooltip("The directory name of the file")]
		public FsmString directoryName;

		[UIHint(UIHint.Variable)]
		[Tooltip("The readonly state of the file")]
		public FsmBool isReadOnly;

		[Tooltip("The creation time of the file")]
		[UIHint(UIHint.Variable)]
		public FsmString creationTime;

		[UIHint(UIHint.Variable)]
		[Tooltip("The last access time of the file")]
		public FsmString lastAccessTime;

		[UIHint(UIHint.Variable)]
		[Tooltip("The last write time of the file")]
		public FsmString lastWriteTime;

		[Tooltip("The file size in bytes")]
		[UIHint(UIHint.Variable)]
		public FsmInt fileSize;

		[Tooltip("Event sent if no file found")]
		public FsmEvent IndexOutOfRangeEvent;

		public override void Reset()
		{
			folderPath = null;
			searchPattern = "*";
			searchOption = SearchOption.TopDirectoryOnly;
			fileIndex = 0;
			dateTimeFormat = "dddd, MMMM dd, yyyy h:mm:ss tt";
			IndexOutOfRangeEvent = null;
			directoryName = null;
			extension = null;
			fileName = null;
			fullName = null;
			isReadOnly = null;
			creationTime = null;
			lastAccessTime = null;
			lastWriteTime = null;
			fileSize = null;
		}

		public override void OnEnter()
		{
			DoGetFileInfoByIndex();
			Finish();
		}

		private void DoGetFileInfoByIndex()
		{
			string[] files = Directory.GetFiles(folderPath.Value, searchPattern.Value, searchOption);
			if (files.Length == 0)
			{
				base.Fsm.Event(IndexOutOfRangeEvent);
			}
			if (fileIndex.Value < 0 || fileIndex.Value >= files.Length)
			{
				base.Fsm.Event(IndexOutOfRangeEvent);
			}
			FileInfo fileInfo = new FileInfo(files[fileIndex.Value]);
			if (!fileName.IsNone)
			{
				fileName.Value = fileInfo.Name;
			}
			if (!extension.IsNone)
			{
				extension.Value = Path.GetExtension(fileInfo.FullName);
			}
			if (!directoryName.IsNone)
			{
				directoryName.Value = fileInfo.DirectoryName;
			}
			if (!fullName.IsNone)
			{
				fullName.Value = fileInfo.FullName;
			}
			if (!isReadOnly.IsNone)
			{
				isReadOnly.Value = fileInfo.IsReadOnly;
			}
			if (!creationTime.IsNone)
			{
				creationTime.Value = fileInfo.CreationTime.ToString(dateTimeFormat.Value);
			}
			if (!lastWriteTime.IsNone)
			{
				lastWriteTime.Value = fileInfo.LastWriteTime.ToString(dateTimeFormat.Value);
			}
			if (!lastAccessTime.IsNone)
			{
				lastAccessTime.Value = fileInfo.LastAccessTime.ToString(lastAccessTime.Value);
			}
			if (!fileSize.IsNone)
			{
				fileSize.Value = (int)fileInfo.Length;
			}
		}
	}
}
