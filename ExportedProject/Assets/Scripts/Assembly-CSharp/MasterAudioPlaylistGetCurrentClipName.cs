using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Get the name of the currently playing Audio Clip in a Playlist in Master Audio")]
public class MasterAudioPlaylistGetCurrentClipName : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Name of Playlist Controller. Not required if you only have one Playlist Controller.")]
	public FsmString playlistControllerName;

	[HutongGames.PlayMaker.Tooltip("Name of Variable to store the current clip name in.")]
	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmString storeResult;

	public override void OnEnter()
	{
		PlaylistController playlistController = null;
		playlistController = (string.IsNullOrEmpty(playlistControllerName.Value) ? MasterAudio.OnlyPlaylistController : PlaylistController.InstanceByName(playlistControllerName.Value));
		AudioClip currentPlaylistClip = playlistController.CurrentPlaylistClip;
		storeResult.Value = ((!(currentPlaylistClip == null)) ? currentPlaylistClip.name : string.Empty);
		Finish();
	}

	public override void Reset()
	{
		playlistControllerName = new FsmString(string.Empty);
		storeResult = new FsmString(string.Empty);
	}
}
