using System.Collections.Generic;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Play a random clip in a Playlist in Master Audio")]
public class MasterAudioPlaylistClipRandom : FsmStateAction
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
				MasterAudio.TriggerRandomPlaylistClip(instances[i].name);
			}
		}
		else if (string.IsNullOrEmpty(playlistControllerName.Value))
		{
			MasterAudio.TriggerRandomPlaylistClip();
		}
		else
		{
			MasterAudio.TriggerRandomPlaylistClip(playlistControllerName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allPlaylistControllers = new FsmBool(false);
		playlistControllerName = new FsmString(string.Empty);
	}
}
