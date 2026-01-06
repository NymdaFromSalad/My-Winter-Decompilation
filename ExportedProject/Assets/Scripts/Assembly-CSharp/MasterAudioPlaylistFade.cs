using System.Collections.Generic;
using HutongGames.PlayMaker;

[Tooltip("Fade the Playlist volume in Master Audio to a specific volume over X seconds.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioPlaylistFade : FsmStateAction
{
	[Tooltip("Check this to perform action on all Playlist Controllers")]
	public FsmBool allPlaylistControllers;

	[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
	public FsmString playlistControllerName;

	[HasFloatSlider(0f, 1f)]
	[Tooltip("Target Playlist Volume")]
	[RequiredField]
	public FsmFloat targetVolume;

	[HasFloatSlider(0f, 10f)]
	[RequiredField]
	[Tooltip("Amount of time to complete fade (seconds)")]
	public FsmFloat fadeTime;

	public override void OnEnter()
	{
		if (allPlaylistControllers.Value)
		{
			List<PlaylistController> instances = PlaylistController.Instances;
			for (int i = 0; i < instances.Count; i++)
			{
				MasterAudio.FadePlaylistToVolume(instances[i].name, targetVolume.Value, fadeTime.Value);
			}
		}
		else if (string.IsNullOrEmpty(playlistControllerName.Value))
		{
			MasterAudio.FadePlaylistToVolume(targetVolume.Value, fadeTime.Value);
		}
		else
		{
			MasterAudio.FadePlaylistToVolume(playlistControllerName.Value, targetVolume.Value, fadeTime.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allPlaylistControllers = new FsmBool(false);
		playlistControllerName = new FsmString(string.Empty);
		targetVolume = new FsmFloat();
		fadeTime = new FsmFloat();
	}
}
