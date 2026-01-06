using HutongGames.PlayMaker;

[Tooltip("Start a Playlist by name in Master Audio")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioPlaylistStartByName : FsmStateAction
{
	[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
	public FsmString playlistControllerName;

	[Tooltip("Name of playlist to start")]
	[RequiredField]
	public FsmString playlistName;

	public override void OnEnter()
	{
		if (string.IsNullOrEmpty(playlistControllerName.Value))
		{
			MasterAudio.StartPlaylist(playlistName.Value);
		}
		else
		{
			MasterAudio.StartPlaylist(playlistControllerName.Value, playlistName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		playlistControllerName = new FsmString(string.Empty);
		playlistName = new FsmString(string.Empty);
	}
}
