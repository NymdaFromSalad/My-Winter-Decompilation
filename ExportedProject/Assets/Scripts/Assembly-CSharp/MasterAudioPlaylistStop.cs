using System.Collections.Generic;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Stop current Playlist in Master Audio")]
public class MasterAudioPlaylistStop : FsmStateAction
{
	[Tooltip("Check this to perform action on all Playlist Controllers")]
	public FsmBool allPlaylistControllers;

	[Tooltip("Name of Playlist Controller containing the Playlist. Not required if you only have one Playlist Controller.")]
	public FsmString playlistControllerName;

	public override void OnEnter()
	{
		if (allPlaylistControllers.Value)
		{
			List<PlaylistController> instances = PlaylistController.Instances;
			for (int i = 0; i < instances.Count; i++)
			{
				MasterAudio.StopPlaylist(instances[i].name);
			}
		}
		else if (string.IsNullOrEmpty(playlistControllerName.Value))
		{
			MasterAudio.StopPlaylist();
		}
		else
		{
			MasterAudio.StopPlaylist(playlistControllerName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allPlaylistControllers = new FsmBool(false);
		playlistControllerName = new FsmString(string.Empty);
	}
}
